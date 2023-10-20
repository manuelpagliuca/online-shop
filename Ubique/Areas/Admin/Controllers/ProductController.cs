using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ubique.DataAccess.Repository.IRepository;
using Ubique.Models;
using Ubique.Models.ViewModels;

namespace Ubique.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ProductController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public ProductController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			List<Product> productList = _unitOfWork.Product.GetAll().ToList();

			foreach (Product product in productList)
			{
				SubCategory subCategory = _unitOfWork.SubCategory.Get(u => u.Id == product.SubCategoryId);
				subCategory.Category = _unitOfWork.Category.Get(u => u.Id == subCategory.CategoryId);
				product.SubCategory = subCategory;
			}			

			return View(productList);
		}

		public IActionResult Create()
		{
			ProductVM productVM = new()
			{
				CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.Id.ToString()
				}),

				SubCategoryList = _unitOfWork.SubCategory.GetAll().Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.Id.ToString()
				}),
				Product = new Product()
			};

			productVM.Product.SubCategory = new SubCategory();

			return View(productVM);
		}

		[HttpPost]
		public IActionResult Create(ProductVM productVM)
		{
			SubCategory subCategory = _unitOfWork.SubCategory.Get(u => u.Id == productVM.Product.SubCategory.Id);
			Category category = _unitOfWork.Category.Get(u => u.Id == productVM.Product.SubCategory.CategoryId);

			productVM.Product.SubCategory = subCategory;
			productVM.Product.SubCategory.Category = category;

			if (subCategory != null && category != null)
			{
				_unitOfWork.Product.Add(productVM.Product);
				_unitOfWork.Save();
				TempData["success"] = "Prodotto creato con successo!";
				return RedirectToAction("Index");
			}
			else
			{
				ModelState.AddModelError("ProductVM", "Categoria/Sotto Categoria non valida");

				productVM.SubCategoryList = _unitOfWork.SubCategory.GetAll().Select(u => new SelectListItem
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

		public IActionResult Delete(int? id)
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

		[HttpPost, ActionName("Delete")]
		public IActionResult DeletePOST(int? id)
		{
			Product? obj = _unitOfWork.Product.Get(u => u.Id == id);

			if (obj == null)
			{
				return NotFound();
			}

			_unitOfWork.Product.Remove(obj);
			_unitOfWork.Save();
			TempData["success"] = "Prodotto rimosso con successo!";
			return RedirectToAction("Index");
		}
	}
}