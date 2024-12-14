using Application.Interfaces;
using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Reflection.Emit;
using VIS_KAL0326_PROJEKT.Models;

namespace VIS_KAL0326_PROJEKT.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubService _clubService;

        private readonly ILoginService _loginService;

        public ClubController(IClubService clubService, ILoginService loginService)
        {
            _clubService = clubService;

            _loginService = loginService;
        }

        [HttpGet]
        public IActionResult SearchClubs()
        {
            ViewBag.IsLoggedIn = _loginService.Authorize(Request.Cookies["UserToken"]);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SearchClubs(SearchClubsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            IEnumerable<Club> clubs;

            try { 
                clubs = await _clubService.GetFilteredClubsAsync(model.Name, model.Address, model.Type, model.Capacity, model.PriceFrom, model.PriceTo, model.ReservationDate);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;

                return View(model);
            }

            ViewBag.Clubs = clubs;

            var token = Request.Cookies["UserToken"];

            ViewBag.IsLoggedIn = _loginService.Authorize(token);

            ViewBag.UserId = !string.IsNullOrEmpty(token) ? _loginService.ExtractUserIdFromToken(token) : null;

            return View(model);
        }
    }
}
