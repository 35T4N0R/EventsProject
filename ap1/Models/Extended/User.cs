using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ap1.Models
{
    [MetadataType(typeof(UserMetadata))]
    public partial class User
    {
        public string ConfirmPassword { get; set; }
    }

    public class UserMetadata
    {
        [Display(Name = "Imię")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "pole wymagane")]
        public string FirstName { get; set; }

        [Display(Name = "Nazwisko")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "pole wymagane")]
        public string LastName { get; set; }

        [Display(Name = "E-mail")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "pole wymagane")]
        [DataType(DataType.EmailAddress)]
        public string EmailID { get; set; }

        [Display(Name = "Data urodzenia")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> DateOfBirth { get; set; }

        [Display(Name = "Hasło")]
        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "pole wymagane")]
        [MinLength(6, ErrorMessage = "Hasło ma mieć minimum 6 znaków")]
        public string Password { get; set; }

        [Display(Name = "Powtórz Hasło")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "nie zgadza się")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Numer NIP")]
        [MinLength(10, ErrorMessage = "NIP musi mieć 10 znaków")]
        [StringLength(10, ErrorMessage = "NIP musi mieć 10 znaków")]
        public string NIP { get; set; }


    }
}