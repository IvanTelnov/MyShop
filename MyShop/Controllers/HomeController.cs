using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Domain;
using MyShop.Domain.Entities;
using MyShop.Domain.Repositories.Interfaces;
using MyShop.Services;
using System.Security.Claims;

namespace MyShop.Controllers
{
	public class HomeController : Controller
	{
		private readonly ICarInfoRepository cars;
		private readonly AppDbContext context;
		private readonly IHttpContextAccessor httpContextAccessor;

		public HomeController(ICarInfoRepository cars, AppDbContext context, IHttpContextAccessor httpContextAccessor)
		{
			this.cars = cars;
			this.context = context;
			this.httpContextAccessor = httpContextAccessor;
		}

		public IActionResult Index(string codeWord, string searchString)
		{
			if(!string.IsNullOrEmpty(searchString))
			{
				return View("Index", cars.GetCarInfo().Where(c => c.Name.ToLower().Contains(searchString.ToLower())));
			}
			else if (codeWord != "Все" && codeWord != null)
				return View("Index", cars.GetCarInfo().Where(c => c.Category == codeWord));
			else
				return View("Index",cars.GetCarInfo());
		}

		public IActionResult Show(Guid id)
		{
			Cart? cart = httpContextAccessor?.HttpContext?.Session.GetCart();
            if (id != default)
            {
				ViewBag.IsExist = false;
				if (cart != null)
				{
					CartItem? item = cart?.Items?.FirstOrDefault(c => c.CarId == id);

					if (item != null)
					{
						ViewBag.IsExist = true;
						ViewBag.Quantity = item.Quantity;
					}
				}

				return View("Show", cars.GetCarInfoById(id));
            }
			else
			{
                return View("Index", cars.GetCarInfo());
            }
        }

	}
}
