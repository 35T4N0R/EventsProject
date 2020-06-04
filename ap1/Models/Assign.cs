using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ap1.Models
{
    public class Assign
    {

        [Display(Name = "Imie:")]
        public string Name { get; set; }

       
        public int EventId { get; set; }

        [Display(Name = "Nazwisko:")]
        [DataType(DataType.Password)]
        public string LastName { get; set; }

        [Display(Name = "Email")]
        public string EmailId { get; set; }

        [Display(Name = "NIP")]
        public string NIP { get; set; }

        [Display(Name = "Liczba biletów")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Ilość biletów musi być większa od 0!!!")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "pole wymagane")]
        public int Tickets { get; set; }
    }
}