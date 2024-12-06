using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using DataAccess.Models;


namespace DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDatabaseAccess _databaseAccess;

        public UserRepository(IDatabaseAccess databaseAccess)
        {
            _databaseAccess = databaseAccess;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var sql = "SELECT * FROM Users";

            return await _databaseAccess.ExecuteQueryAsync<User>(sql);
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            var sql = "SELECT * FROM Users WHERE UserId = @UserId";

            var users = await _databaseAccess.ExecuteQueryAsync<User>(sql, new { UserId = userId });

            return users.FirstOrDefault();
        }

        public async Task AddUserAsync(User user)
        {
            var sql = @"
                INSERT INTO Users (Email, Telephone, Login, Password, RegistrationDate)
                VALUES (@Email, @Telephone, @Login, @Password, @RegistrationDate)";

            await _databaseAccess.ExecuteNonQueryAsync(sql, user);
        }

        public async Task UpdateUserAsync(User user)
        {
            var sql = @"
                UPDATE Users
                SET Email = @Email, Telephone = @Telephone, Login = @Login, 
                    Password = @Password, RegistrationDate = @RegistrationDate
                WHERE UserId = @UserId";

            await _databaseAccess.ExecuteNonQueryAsync(sql, user);
        }

        public async Task DeleteUserAsync(int userId)
        {
            var sql = "DELETE FROM Users WHERE UserId = @UserId";

            await _databaseAccess.ExecuteNonQueryAsync(sql, new { UserId = userId });
        }
    }
}
