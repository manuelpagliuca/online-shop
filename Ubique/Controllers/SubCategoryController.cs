using Microsoft.AspNetCore.Mvc;
using Ubique.DataAccess.Data;
using Ubique.Models;
using Ubique.Models.ViewModels;

namespace Ubique.Controllers
{
	public class SubCategoryController : Controller
	{
		private readonly ApplicationDbContext _db;

		public SubCategoryController(ApplicationDbContext db)
		{
			_db = db;
		}

		public IActionResult Index()
		{
			List<SubCategory> subCategories = _db.SubCategories.ToList();

			foreach (SubCategory subCategory in subCategories)
			{
				subCategory.Category = _db.Categories.Find(subCategory.CategoryId);
			}

			return View(subCategories);
		}

		public IActionResult Create()
		{
			SubCategoryVM viewModel = new()
			{
				SubCategory = new SubCategory(),
				Categories = _db.Categories.ToList()
			};

			return View(viewModel);
		}

		[HttpPost]
		public IActionResult Create(SubCategory subCategory)
		{
			Category? category = _db.Categories.Find(subCategory.CategoryId);

			if (category != null)
			{
				subCategory.Category = category;
				_db.SubCategories.Add(subCategory);
				_db.SaveChanges();
				TempData["success"] = "SottoCategoria creata con successo!";
				return RedirectToAction("Index", "SubCategory");
			}
			else
			{
				ModelState.AddModelError("SubCategory", "Categoria non valida");
			}

			return View(subCategory);
		}

		public IActionResult Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			SubCategory? subCategory = _db.SubCategories.Find(id);

			if (subCategory == null)
			{
				return NotFound();
			}

			SubCategoryVM viewModel = new()
			{
				SubCategory = subCategory,
				Categories = _db.Categories.ToList()
			};

			return View(viewModel);
		}

		[HttpPost]
		public IActionResult Edit(SubCategoryVM viewModel)
		{
			viewModel.Categories = _db.Categories.ToList();
			viewModel.SubCategory.Category = _db.Categories.Find(viewModel.SubCategory.CategoryId);

			if (viewModel.SubCategory != null)
			{
				_db.SubCategories.Update(viewModel.SubCategory);
				_db.SaveChanges();
				TempData["success"] = "SottoCategoria aggiornata con successo!";
				return RedirectToAction("Index", "SubCategory");
			}

			return View(viewModel);
		}

		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}

			SubCategory? subCategoryFromDb = _db.SubCategories.Find(id);

			if (subCategoryFromDb == null)
			{
				return NotFound();
			}

			return View(subCategoryFromDb);
		}

		[HttpPost, ActionName("Delete")]
		public IActionResult DeletePOST(int? id)
		{
			SubCategory? subCategory = _db.SubCategories.Find(id);

			if (subCategory == null)
			{
				return NotFound();
			}

			_db.SubCategories.Remove(subCategory);
			_db.SaveChanges();
			TempData["success"] = "SottoCategoria rimossa con successo!";
			return RedirectToAction("Index");
		}
	}
}
