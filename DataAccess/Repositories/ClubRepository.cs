using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DataAccess.Interfaces;
using DataAccess.Models;

namespace DataAccess.Repositories
{
    public class ClubRepository : IClubRepository
    {
        private readonly IDatabaseAccess _databaseAccess;

        public ClubRepository(IDatabaseAccess databaseAccess)
        {
            _databaseAccess = databaseAccess;
        }

        public async Task<IEnumerable<Club>> GetAllClubsAsync()
        {
            var sql = "SELECT * FROM Clubs";

            var clubs = await _databaseAccess.ExecuteQueryAsync<Club>(sql);

            var reservationSql = "SELECT * FROM Reservations";

            var reservations = await _databaseAccess.ExecuteQueryAsync<Reservation>(reservationSql);

            foreach (var club in clubs)
            {
                club.Reservations = reservations.Where(r => r.ClubId == club.ClubId).ToList();
            }

            return clubs;
        }

        public async Task<Club> GetClubByIdAsync(int clubId)
        {
            var sql = "SELECT * FROM Clubs WHERE ClubId = @ClubId";

            var clubs = await _databaseAccess.ExecuteQueryAsync<Club>(sql, new { ClubId = clubId });

            return clubs.FirstOrDefault();
        }

        public async Task AddClubAsync(Club club)
        {
            var sql = @"
                INSERT INTO Clubs (Name, Address, Description, Type, Capacity, Price, Image, Services)
                VALUES (@Name, @Address, @Description, @Type, @Capacity, @Price, @Image, @Services)";

            await _databaseAccess.ExecuteNonQueryAsync(sql, club);
        }

        public async Task UpdateClubAsync(Club club)
        {
            var sql = @"
                UPDATE Clubs
                SET Name = @Name, Address = @Address, Description = @Description, Type = @Type,
                    Capacity = @Capacity, Price = @Price, Image = @Image, Services = @Services
                WHERE ClubId = @ClubId";

            await _databaseAccess.ExecuteNonQueryAsync(sql, club);
        }

        public async Task DeleteClubAsync(int clubId)
        {
            var sql = "DELETE FROM Clubs WHERE ClubId = @ClubId";

            await _databaseAccess.ExecuteNonQueryAsync(sql, new { ClubId = clubId });
        }

        public async Task<IEnumerable<Club>> GetFilteredClubsAsync(string? name, string? address, string? type, int? capacity, int? priceFrom, int? priceTo, DateTime reservationDate)
        {
            var sql = @"
                SELECT DISTINCT c.*
                FROM Clubs c
                LEFT JOIN Reservations r ON c.ClubId = r.ClubId
                    AND DATE(r.ReservationDate) = DATE(@ReservationDate)
                WHERE
                    (@Name IS NULL OR c.Name LIKE '%' || @Name || '%') AND
                    (@Address IS NULL OR c.Address LIKE '%' || @Address || '%') AND
                    (@Type IS NULL OR c.Type = @Type) AND
                    (@Capacity IS NULL OR c.Capacity >= @Capacity) AND
                    (@PriceFrom IS NULL OR c.Price >= @PriceFrom) AND
                    (@PriceTo IS NULL OR c.Price <= @PriceTo) AND
                    (r.ReservationId IS NULL OR DATE(r.ReservationDate) != DATE(@ReservationDate));";

            return await _databaseAccess.ExecuteQueryAsync<Club>(sql, new
            {
                Name = name,
                Address = address,
                Type = type,
                Capacity = capacity,
                PriceFrom = priceFrom,
                PriceTo = priceTo,
                ReservationDate = reservationDate.ToString("yyyy-MM-dd")
            });
        }
    }
}
