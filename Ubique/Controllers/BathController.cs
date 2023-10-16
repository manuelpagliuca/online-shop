using Microsoft.AspNetCore.Mvc;
using Ubique.Data;
using Ubique.Models;

namespace Ubique.Controllers
{
	public class BathController : Controller
	{
		private readonly ApplicationDbContext _db;

		public BathController(ApplicationDbContext db)
		{
			_db = db;
		}

		public IActionResult Index()
		{
			List<Bath> bathComponents = _db.BathComponents.ToList();
			return View();
		}
	}
}
