using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyShop.Domain;
using MyShop.Models;

namespace MyShop.Controllers
{
	[Authorize]
	public class AccountController : Controller
	{
        private readonly UserManager<IdentityUser>? userManager;
        private readonly SignInManager<IdentityUser>? signInManager;
        private readonly AppDbContext context;
        public AccountController(UserManager<IdentityUser>? userManager, SignInManager<IdentityUser>? signInManager, AppDbContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
        }

        [AllowAnonymous]
        public IActionResult Login()
		{
			ViewBag.ReturnUrl = "/";
			return View(new LoginModel());
		}

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser? user = await userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
						return Redirect("/");
                    }
                }
                ModelState.AddModelError(nameof(LoginModel.UserName), "Неверный логин или пароль");
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult Signup()
        {
            ViewBag.ReturnUrl = "/";
            return View(new SignupModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Signup(SignupModel model)
        {
            if (ModelState.IsValid)
            {
                    IdentityUser newUser = new IdentityUser { Email = model.Email, UserName = model.UserName, EmailConfirmed = true };
                    var result = await userManager.CreateAsync(newUser, model.Password);
                    if(result.Succeeded)
                    {
                        await signInManager.SignInAsync(newUser, isPersistent: false);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
            }
            return View(model);
        }
    }
}
