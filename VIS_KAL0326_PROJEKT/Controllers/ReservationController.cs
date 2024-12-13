using Application.Interfaces;
using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using VIS_KAL0326_PROJEKT.Models;

namespace VIS_KAL0326_PROJEKT.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IClubService _clubService;
        private readonly IReservationService _reservationService;
        private readonly ILoginService _loginService;

        public ReservationController(IClubService clubService, IReservationService reservationService, ILoginService loginService)
        {
            _reservationService = reservationService;

            _clubService = clubService;

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

            var club = await _clubService.GetClubByIdAsync(ClubId);

            if (club == null)
            {
                return NotFound("Club not found.");
            }

            var viewModel = new ReviewReservationViewModel
            {
                ClubId = club.ClubId,
                UserId = UserId,
                ReservationDate = ReservationDate,
                ClubName = club.Name,
                ClubAddress = club.Address,
                ClubType = club.Type,
                ClubCapacity = club.Capacity,
                ClubPricePerPerson = club.Price,
                Price = club.Price
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation(ReviewReservationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("ReviewReservation", model);
            }

            var token = Request.Cookies["UserToken"];

            if (!_loginService.Authorize(token))
            {
                ViewBag.IsLoggedIn = false;

                return RedirectToAction("Login", "Home");
            }

            ViewBag.IsLoggedIn = true;

            var reservation = new Reservation
            {
                ClubId = model.ClubId,
                UserId = model.UserId,
                ReservationDate = model.ReservationDate,
                NumberOfPeople = model.NumberOfPeople,
                Price = model.Price,
                IsConfirmed = false,
                State = "Pending"
            };

            await _reservationService.AddReservationAsync(reservation);

            return Redirect("ListReservations");
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

            var viewModel = new ListReservationsViewModel
            {
                Reservations = reservations.Select(r => new ReservationItemViewModel
                {
                    ReservationId = r.ReservationId,
                    ClubName = r.Club.Name,
                    ReservationDate = r.ReservationDate,
                    NumberOfPeople = r.NumberOfPeople,
                    State = r.State,
                    Price = r.Price
                }).ToList()
            };

            return View(viewModel);
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

            var viewModel = new PayReservationViewModel
            {
                ReservationId = reservation.ReservationId,
                ClubName = reservation.Club.Name,
                ReservationDate = reservation.ReservationDate,
                NumberOfPeople = reservation.NumberOfPeople,
                Price = reservation.Price
            };

            return View("PayReservation", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmPayment(PayReservationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("PayReservation", model);
            }

            var token = Request.Cookies["UserToken"];

            if (!_loginService.Authorize(token))
            {
                return RedirectToAction("Login", "Home");
            }

            var reservation = await _reservationRepository.GetReservationByIdAsync(model.ReservationId);

            if (reservation == null)
            {
                return NotFound("Reservation not found.");
            }

            reservation.State = "Paid";

            await _reservationRepository.UpdateReservationAsync(reservation);

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
