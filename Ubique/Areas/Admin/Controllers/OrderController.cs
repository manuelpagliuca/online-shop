using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Ubique.DataAccess.Repository.IRepository;
using Ubique.Models;

namespace Ubique.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class OrderController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public OrderController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			return View();
		}

		#region API CALLS

		[HttpGet]
		public IActionResult GetAll()
		{
			List<OrderHeader> orderHeadersList = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser").ToList();

			return Json(new { data = orderHeadersList });
		}

		#endregion
	}
}
