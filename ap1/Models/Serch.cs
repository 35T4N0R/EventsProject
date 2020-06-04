using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ap1.Models
{
    public class Serch
    {
        [Display(Name = "Lokalizacja")]
        public string Localization { get; set; }

        [Display(Name = "Od")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public System.DateTime MinDate { get; set; }

        [Display(Name = "Do")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public System.DateTime MaxDate { get; set; }

        public List<Event> list { get; set; }

    }
}