using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Club
    {
        public int ClubId { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public int Capacity { get; set; }

        public int Price { get; set; }

        public string Image { get; set; }

        public string Services { get; set; }


        public IEnumerable<Reservation> Reservations { get; set; }
    }
}
