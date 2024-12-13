using Application.Interfaces;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Emit;
using VIS_KAL0326_PROJEKT.Models;

namespace VIS_KAL0326_PROJEKT.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubRepository _clubRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly ILoginService _loginService;

        public ClubController(IClubRepository clubRepository, IReservationRepository reservationRepository, ILoginService loginService)
        {
            _clubRepository = clubRepository;
            _reservationRepository = reservationRepository;
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

            var clubs = await _clubRepository.GetFilteredClubsAsync(model.Name, model.Address, model.Type, model.Capacity, model.PriceFrom, model.PriceTo, model.ReservationDate);

            ViewBag.Clubs = clubs;

            var token = Request.Cookies["UserToken"];

            ViewBag.IsLoggedIn = _loginService.Authorize(token);

            ViewBag.UserId = !string.IsNullOrEmpty(token) ? _loginService.ExtractUserIdFromToken(token) : null;

            return View();
        }
    }
}
