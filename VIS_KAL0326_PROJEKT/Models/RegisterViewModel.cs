using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace VIS_KAL0326_PROJEKT.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email je povinný.")]
        [EmailAddress(ErrorMessage = "Zadejte platnou emailovou adresu.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefon je povinný.")]
        [RegularExpression(@"^\+?\d{9}$", ErrorMessage = "Telefonní číslo musí obsahovat 9 číslic.")]
        public string Telephone { get; set; }

        [Required(ErrorMessage = "Uživatelské jméno je povinné.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Uživatelské jméno musí mít alespoň 3 znaky.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Heslo je povinné.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Heslo musí mít alespoň 6 znaků.")]
        public string Password { get; set; }
    }
}
