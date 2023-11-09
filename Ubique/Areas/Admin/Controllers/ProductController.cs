using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ubique.DataAccess.Repository.IRepository;
using Ubique.Models;
using Ubique.Models.ViewModels;
using Ubique.Utility;

namespace Ubique.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = StaticDetails.Role_Admin)]
	public class ProductController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
		{
			_unitOfWork = unitOfWork;
			_webHostEnvironment = webHostEnvironment;
		}

		public IActionResult Index()
		{
			List<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "SubCategory.Category").ToList();

			return View(productList);
		}

		public IActionResult Upsert(int? id)
		{
			ProductVM productVM = new ProductVM();
			productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
			{
				Text = u.Name,
				Value = u.Id.ToString()
			});

			if (id != 0 && id != null)
			{
				// update
				productVM.Product = _unitOfWork.Product.Get(u => u.Id == id, includeProperties: "ProductImages");
				productVM.Product.SubCategory = _unitOfWork.SubCategory.Get(u => u.Id == productVM.Product.SubCategoryId);

				IEnumerable<SubCategory>? subCategories = _unitOfWork.SubCategory.GetList(u => u.CategoryId == productVM.Product.SubCategory.CategoryId);

				if (subCategories != null)
				{
					IEnumerable<SelectListItem> subCategoriesItems =
						subCategories.Select(subCategory => new SelectListItem()
						{
							Text = subCategory.Name,
							Value = subCategory.Id.ToString()
						});

					productVM.SubCategoryList = subCategoriesItems;
				}
			}
			else
			{
				// insert
				productVM.Product = new Product();
				productVM.Product.SubCategory = new SubCategory();
				productVM.Product.SubCategory.Category = new Category();
			}

			return View(productVM);
		}

		[HttpPost]
		public IActionResult Upsert(ProductVM productVM, List<IFormFile> files)
		{
			if (productVM.Product.IsValid()) // TODO: ModelState.IsValid is a better option
			{
				if (productVM.Product.Id == 0)
				{
					_unitOfWork.Product.Add(productVM.Product);
				}
				else
				{
					_unitOfWork.Product.Update(productVM.Product);
				}

				_unitOfWork.Save();

				string wwwRootPath = _webHostEnvironment.WebRootPath;

				if (files != null)
				{
					foreach (IFormFile file in files)
					{
						string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
						string productPath = @"images\products\product-" + productVM.Product.Id;
						string finalPath = Path.Combine(wwwRootPath, productPath);

						if (!Directory.Exists(finalPath))
						{
							Directory.CreateDirectory(finalPath);
						}

						using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
						{
							file.CopyTo(fileStream);
						}

						ProductImage productImage = new()
						{
							ImageUrl = @"\" + productPath + @"\" + fileName,
							ProductId = productVM.Product.Id,
						};

						if (productVM.Product.ProductImages == null)
						{
							productVM.Product.ProductImages = new List<ProductImage>();
						}

						productVM.Product.ProductImages.Add(productImage);
					}

					_unitOfWork.Product.Update(productVM.Product);
					_unitOfWork.Save();
				}

				TempData["success"] = productVM.Product.Name + " è un nuovo Prodotto.";

				return RedirectToAction("Index");
			}
			else
			{
				productVM.CategoryList = _unitOfWork.Category
					.GetAll()
					.ToList()
					.Select(u => new SelectListItem
					{
						Text = u.Name,
						Value = u.Id.ToString()
					});

				productVM.SubCategoryList = _unitOfWork.SubCategory
					.GetList(u => u.CategoryId == productVM.Product.SubCategoryId)
					.ToList()
					.Select(u => new SelectListItem
					{
						Text = u.Name,
						Value = u.Id.ToString()
					});

				return View(productVM);
			}
		}

		public IActionResult DeleteImage(int imageId)
		{
			var imageToBeDeleted = _unitOfWork.ProductImage.Get(u => u.Id == imageId);
			int productId = imageToBeDeleted.ProductId;

			if (imageToBeDeleted != null)
			{
				if (!string.IsNullOrEmpty(imageToBeDeleted.ImageUrl))
				{
					var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, imageToBeDeleted.ImageUrl.TrimStart('\\'));

					if (!string.IsNullOrEmpty(oldImagePath) && System.IO.File.Exists(oldImagePath))
					{
						System.IO.File.Delete(oldImagePath);
					}
				}

				_unitOfWork.ProductImage.Remove(imageToBeDeleted);
				_unitOfWork.Save();

				TempData["success"] = "Immagine rimossa.";
			}

			return RedirectToAction(nameof(Upsert), new { id = productId });
		}

		[HttpGet]
		public IActionResult GetSubCategoriesBasedOnCategory(int id)
		{
			IEnumerable<SubCategory> subCategories = _unitOfWork.SubCategory.GetList(u => u.CategoryId == id);
			IEnumerable<SelectListItem> listItems = subCategories.Select(subCategory => new SelectListItem
			{
				Text = subCategory.Name,
				Value = subCategory.Id.ToString()
			});

			return new JsonResult(listItems);
		}

		public IActionResult Edit(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}

			Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id);

			if (productFromDb == null)
			{
				return NotFound();
			}

			return View(productFromDb);
		}

		[HttpPost]
		public IActionResult Edit(Product product)
		{
			if (ModelState.IsValid)
			{
				_unitOfWork.Product.Update(product);
				_unitOfWork.Save();
				TempData["success"] = product.Name + " è stato aggiornato.";
				return RedirectToAction("Index");
			}

			return View();
		}

		#region API CALLS

		[HttpGet]
		public IActionResult GetAll()
		{
			List<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "SubCategory.Category").ToList();

			return Json(new { data = productList });
		}

		[HttpDelete]
		public IActionResult Delete(int? id)
		{
			var productToBeDeleted = _unitOfWork.Product.Get(u => u.Id == id);

			if (productToBeDeleted == null)
			{
				return Json(new { success = false, message = "Errore durante la rimozione." });
			}

			string productPath = @"images\products\product-" + id;
			string finalPath = Path.Combine(_webHostEnvironment.WebRootPath, productPath);

			if (Directory.Exists(finalPath))
			{
				string[] filePaths = Directory.GetFiles(finalPath);

				foreach (string filePath in filePaths)
				{
					System.IO.File.Delete(filePath);
				}

				Directory.Delete(finalPath);
			}

			_unitOfWork.Product.Remove(productToBeDeleted);
			_unitOfWork.Save();

			return Json(new { success = true, message = productToBeDeleted.Name + " è stato cancellato." });
		}
		#endregion
	}
}