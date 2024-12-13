using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IClubRepository
    {
        Task<IEnumerable<Club>> GetAllClubsAsync();
        Task<Club> GetClubByIdAsync(int clubId);
        Task AddClubAsync(Club club);
        Task UpdateClubAsync(Club club);
        Task DeleteClubAsync(int clubId);
        Task<IEnumerable<Club>> GetFilteredClubsAsync(string name, string address, string type, int? capacity, int? priceFrom, int? priceTo, DateTime reservationDate);
    }
}
