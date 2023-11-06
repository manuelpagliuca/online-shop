using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ubique.DataAccess.Data;
using Ubique.DataAccess.Repository.IRepository;
using Ubique.Models;
using Ubique.Utility;

namespace Ubique.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = StaticDetails.Role_Admin)]
	public class UserController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ApplicationDbContext _db;

		public UserController(IUnitOfWork unitOfWork, ApplicationDbContext db)
		{
			_unitOfWork = unitOfWork;
			_db = db;
		}
		public IActionResult Index()
		{
			return View();
		}

		#region API CALLS

		[HttpGet]
		public IActionResult GetAll()
		{
			List<ApplicationUser> userList = _db.ApplicationUsers.Include(u => u.Company).ToList();
			var userRole = _db.UserRoles.ToList();
			var roles = _db.Roles.ToList();
			// if not a company user
			foreach (var user in userList)
			{
				var roleId = userRole.FirstOrDefault(u => u.UserId == user.Id).RoleId;
				user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;

				if (user.Company == null)
				{
					user.Company = new() { Name = "" };
				}
			}


			return Json(new { data = userList });
		}

		[HttpPost]
		public IActionResult LockUnlock([FromBody] string? id)
		{
			var fromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);

			if (fromDb == null)
			{
				return Json(new { success = false, message = "Error while Locking/Unlocking" });
			}

			if (fromDb.LockoutEnd != null && fromDb.LockoutEnd > DateTime.Now)
			{
				// user is currently locked and it needs to be unlocked
				fromDb.LockoutEnd = DateTime.Now;
			}
			else
			{
				fromDb.LockoutEnd = DateTime.Now.AddYears(1000);
			}

			_db.SaveChanges();

			return Json(new { success = true, message = "Operazione Compiuta" });
		}

		#endregion
	}
}
