using Application.Interfaces;
using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VIS_KAL0326_PROJEKT.Models;

namespace VIS_KAL0326_PROJEKT.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILoginService _loginService;

        public HomeController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        private bool IsUserLoggedIn()
        {
            string token = Request.Cookies["UserToken"];

            return !string.IsNullOrEmpty(token) && _loginService.Authorize(token);
        }

        public IActionResult Index()
        {
            ViewBag.IsLoggedIn = IsUserLoggedIn();

            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (IsUserLoggedIn())
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int authToken = new Random().Next(1000, 9999);

            var result = _loginService.Authenticate(model.Username, model.Password, authToken);

            if (result.Status == 1)
            {
                Response.Cookies.Append("UserToken", result.Token, new CookieOptions
                {
                    HttpOnly = true,

                    Expires = DateTime.UtcNow.AddMinutes(30)
                });

                return RedirectToAction("Index");
            }

            ViewBag.ErrorMessage = "Špatnì zadané uživatelské jméno nebo heslo.";

            return View();
        }

        public IActionResult Logout()
        {
            string token = Request.Cookies["UserToken"];

            if (!string.IsNullOrEmpty(token))
            {
                _loginService.Logout(token);

                ViewBag.isLoggedIn = false;

                Response.Cookies.Delete("UserToken");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (IsUserLoggedIn())
            {
                return RedirectToAction("Index"); 
            }

            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var newUser = new User
            {
                Email = model.Email,
                Telephone = model.Telephone,
                Login = model.Username,
                Password = model.Password,
                RegistrationDate = DateTime.UtcNow
            };

            if (_loginService.Register(newUser))
            {
                return RedirectToAction("Login");
            }

            ViewBag.ErrorMessage = "Registrace selhala. Uživatelské jméno nebo email je již použito.";

            return View();
        }
    }
}
