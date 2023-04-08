using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.Domain.Repositories.Interfaces;
using MyShop.Models.ViewModels;

namespace MyShop.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Route("Admin")]
	public class HomeController : Controller
	{
		private readonly ICarInfoRepository carInfo;

		public HomeController(ICarInfoRepository carInfo)
		{
			this.carInfo = carInfo;
		}

		[Authorize(Roles = "admin")]
		public IActionResult Index()
		{
			return View(carInfo.GetCarInfo());
		}

		[HttpPost]
		[Authorize(Roles = "admin")]
		public IActionResult EditQuantity(QuantityViewModel model, Guid id)
		{
			carInfo.SetQuantity(id, model.Quantity);
			return RedirectToAction("Index");
		}

	}
}
