using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IReservationService
    {
        Task AddReservationAsync(Reservation reservation);

        Task DeleteReservationAsync(int reservationId);

        Task<Reservation> GetReservationByIdAsync(int reservationId);

        Task<IEnumerable<Reservation>> GetReservationsByUserIdAsync(int userId);

        Task UpdateReservationAsync(Reservation reservation);
    }
}
