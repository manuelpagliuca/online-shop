using Microsoft.AspNetCore.Mvc;
using Ubique.DataAccess.Data;
using Ubique.DataAccess.Repository;
using Ubique.DataAccess.Repository.IRepository;
using Ubique.Models;
using Ubique.Models.ViewModels;

namespace Ubique.Controllers
{
	public class SubCategoryController : Controller
	{
		private readonly ISubCategoryRepository _subCategoryRepository;
		private readonly ICategoryRepository _categoryRepository;

		public SubCategoryController(ISubCategoryRepository subCategoryRepository, ICategoryRepository categoryRepository)
		{
			_subCategoryRepository = subCategoryRepository;
			_categoryRepository = categoryRepository;
		}

		public IActionResult Index()
		{
			List<SubCategory> subCategories = _subCategoryRepository.GetAll().ToList();

			foreach (SubCategory subCategory in subCategories)
			{
				subCategory.Category = _categoryRepository.Get(u => u.Id == subCategory.CategoryId);
			}

			return View(subCategories);
		}

		public IActionResult Create()
		{
			SubCategoryVM viewModel = new()
			{
				SubCategory = new SubCategory(),
				Categories = _categoryRepository.GetAll().ToList()
			};

			return View(viewModel);
		}

		[HttpPost]
		public IActionResult Create(SubCategory subCategory)
		{
			Category? category = _categoryRepository.Get(u => u.Id == subCategory.CategoryId);

			if (category != null)
			{
				subCategory.Category = category;
				_subCategoryRepository.Add(subCategory);
				_subCategoryRepository.Save();
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

			SubCategory? subCategory = _subCategoryRepository.Get(u => u.Id == id);

			if (subCategory == null)
			{
				return NotFound();
			}

			SubCategoryVM viewModel = new()
			{
				SubCategory = subCategory,
				Categories = _categoryRepository.GetAll().ToList()
			};

			return View(viewModel);
		}

		[HttpPost]
		public IActionResult Edit(SubCategoryVM viewModel)
		{
			viewModel.Categories = _categoryRepository.GetAll().ToList();
			viewModel.SubCategory.Category = _categoryRepository.Get(u => u.Id == viewModel.SubCategory.CategoryId);

			if (viewModel.SubCategory != null)
			{
				_subCategoryRepository.Update(viewModel.SubCategory);
				_subCategoryRepository.Save();
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

			SubCategory? subCategoryFromDb = _subCategoryRepository.Get(u => u.Id == id);

			if (subCategoryFromDb == null)
			{
				return NotFound();
			}

			return View(subCategoryFromDb);
		}

		[HttpPost, ActionName("Delete")]
		public IActionResult DeletePOST(int? id)
		{
			SubCategory? subCategory = _subCategoryRepository.Get(u => u.Id == id);

			if (subCategory == null)
			{
				return NotFound();
			}

			_subCategoryRepository.Remove(subCategory);
			_subCategoryRepository.Save();
			TempData["success"] = "SottoCategoria rimossa con successo!";
			return RedirectToAction("Index");
		}
	}
}
