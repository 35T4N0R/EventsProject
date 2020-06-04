using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ap1.Models
{
    [MetadataType(typeof(EventMetadata))]
    public partial class Event
    {

    }

    public class EventMetadata
    {
        [Display(Name = "Nazwa wydarzenia")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "pole wymagane")]
        public string Name { get; set; }

        [Display(Name = "Typ wydarzenia")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "pole wymagane")]
        public string Type { get; set; }

        [Display(Name = "Miejsce")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "pole wymagane")]
        public string Localization { get; set; }

        [Display(Name = "Data")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> Date { get; set; }

        [Display(Name = "Ilość przewidzianych biletów")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "pole wymagane")]
        [Range(1,Int32.MaxValue, ErrorMessage = "Ilość biletów musi być większa od 0!!!")]
        public int ExpectedTicketAmount { get; set; }

        [Display(Name = "Limit biletów na osobę")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "pole wymagane")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Ilość biletów musi być większa od 0!!!")]
        public int MaxTicketAmountForOnePerson { get; set; }

        [Display(Name = "Godzina")]
        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:t}")]
        
        public System.TimeSpan Time { get; set; }

        public string OrganiserEmail { get; set; }
    }
}