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
			return View();
		}
	}
}
