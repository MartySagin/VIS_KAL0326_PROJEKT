using Application.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BusinessLogic
{
    public class LoginCacheService : ILoginCacheService
    {
        private readonly ConcurrentDictionary<int, (DateTime LoginTime, DateTime ExpirationTime, string Token)> _userCache;

        private readonly TimeSpan _tokenLifetime;

        public LoginCacheService(TimeSpan tokenLifetime)
        {
            _userCache = new ConcurrentDictionary<int, (DateTime, DateTime, string)>();

            _tokenLifetime = tokenLifetime;
        }

        public void AddOrUpdateUser(int userId, string token)
        {
            var loginTime = DateTime.UtcNow;

            var expirationTime = loginTime.Add(_tokenLifetime);

            _userCache.AddOrUpdate(userId,
                (loginTime, expirationTime, token),
                (key, existing) => (loginTime, expirationTime, token));
        }

        public bool IsUserLoggedIn(int userId)
        {
            if (_userCache.TryGetValue(userId, out var userInfo))
            {
                if (DateTime.UtcNow <= userInfo.ExpirationTime)
                {
                    return true;
                }

                RemoveUser(userId);
            }

            return false;
        }

        public (DateTime LoginTime, DateTime ExpirationTime, string Token)? GetUserInfo(int userId)
        {
            if (_userCache.TryGetValue(userId, out var userInfo))
            {
                return userInfo;
            }

            return null;
        }

        public void RemoveUser(int userId)
        {
            _userCache.TryRemove(userId, out _);
        }
    }
}