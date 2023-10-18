using Microsoft.AspNetCore.Mvc;
using Ubique.DataAccess.Repository.IRepository;
using Ubique.Models;

namespace Ubique.Controllers
{
	public class CategoryController : Controller
	{
		private readonly ICategoryRepository _categoryRepository;

		public CategoryController(ICategoryRepository categoryRepository)
		{
			_categoryRepository = categoryRepository;
		}

		public IActionResult Index()
		{
			List<Category> categories = _categoryRepository.GetAll().ToList();
			return View(categories);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(Category obj)
		{
			if (obj.Name == obj.DisplayOrder.ToString())
			{
				ModelState.AddModelError("Name", "Il campo \"Ordine Visualizzazione\" non può essere uguale al campo \"Nome Categoria\".");
			}

			if (ModelState.IsValid)
			{
				_categoryRepository.Add(obj);
				_categoryRepository.Save();
				TempData["success"] = "Categoria creata con successo!";
				return RedirectToAction("Index", "Category");
			}

			return View();
		}

		public IActionResult Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			Category? fromDb = _categoryRepository.Get(u => u.Id == id);

			if (fromDb == null)
			{
				return NotFound();
			}

			return View(fromDb);
		}

		[HttpPost]
		public IActionResult Edit(Category obj)
		{
			if (ModelState.IsValid)
			{
				_categoryRepository.Update(obj);
				_categoryRepository.Save();
				TempData["success"] = "Categoria aggiornata con successo!";
				return RedirectToAction("Index", "Category");
			}

			return View();
		}

		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}

			Category? categoryFromDb = _categoryRepository.Get(u => u.Id == id);

			if (categoryFromDb == null)
			{
				return NotFound();
			}

			return View(categoryFromDb);
		}

		[HttpPost, ActionName("Delete")]
		public IActionResult DeletePOST(int? id)
		{
			Category? obj = _categoryRepository.Get(u => u.Id == id);

			if (obj == null)
			{
				return NotFound();
			}

			_categoryRepository.Remove(obj);
			_categoryRepository.Save();
			TempData["success"] = "Categoria rimossa con successo!";
			return RedirectToAction("Index");
		}
	}
}
