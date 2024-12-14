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

        public async Task DeleteReservationAsync(int reservationId)
        {
            await _reservationRepository.DeleteReservationAsync(reservationId);
        }

        public async Task<Reservation> GetReservationByIdAsync(int reservationId)
        {
            return await _reservationRepository.GetReservationByIdAsync(reservationId);
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByUserIdAsync(int userId)
        {
            return await _reservationRepository.GetReservationsByUserIdAsync(userId);
        }

        public async Task UpdateReservationAsync(Reservation reservation)
        {
            await _reservationRepository.UpdateReservationAsync(reservation);
        }
    }
}
