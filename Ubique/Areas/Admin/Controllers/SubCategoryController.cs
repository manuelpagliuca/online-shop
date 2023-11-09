using Microsoft.AspNetCore.Mvc;
using Ubique.DataAccess.Repository.IRepository;
using Ubique.Models;
using Ubique.Models.ViewModels;

namespace Ubique.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class SubCategoryController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public SubCategoryController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			List<SubCategory> subCategories = _unitOfWork.SubCategory.GetAll().ToList();

			foreach (SubCategory subCategory in subCategories)
			{
				subCategory.Category = _unitOfWork.Category.Get(u => u.Id == subCategory.CategoryId);
			}

			return View(subCategories);
		}

		public IActionResult Create()
		{
			SubCategoryVM viewModel = new()
			{
				SubCategory = new SubCategory(),
				Categories = _unitOfWork.Category.GetAll().ToList()
			};

			return View(viewModel);
		}

		public IActionResult Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			SubCategory? subCategory = _unitOfWork.SubCategory.Get(u => u.Id == id);

			if (subCategory == null)
			{
				return NotFound();
			}

			SubCategoryVM viewModel = new()
			{
				SubCategory = subCategory,
				Categories = _unitOfWork.Category.GetAll().ToList()
			};

			return View(viewModel);
		}

		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}

			SubCategory? subCategoryFromDb = _unitOfWork.SubCategory.Get(u => u.Id == id);

			if (subCategoryFromDb == null)
			{
				return NotFound();
			}

			return View(subCategoryFromDb);
		}


		#region API
		[HttpGet]
		public IActionResult GetAll()
		{
			return Json(new { data = _unitOfWork.SubCategory.GetAll(includeProperties:"Category").ToList() });
		}

		[HttpPost]
		public IActionResult Create(SubCategory subCategory)
		{
			if (ModelState.IsValid)
			{
				_unitOfWork.SubCategory.Add(subCategory);
				_unitOfWork.Save();
				TempData["success"] = subCategory.Name + "è una nuova Sotto Categoria.";
				return RedirectToAction("Index", "SubCategory");
			}
			else
			{
				ModelState.AddModelError("SubCategory", "Categoria non valida.");
			}

			return View(subCategory);
		}


		[HttpPost]
		public IActionResult Edit(SubCategoryVM viewModel)
		{
			if (ModelState.IsValid)
			{
				_unitOfWork.SubCategory.Update(viewModel.SubCategory);
				_unitOfWork.Save();
				TempData["success"] = "Sotto Categoria aggiornata con successo!";
				return RedirectToAction("Index", "SubCategory");
			}

			return View(viewModel);
		}

		[HttpDelete, ActionName("Delete")]
		public IActionResult DeleteREST(int? id)
		{
			SubCategory? subCategory = _unitOfWork.SubCategory.Get(u => u.Id == id);

			if (subCategory == null)
			{
				return NotFound();
			}

			_unitOfWork.SubCategory.Remove(subCategory);
			_unitOfWork.Save();

			return Json(new { message = subCategory.Name + " è stata rimossa." });
		}
		#endregion
	}
}
