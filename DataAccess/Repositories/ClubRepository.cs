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

            return await _databaseAccess.ExecuteQueryAsync<Club>(sql);
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
    }
}
