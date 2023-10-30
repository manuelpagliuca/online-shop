using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ubique.DataAccess.Repository.IRepository;
using Ubique.Models;
using Ubique.Models.ViewModels;

namespace Ubique.Areas.Admin.Controllers
{
	[Area("Admin")]
	//[Authorize(Roles = StaticDetails.Role_Admin)]
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
				productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
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
		public IActionResult Upsert(ProductVM productVM, IFormFile? file)
		{
			if (productVM.Product.IsValid()) // TODO: ModelState.IsValid is a better option
			{
				string wwwRootPath = _webHostEnvironment.WebRootPath;

				if (file != null)
				{
					string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
					string productPath = Path.Combine(wwwRootPath, @"images\product");

					if (productVM.Product.Id != 0)
					{
						string oldImagePath = Path.Combine(wwwRootPath, _unitOfWork.Product.Get(u => u.Id == productVM.Product.Id).ImageUrl.TrimStart('\\'));

						if (!string.IsNullOrEmpty(oldImagePath) && System.IO.File.Exists(oldImagePath))
						{
							System.IO.File.Delete(oldImagePath);
						}
					}

					using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
					{
						file.CopyTo(fileStream);
					}

					productVM.Product.ImageUrl = @"\images\product\" + fileName;
				}

				if (productVM.Product.Id == 0)
				{
					productVM.Product.SubCategory = _unitOfWork.SubCategory.Get(u => u.Id == productVM.Product.SubCategoryId);
					productVM.Product.SubCategory.Category = _unitOfWork.Category.Get(u => u.Id == productVM.Product.SubCategory.CategoryId);
				}

				if (productVM.Product.Id == 0)
				{
					_unitOfWork.Product.Add(productVM.Product);
				}
				else
				{
					_unitOfWork.Product.Update(productVM.Product);
				}

				_unitOfWork.Save();
				TempData["success"] = "Prodotto creato con successo!";

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
		public IActionResult Edit(Product obj)
		{
			if (ModelState.IsValid)
			{
				_unitOfWork.Product.Update(obj);
				_unitOfWork.Save();
				TempData["success"] = "Prodotto aggiornato con successo!";
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
				return Json(new { success = false, message = "Error while deleting" });
			}

			var oldImagePath = Path.Combine(
				_webHostEnvironment.WebRootPath,
				productToBeDeleted.ImageUrl.TrimStart('\\'));

			if (!string.IsNullOrEmpty(oldImagePath) && System.IO.File.Exists(oldImagePath))
			{
				System.IO.File.Delete(oldImagePath);
			}

			_unitOfWork.Product.Remove(productToBeDeleted);
			_unitOfWork.Save();

			return Json(new { success = true, message = "Delete Successful" });
		}

		#endregion
	}
}