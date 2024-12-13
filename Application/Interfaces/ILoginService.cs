using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;

namespace Application.Interfaces
{
    public interface ILoginService
    {
        (int Status, int UserId, string Token) Authenticate(string login, string password, int authToken);
        bool Authorize(string token);
        void Logout(string token);
        bool Register(User user);
        bool ValidatePassword(string password);

        int? ExtractUserIdFromToken(string token);
    }
}
