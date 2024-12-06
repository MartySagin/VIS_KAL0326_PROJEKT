using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }

        public int ClubId { get; set; }

        public int UserId { get; set; }

        public DateTime ReservationDate { get; set; }

        public int NumberOfPeople { get; set; }

        public bool IsConfirmed { get; set; }

        public string State { get; set; }

        public int Price { get; set; }

        
        public Club Club { get; set; }

        public User User { get; set; }
        
    }
}
