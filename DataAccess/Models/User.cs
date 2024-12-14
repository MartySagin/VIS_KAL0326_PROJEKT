namespace DataAccess.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string Email { get; set; }

        public string Telephone { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public DateTime RegistrationDate { get; set; }

        public IEnumerable<Reservation> Reservations { get; set; }
    }
}
