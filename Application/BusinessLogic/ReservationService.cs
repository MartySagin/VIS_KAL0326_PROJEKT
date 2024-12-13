using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using DataAccess.Interfaces;
using DataAccess.Models;

namespace Application.BusinessLogic
{
    public class ReservationService : IReservationService
    {
        IReservationRepository _reservationRepository;

        public ReservationService(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task AddReservationAsync(Reservation reservation)
        {
            await _reservationRepository.AddReservationAsync(reservation);
        }
    }
}
