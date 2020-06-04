using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ap1.Models
{
    public class ResetPasswordModel
    {
        [Display(Name = "Hasło")]
        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "pole wymagane")]
        [MinLength(6, ErrorMessage = "Hasło ma mieć minimum 6 znaków")]
        public string NewPassword { get; set; }

        [Display(Name = "Powtórz Hasło")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Hasła się nie pokrywają!")]
        public string ConfirmPassword { get; set; }


        [Required]
        public string ResetCode { get; set; }

    }
}