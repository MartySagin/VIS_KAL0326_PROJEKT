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
            if (priceFrom != null && priceTo != null && priceFrom > priceTo)
            {
                throw new ArgumentException("Cena od nesmí být vyšší než cena do.");
            }

            if (capacity != null && capacity <= 0)
            {
                throw new ArgumentException("Kapacita musí být větší než 0.");
            }

            if (priceFrom != null && priceFrom < 0)
            {
                throw new ArgumentException("Cena od musí být větší nebo rovna 0.");
            }

            if (priceTo != null && priceTo < 0)
            {
                throw new ArgumentException("Cena do musí být větší nebo rovna 0.");
            }

            if (reservationDate < DateTime.UtcNow.Date)
            {
                throw new ArgumentException("Datum rezervace nesmí být v minulosti.");
            }

            return await _clubRepository.GetFilteredClubsAsync(name, address, type, capacity, priceFrom, priceTo, reservationDate.ToString("yyyy-MM-dd"));
        }

        public async Task<Club> GetClubByIdAsync(int clubId)
        {
            return await _clubRepository.GetClubByIdAsync(clubId);
        }
    }
}
