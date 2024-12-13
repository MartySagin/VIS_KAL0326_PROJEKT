using Application.Interfaces;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;

namespace Application.BusinessLogic
{
    public class ClubService : IClubService
    {
        IClubRepository _clubRepository;

        public ClubService(IClubRepository clubRepository)
        {
            _clubRepository = clubRepository;
        }

        public async Task<IEnumerable<Club>> GetFilteredClubsAsync(string? name, string? address, string? type, int? capacity, int? priceFrom, int? priceTo, DateTime reservationDate)
        {
            return await _clubRepository.GetFilteredClubsAsync(name, address, type, capacity, priceFrom, priceTo, reservationDate);
        }

        public async Task<Club> GetClubByIdAsync(int clubId)
        {
            return await _clubRepository.GetClubByIdAsync(clubId);
        }
    }
}
