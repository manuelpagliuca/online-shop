using Microsoft.AspNetCore.Mvc;
using Ubique.Data;
using Ubique.Models;

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
			List<Category> subCategories = _db.Categories.ToList();
			return View(subCategories);
		}
	}
}
