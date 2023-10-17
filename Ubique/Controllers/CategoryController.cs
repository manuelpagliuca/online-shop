using Microsoft.AspNetCore.Mvc;
using Ubique.Data;
using Ubique.Models;

namespace Ubique.Controllers
{
	public class CategoryController : Controller
	{
		private readonly ApplicationDbContext _db;

		public CategoryController(ApplicationDbContext db)
		{
			_db = db;
		}

		public IActionResult Index()
		{
			List<Category> categories = _db.Categories.ToList();
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
				// If the custom validation is not passing, then we add an error to the model, so that it is possible to display it through helpers-tag.
				ModelState.AddModelError("Name", "Il campo \"Ordine Visualizzazione\" non può essere uguale al campo \"Nome Categoria\".");
			}

			if (ModelState.IsValid)
			{
				_db.Categories.Add(obj);
				_db.SaveChanges();
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

			Category? fromDb = _db.Categories.Find(id);

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
				_db.Categories.Update(obj);
				_db.SaveChanges();
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

			Category? categoryFromDb = _db.Categories.Find(id);

			if (categoryFromDb == null)
			{
				return NotFound();
			}

			return View(categoryFromDb);
		}

		[HttpPost, ActionName("Delete")]
		public IActionResult DeletePOST(int? id)
		{
			Category? obj = _db.Categories.Find(id);

			if (obj == null)
			{
				return NotFound();
			}

			_db.Categories.Remove(obj);
			_db.SaveChanges();
			TempData["success"] = "Categoria rimossa con successo!";
			return RedirectToAction("Index");
		}
	}
}
