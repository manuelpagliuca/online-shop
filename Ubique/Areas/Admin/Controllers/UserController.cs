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
				var role = roles.FirstOrDefault(u => u.Id == roleId).Name;

				if (user.Company == null)
				{
					user.Company = new() { Name = "" };
				}
			}


			return Json(new { data = userList });
		}

		[HttpDelete]
		public IActionResult Delete(int? id)
		{


			return Json(new { success = true, message = "Delete Successful" });
		}

		#endregion
	}
}
