using Microsoft.AspNetCore.Mvc;
using MyShop.Domain.Entities;
using MyShop.Domain.Repositories.Interfaces;
using MyShop.Services;

namespace MyShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/[controller]")]
    public class CarInfoController : Controller
    {
        private readonly ICarInfoRepository? carInfo;
        private readonly IWebHostEnvironment hostEnvironment;
        public CarInfoController(ICarInfoRepository? carInfo, IWebHostEnvironment hostEnvironment)
        {
            this.carInfo = carInfo;
            this.hostEnvironment = hostEnvironment;
        }

        public IActionResult Edit(Guid id)
        {
            var entity = id == default ? new CarInfo() : carInfo?.GetCarInfoById(id);
            return View(entity);
        }

        [HttpPost]
        public IActionResult Edit(CarInfo model, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    model.ImagePath = imageFile.FileName;
                    using (var stream = new FileStream(Path.Combine(hostEnvironment.WebRootPath, "img/", imageFile.FileName), FileMode.Create))
                    {
                        imageFile.CopyTo(stream);
                    }
                }
                carInfo?.SaveCarInfo(model);
                return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
            }
            return View(model);
        }

        [HttpPost]
        [Route("/")]
        public IActionResult Delete(Guid id)
        {
            carInfo?.DeleteCarInfo(id);
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
        }

    }
}
