using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Ubique.DataAccess.Repository.IRepository;
using Ubique.Models;
using Ubique.Utility;

namespace Ubique.Areas.Customer.Controllers
{
	[Area("Customer")]
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IUnitOfWork _unitOfWork;

		public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
		{
			_logger = logger;
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			ClaimsIdentity? claimsIdentity = (ClaimsIdentity)User.Identity;
			Claim? claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

			if (claim != null)
			{
				List<ShoppingCart>? carts = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value).ToList();
				int itemsCount = 0;

				foreach (ShoppingCart? itemCart in carts)
				{
					itemsCount += itemCart.Count;
				}

				HttpContext.Session.SetInt32(StaticDetails.SessionCart, itemsCount);
			}

			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}