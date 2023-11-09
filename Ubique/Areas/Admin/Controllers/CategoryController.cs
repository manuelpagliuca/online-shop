using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubique.DataAccess.Repository.IRepository;
using Ubique.Models;
using Ubique.Utility;

namespace Ubique.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = StaticDetails.Role_Admin)]
	public class CategoryController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public CategoryController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			List<Category> categories = _unitOfWork.Category.GetAll().ToList();
			return View(categories);
		}

		public IActionResult Create()
		{
			return View();
		}

		public IActionResult Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			Category? fromDb = _unitOfWork.Category.Get(u => u.Id == id);

			if (fromDb == null)
			{
				return NotFound();
			}

			return View(fromDb);
		}
		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}

			Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);

			if (categoryFromDb == null)
			{
				return NotFound();
			}

			return View(categoryFromDb);
		}

		#region API
		[HttpGet]
		public IActionResult GetAll()
		{
			return Json(new { data = _unitOfWork.Category.GetAll().ToList() });
		}

		[HttpPost]
		public IActionResult Create(Category category)
		{
			if (ModelState.IsValid)
			{
				_unitOfWork.Category.Add(category);
				_unitOfWork.Save();
				TempData["success"] = category.Name + "è una nuova Categoria.";
				return RedirectToAction("Index", "Category");
			}

			return View();
		}

		[HttpPost]
		public IActionResult Edit(Category obj)
		{
			if (ModelState.IsValid)
			{
				_unitOfWork.Category.Update(obj);
				_unitOfWork.Save();
				TempData["success"] = "Categoria aggiornata con successo!";

				return RedirectToAction("Index", "Category");
			}

			return View();
		}

		[HttpDelete, ActionName("Delete")]
		public IActionResult DeleteREST(int? id)
		{
			Category? category = _unitOfWork.Category.Get(u => u.Id == id);

			if (category == null)
			{
				return NotFound();
			}

			_unitOfWork.Category.Remove(category);
			_unitOfWork.Save();

			return Json(new { message = category.Name +  "è stata rimossa." });
		}

		#endregion
	}
}