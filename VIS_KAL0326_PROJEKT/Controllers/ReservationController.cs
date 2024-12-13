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
            
            var token = Request.Cookies["UserToken"];

            if (!_loginService.Authorize(token))
            {
                ViewBag.IsLoggedIn = false;

                return RedirectToAction("Login", "Home");
            }

            ViewBag.IsLoggedIn = true;

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

            return RedirectToAction("SearchClubs", "Club");
        }

        [HttpGet]

        public async Task<IActionResult> ListReservations()
        {
            var token = Request.Cookies["UserToken"];

            if (!_loginService.Authorize(token))
            {
                ViewBag.IsLoggedIn = false;

                return RedirectToAction("Login", "Home");
            }

            ViewBag.IsLoggedIn = true;

            var userId = _loginService.ExtractUserIdFromToken(token);

            var reservations = await _reservationRepository.GetReservationsByUserIdAsync(userId ?? -1);

            return View(reservations);
        }

        [HttpPost]
        public async Task<IActionResult> PayReservation(int ReservationId)
        {
            var token = Request.Cookies["UserToken"];

            if (!_loginService.Authorize(token))
            {
                ViewBag.IsLoggedIn = false;

                return RedirectToAction("Login", "Home");
            }

            ViewBag.IsLoggedIn = true;

            var reservation = await _reservationRepository.GetReservationByIdAsync(ReservationId);

            if (reservation == null)
            {
                return NotFound("Reservation not found.");
            }

            if (reservation.State != "Confirmed")
            {
                return RedirectToAction("ListReservations");
            }

            return View("PayReservation", reservation);
        }

        [HttpPost]

        public async Task<IActionResult> ConfirmPayment(int ReservationId)
        {
            var token = Request.Cookies["UserToken"];

            if (!_loginService.Authorize(token))
            {
                ViewBag.IsLoggedIn = false;

                return RedirectToAction("Login", "Home");
            }

            ViewBag.IsLoggedIn = true;

            var reservation = await _reservationRepository.GetReservationByIdAsync(ReservationId);  

            reservation.State = "Paid";

            _reservationRepository.UpdateReservationAsync(reservation);

            return RedirectToAction("ListReservations");
        }

        [HttpPost]

        public async Task<IActionResult> DeleteReservation(int ReservationId    )
        {
            var token = Request.Cookies["UserToken"];

            if (!_loginService.Authorize(token))
            {
                ViewBag.IsLoggedIn = false;

                return RedirectToAction("Login", "Home");
            }

            ViewBag.IsLoggedIn = true;

            await _reservationRepository.DeleteReservationAsync(ReservationId);

            return RedirectToAction("ListReservations");
        }

    }
}
