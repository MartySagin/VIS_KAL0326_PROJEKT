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

        private readonly IMyLogger _myLogger;

        public UserRepository(IDatabaseAccess databaseAccess, IMyLogger myLogger)
        {
            _databaseAccess = databaseAccess;

            _myLogger = myLogger;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var sql = "SELECT * FROM Users";

            _myLogger.Information("Executing GetAllUsersAsync method");

            return await _databaseAccess.ExecuteQueryAsync<User>(sql);
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            var sql = "SELECT * FROM Users WHERE UserId = @UserId";

            var users = await _databaseAccess.ExecuteQueryAsync<User>(sql, new { UserId = userId });

            _myLogger.Information("Executing GetUserByIdAsync method");

            return users.FirstOrDefault();
        }

        public async Task<bool> AddUserAsync(User user)
        {
            var sql = @"
                INSERT INTO Users (Email, Telephone, Login, Password, RegistrationDate)
                VALUES (@Email, @Telephone, @Login, @Password, @RegistrationDate)";

            await _databaseAccess.ExecuteNonQueryAsync(sql, user);

            _myLogger.Information("Executing AddUserAsync method");

            return true;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            var sql = @"
                UPDATE Users
                SET Email = @Email, Telephone = @Telephone, Login = @Login, 
                    Password = @Password, RegistrationDate = @RegistrationDate
                WHERE UserId = @UserId";

            await _databaseAccess.ExecuteNonQueryAsync(sql, user);

            _myLogger.Information("Executing UpdateUserAsync method");

            return true;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var sql = "DELETE FROM Users WHERE UserId = @UserId";

            await _databaseAccess.ExecuteNonQueryAsync(sql, new { UserId = userId });

            _myLogger.Information("Executing DeleteUserAsync method");

            return true;
        }

        public async Task<User?> GetUserByLoginAsync(string login)
        {
            var sql = "SELECT * FROM Users WHERE Login = @Login";

            var users = await _databaseAccess.ExecuteQueryAsync<User>(sql, new { Login = login });

            _myLogger.Information("Executing GetUserByLoginAsync method");

            return users.FirstOrDefault();
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var sql = "SELECT * FROM Users WHERE Email = @Email";

            var users = await _databaseAccess.ExecuteQueryAsync<User>(sql, new { Email = email });

            _myLogger.Information("Executing GetUserByEmailAsync method");

            return users.FirstOrDefault();
        }
    }
}
