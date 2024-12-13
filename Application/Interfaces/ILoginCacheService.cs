using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ILoginCacheService
    {
        void AddOrUpdateUser(int userId, string token);

        bool IsUserLoggedIn(int userId);

        (DateTime LoginTime, DateTime ExpirationTime, string Token)? GetUserInfo(int userId);

        void RemoveUser(int userId);
    }
}
