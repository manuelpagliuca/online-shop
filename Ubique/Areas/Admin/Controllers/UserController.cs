using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ubique.DataAccess.Repository.IRepository;
using Ubique.Models;
using Ubique.Models.ViewModels;
using Ubique.Utility;

namespace Ubique.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = StaticDetails.Role_Admin)]
	public class UserController : Controller
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IUnitOfWork _unitOfWork;
		public UserController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IUnitOfWork unitOfWork)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_unitOfWork = unitOfWork;
		}
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult RoleManagment(string userId)
		{
			RoleManagmentVM RoleVM = new RoleManagmentVM()
			{
				ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId, includeProperties: "Company"),
				RoleList = _roleManager.Roles.Select(i => new SelectListItem
				{
					Text = i.Name,
					Value = i.Name
				}),
				CompanyList = _unitOfWork.Company.GetAll().Select(i => new SelectListItem
				{
					Text = i.Name,
					Value = i.Id.ToString()
				}),
			};

			RoleVM.ApplicationUser.Role = _userManager
				.GetRolesAsync(_unitOfWork.ApplicationUser.Get(u => u.Id == userId))
				.GetAwaiter().GetResult().FirstOrDefault();

			return View(RoleVM);
		}

		#region API CALLS

		[HttpPost]
		public IActionResult RoleManagment(RoleManagmentVM roleManagementVM)
		{
			string oldRole = _userManager
				.GetRolesAsync(_unitOfWork.ApplicationUser.Get(u => u.Id == roleManagementVM.ApplicationUser.Id))
				.GetAwaiter().GetResult().FirstOrDefault();

			ApplicationUser applicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == roleManagementVM.ApplicationUser.Id);

			if (!(roleManagementVM.ApplicationUser.Role == oldRole))
			{
				// a role was updated
				if (roleManagementVM.ApplicationUser.Role == StaticDetails.Role_Company)
				{
					applicationUser.CompanyId = roleManagementVM.ApplicationUser.CompanyId;
				}
				if (oldRole == StaticDetails.Role_Company)
				{
					applicationUser.CompanyId = null;
				}

				_unitOfWork.ApplicationUser.Update(applicationUser);
				_unitOfWork.Save();

				_userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();
				_userManager.AddToRoleAsync(applicationUser, roleManagementVM.ApplicationUser.Role).GetAwaiter().GetResult();
			}
			else
			{
				if (oldRole == StaticDetails.Role_Company && applicationUser.CompanyId != roleManagementVM.ApplicationUser.CompanyId)
				{
					applicationUser.CompanyId = roleManagementVM.ApplicationUser.CompanyId;
					_unitOfWork.ApplicationUser.Update(applicationUser);
					_unitOfWork.Save();
				}
			}

			TempData["success"] = "Utente aggiornato.";

			return RedirectToAction("Index");
		}

		[HttpGet]
		public IActionResult GetAll()
		{
			List<ApplicationUser> userList = _unitOfWork.ApplicationUser.GetAll(includeProperties: "Company").ToList();

			// if not a company user
			foreach (var user in userList)
			{
				user.Role = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().FirstOrDefault();

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
			var fromDb = _unitOfWork.ApplicationUser.Get(u => u.Id == id);

			if (fromDb == null)
			{
				return Json(new { success = false, message = "Errore durante il Locking/Unlocking." });
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

			_unitOfWork.ApplicationUser.Update(fromDb);
			_unitOfWork.Save();

			return Json(new { success = true, message = "Operazione Compiuta" });
		}

		#endregion
	}
}
