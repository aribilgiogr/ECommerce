using Core.Abstracts.IServices;
using Core.Concretes.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace UI.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IAuthService service;

        public AccountsController(IAuthService service)
        {
            this.service = service;
        }

        // Profil ekranı:
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(string returnUrl = "/")
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto model, string returnUrl = "/")
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await service.LoginAsync(model);
            if (result.IsSuccess)
            {
                return Redirect(returnUrl);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await service.RegisterAsync(model);
            if (result.IsSuccess) {
                return RedirectToAction("login");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await service.LogoutAsync();
            return RedirectToAction("index", "home");
        }
    }
}
