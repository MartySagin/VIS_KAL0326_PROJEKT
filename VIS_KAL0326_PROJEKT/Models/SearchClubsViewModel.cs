using System.ComponentModel.DataAnnotations;

namespace VIS_KAL0326_PROJEKT.Models
{
    public class SearchClubsViewModel
    {
        [StringLength(100, ErrorMessage = "Název klubu nesmí být delší než 100 znaků.")]
        [Display(Name = "Název klubu")]
        public string? Name { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Adresa nesmí být delší než 100 znaků.")]
        [Display(Name = "Adresa")]
        public string? Address { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "Typ klubu nesmí být delší než 50 znaků.")]
        [Display(Name = "Typ klubu")]
        public string? Type { get; set; } = string.Empty;

        [Range(1, 1000, ErrorMessage = "Kapacita musí být mezi 1 a 1000.")]
        [Display(Name = "Kapacita")]
        public int? Capacity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Cena od musí být nezáporná.")]
        [Display(Name = "Cena od")]
        public int? PriceFrom { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Cena do musí být nezáporná.")]
        [Display(Name = "Cena do")]
        public int? PriceTo { get; set; }

        [Required(ErrorMessage = "Datum rezervace je povinné.")]
        [DataType(DataType.Date, ErrorMessage = "Neplatný formát data.")]
        [Display(Name = "Datum rezervace")]
        public DateTime ReservationDate { get; set; }
    }
}
