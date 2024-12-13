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
        public IActionResult Login(string username, string password)
        {
            int authToken = new Random().Next(1000, 9999);

            var result = _loginService.Authenticate(username, password, authToken);

            if (result.Status == 1)
            {
                Response.Cookies.Append("UserToken", result.Token, new CookieOptions
                {
                    HttpOnly = true,

                    Expires = DateTime.UtcNow.AddMinutes(30)
                });

                return RedirectToAction("Index");
            }

            ViewBag.ErrorMessage = "Invalid username or password.";

            return View();
        }

        public IActionResult Logout()
        {
            string token = Request.Cookies["UserToken"];

            if (!string.IsNullOrEmpty(token))
            {
                _loginService.Logout(token);

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
        public IActionResult Register(string email, string telephone, string username, string password)
        {
            var newUser = new User
            {
                Email = email,
                Telephone = telephone,
                Login = username,
                Password = password,
                RegistrationDate = DateTime.UtcNow
            };

            if (_loginService.Register(newUser))
            {
                ViewBag.Message = "Registration successful. You can now log in.";

                return RedirectToAction("Login");
            }

            ViewBag.ErrorMessage = "Registration failed. Username or email may already exist.";

            return View();
        }
    }
}
