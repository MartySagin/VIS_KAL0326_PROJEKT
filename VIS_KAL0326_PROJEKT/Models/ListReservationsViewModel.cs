namespace VIS_KAL0326_PROJEKT.Models
{
    public class ListReservationsViewModel
    {
        public List<ReservationItemViewModel> Reservations { get; set; } = new List<ReservationItemViewModel>();
    }

    public class ReservationItemViewModel
    {
        public int ReservationId { get; set; }
        public string ClubName { get; set; }
        public DateTime ReservationDate { get; set; }
        public int NumberOfPeople { get; set; }
        public string State { get; set; }
        public int Price { get; set; }
    }
}
