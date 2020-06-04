using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ap1.Models
{
    public class UserLogin
    {
        [Display(Name = "Adres Email:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Adres Email jest wymagany " )]
        public string EmailID { get; set; }

        [Display(Name = "Hasło:")]
        [Required(AllowEmptyStrings = false, ErrorMessage ="Hasło jest wymagane")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Zapamiętaj mnie!" )]
        public bool RememberMe { get; set; }
    }
}