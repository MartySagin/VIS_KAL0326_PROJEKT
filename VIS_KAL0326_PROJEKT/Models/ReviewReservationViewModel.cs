using System.ComponentModel.DataAnnotations;

namespace VIS_KAL0326_PROJEKT.Models
{
    public class ReviewReservationViewModel
    {
        [Required(ErrorMessage = "ClubId je povinný.")]
        public int ClubId { get; set; }

        [Required(ErrorMessage = "UserId je povinný.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Datum rezervace je povinné.")]
        [DataType(DataType.Date, ErrorMessage = "Neplatný formát data.")]
        public DateTime ReservationDate { get; set; }

        [Required(ErrorMessage = "Počet osob je povinný.")]
        [Range(1, int.MaxValue, ErrorMessage = "Počet osob musí být alespoň 1.")]
        public int NumberOfPeople { get; set; }

        [Required(ErrorMessage = "Cena je povinná.")]
        [Range(0, int.MaxValue, ErrorMessage = "Cena musí být nezáporná.")]
        public int Price { get; set; }

        [Display(Name = "Název klubu")]
        public string? ClubName { get; set; }

        [Display(Name = "Adresa klubu")]
        public string? ClubAddress { get; set; }

        [Display(Name = "Typ klubu")]
        public string? ClubType { get; set; }

        [Display(Name = "Kapacita klubu")]
        public int? ClubCapacity { get; set; }

        [Display(Name = "Cena za osobu")]
        public int? ClubPricePerPerson { get; set; }
    }
}
