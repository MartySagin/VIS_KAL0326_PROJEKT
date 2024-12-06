using DataAccess.Interfaces;
using DataAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly IDatabaseAccess _databaseAccess;

        public ReservationRepository(IDatabaseAccess databaseAccess)
        {
            _databaseAccess = databaseAccess;
        }

        public async Task<IEnumerable<Reservation>> GetAllReservationsAsync()
        {
            var sql = @"
                SELECT r.ReservationId, r.ReservationDate, r.NumberOfPeople, r.IsConfirmed, r.State, 
                       r.Price, r.ClubId, r.UserId,
                       c.Name AS ClubName, u.Email AS UserEmail
                FROM Reservations r
                JOIN Clubs c ON r.ClubId = c.ClubId
                JOIN Users u ON r.UserId = u.UserId";

            return await _databaseAccess.ExecuteQueryAsync<Reservation>(sql);
        }

        public async Task<Reservation> GetReservationByIdAsync(int reservationId)
        {
            var sql = @"
                SELECT r.ReservationId, r.ReservationDate, r.NumberOfPeople, r.IsConfirmed, r.State, 
                       r.Price, r.ClubId, r.UserId,
                       c.Name AS ClubName, u.Email AS UserEmail
                FROM Reservations r
                JOIN Clubs c ON r.ClubId = c.ClubId
                JOIN Users u ON r.UserId = u.UserId
                WHERE r.ReservationId = @ReservationId";

            var reservations = await _databaseAccess.ExecuteQueryAsync<Reservation>(sql, new { ReservationId = reservationId });

            return reservations.FirstOrDefault();
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByUserIdAsync(int userId)
        {
            var sql = @"
                SELECT r.ReservationId, r.ReservationDate, r.NumberOfPeople, r.IsConfirmed, r.State, 
                       r.Price, r.ClubId, r.UserId,
                       c.Name AS ClubName, u.Email AS UserEmail
                FROM Reservations r
                JOIN Clubs c ON r.ClubId = c.ClubId
                JOIN Users u ON r.UserId = u.UserId
                WHERE r.UserId = @UserId";

            return await _databaseAccess.ExecuteQueryAsync<Reservation>(sql, new { UserId = userId });
        }

        public async Task AddReservationAsync(Reservation reservation)
        {
            var sql = @"
                INSERT INTO Reservations (ReservationDate, NumberOfPeople, IsConfirmed, State, Price, ClubId, UserId)
                VALUES (@ReservationDate, @NumberOfPeople, @IsConfirmed, @State, @Price, @ClubId, @UserId)";

            await _databaseAccess.ExecuteNonQueryAsync(sql, reservation);
        }

        public async Task UpdateReservationAsync(Reservation reservation)
        {
            var sql = @"
                UPDATE Reservations
                SET ReservationDate = @ReservationDate, NumberOfPeople = @NumberOfPeople, 
                    IsConfirmed = @IsConfirmed, State = @State, Price = @Price, 
                    ClubId = @ClubId, UserId = @UserId
                WHERE ReservationId = @ReservationId";

            await _databaseAccess.ExecuteNonQueryAsync(sql, reservation);
        }

        public async Task DeleteReservationAsync(int reservationId)
        {
            var sql = "DELETE FROM Reservations WHERE ReservationId = @ReservationId";

            await _databaseAccess.ExecuteNonQueryAsync(sql, new { ReservationId = reservationId });
        }
    }
}
