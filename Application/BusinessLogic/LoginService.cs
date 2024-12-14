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
    public class LoginService : ILoginService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILoginCacheService _loginCacheService;
        private readonly IMyLogger _myLogger;

        public LoginService(IUserRepository userRepository, ILoginCacheService loginCacheService, IMyLogger customLogger)
        {
            _userRepository = userRepository;

            _loginCacheService = loginCacheService;

            _myLogger = customLogger;
        }

        public (int Status, int UserId, string Token) Authenticate(string login, string password, int authToken)
        {
            var user = _userRepository.GetUserByLoginAsync(login).Result;

            if (user == null)
            {
                _myLogger.Warning($"Authentication failed. User '{login}' not found.");

                return (-1, -1, "");
            }

            if (user.Password != password)
            {
                _myLogger.Warning($"Authentication failed. Incorrect password for user '{login}'.");

                return (-1, -1, "");
            }

            var token = GenerateToken(user.UserId, authToken);

            _loginCacheService.AddOrUpdateUser(user.UserId, token);

            _myLogger.Information($"User '{login}' authenticated successfully with token '{token}'.");

            return (1, user.UserId, token);
        }

        public bool Authorize(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                _myLogger.Warning("Authorization failed. Token is null or empty.");

                return false;
            }

            var userId = ExtractUserIdFromToken(token);


            if (!_loginCacheService.IsUserLoggedIn(userId ?? -1))
            {
                _myLogger.Warning($"Authorization failed. User with ID '{userId}' is not logged in.");

                return false;
            }

            var cachedToken = _loginCacheService.GetUserInfo(userId ?? -1)?.Item3;

            if (cachedToken == token)
            {
                _myLogger.Information($"Token '{token}' authorized successfully.");

                return true;
            }

            _myLogger.Warning($"Authorization failed. Token '{token}' does not match the cached token.");

            return false;
        }

        public void Logout(string token)
        {
            var userId = ExtractUserIdFromToken(token);

            _loginCacheService.RemoveUser(userId ?? -1);

            _myLogger.Information($"User with token '{token}' logged out successfully.");
        }

        public bool Register(User user)
        {
            if (_userRepository.GetUserByLoginAsync(user.Login).Result != null)
            {
                _myLogger.Warning($"Registration failed. User '{user.Login}' already exists.");

                return false;
            }

            if (_userRepository.GetUserByEmailAsync(user.Email).Result != null)
            {
                _myLogger.Warning($"Registration failed. Email '{user.Email}' is already registered.");

                return false;
            }

            if (!ValidatePassword(user.Password))
            {
                _myLogger.Warning($"Registration failed. Password for user '{user.Login}' does not meet the criteria.");

                return false;
            }

            if (_userRepository.AddUserAsync(user).Result)
            {
                _myLogger.Information($"User '{user.Login}' registered successfully.");

                return true;
            }

            _myLogger.Error($"Registration failed for user '{user.Login}'.");

            return false;
        }

        public bool ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 6)
            {
                return false;
            }

            return true;
        }

        private string GenerateToken(int userId, int authToken)
        {
            return $"{userId}t{authToken}";
        }

        public int? ExtractUserIdFromToken(string token)
        {
            return int.Parse(token.Split('t')[0]);
        }
    }
}
