using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IClubService
    {
        Task<Club> GetClubByIdAsync(int clubId);
        Task<IEnumerable<Club>> GetFilteredClubsAsync(string? name, string? address, string? type, int? capacity, int? priceFrom, int? priceTo, DateTime reservationDate);
    }
}
