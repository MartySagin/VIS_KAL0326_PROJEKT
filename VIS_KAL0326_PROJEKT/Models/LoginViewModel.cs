using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace VIS_KAL0326_PROJEKT.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Uživatelské jméno je povinné.")]
        [StringLength(100, ErrorMessage = "Uživatelské jméno nesmí přesáhnout 100 znaků.")]
        [Display(Name = "Uživatelské jméno")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Heslo je povinné.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Heslo musí mít alespoň 6 znaků.")]
        [Display(Name = "Heslo")]

        public string Password { get; set; }
    }
}
