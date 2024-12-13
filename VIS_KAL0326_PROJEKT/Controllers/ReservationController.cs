using Application.Interfaces;
using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace VIS_KAL0326_PROJEKT.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IClubRepository _clubRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly ILoginService _loginService;

        public ReservationController(IClubRepository clubRepository, IReservationRepository reservationRepository, ILoginService loginService)
        {
            _clubRepository = clubRepository;

            _reservationRepository = reservationRepository;

            _loginService = loginService;
        }

        [HttpPost]
        public async Task<IActionResult> ReviewReservation(int ClubId, int UserId, DateTime ReservationDate)
        {
            var token = Request.Cookies["UserToken"];

            if (!_loginService.Authorize(token))
            {
                ViewBag.IsLoggedIn = false;

                return RedirectToAction("Login", "Home");
            }

            ViewBag.IsLoggedIn = true;

            var club = await _clubRepository.GetClubByIdAsync(ClubId);

            if (club == null)
            {
                return NotFound("Club not found.");
            }

            ViewBag.ReservationDate = ReservationDate;

            ViewBag.UserId = UserId;

            return View(club); 
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation(int ClubId, int UserId, DateTime ReservationDate, int NumberOfPeople, int Price)
        {
          
            var reservation = new Reservation
            {
                ClubId = ClubId,
                UserId = UserId,
                ReservationDate = ReservationDate,
                NumberOfPeople = NumberOfPeople,
                Price = Price, 
                IsConfirmed = false, 
                State = "Pending" 
            };

            await _reservationRepository.AddReservationAsync(reservation);

            TempData["SuccessMessage"] = "Rezervace byla úspěšně vytvořena!";

            return RedirectToAction("Search", "Club");
        }

    }
}
