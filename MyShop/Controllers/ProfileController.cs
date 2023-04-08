using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MyShop.Domain;
using MyShop.Domain.Entities;
using MyShop.Models;
using System.Security.Claims;

namespace MyShop.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProfileController(AppDbContext context, IWebHostEnvironment hostEnvironment)
        {
            this.context = context;
            webHostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var profile = context.Profiles.SingleOrDefault(c => c.Email == email);
            if (profile == null)
                return RedirectToAction("Edit", "Profile", HttpMethod.Get);
            else
                return View(profile);
        }

        public IActionResult Edit()
        {
            var profile = context.Profiles.FirstOrDefault(c => c.Email == User.FindFirstValue(ClaimTypes.Email)) ?? new Profile { Email = User.FindFirstValue(ClaimTypes.Email) };
            return View(profile);
        }

        [HttpPost]
        public IActionResult Edit(Profile model, IFormFile formFile)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var profile = context.Profiles.SingleOrDefault(c => c.Email == email);

            if (ModelState.IsValid)
            {

                if(formFile != null)
                {
                model.ImagePath = formFile.FileName;
                    using (var stream = new FileStream(Path.Combine(webHostEnvironment.WebRootPath, "img/", formFile.FileName), FileMode.Create))
                    {
                        formFile.CopyTo(stream);
                    }
                }
                if (profile == null)
                    context.Entry(model).State = EntityState.Added;
                else
                {
                    profile.Sex = model.Sex;
                    profile.BirthDate = model.BirthDate;
                    profile.UserName = model.UserName;
                    profile.ImagePath = model.ImagePath;
                }
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
