using System.ComponentModel.DataAnnotations;

namespace VIS_KAL0326_PROJEKT.Models
{
    public class PayReservationViewModel
    {
        [Required(ErrorMessage = "ID rezervace je povinné.")]
        public int ReservationId { get; set; }

        [Display(Name = "Název klubu")]
        public string? ClubName { get; set; }

        [Display(Name = "Datum rezervace")]
        [DataType(DataType.Date, ErrorMessage = "Neplatný formát data.")]
        public DateTime ReservationDate { get; set; }

        [Display(Name = "Počet osob")]
        public int NumberOfPeople { get; set; }

        [Display(Name = "Cena")]
        [Range(0, int.MaxValue, ErrorMessage = "Cena musí být nezáporná.")]
        public int Price { get; set; }

        [Required(ErrorMessage = "Způsob platby je povinný.")]
        [Display(Name = "Způsob platby")]
        public string? PaymentMethod { get; set; }
    }
}
