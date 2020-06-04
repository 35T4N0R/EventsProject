using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ap1.Models;

namespace ap1.Models
{
    public class EventViewModel
    {
        public int EventId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public System.DateTime DateAndTime { get; set; }
        public string Localization { get; set; }
        public int ExpectedTicketAmount { get; set; }
        public int MaxTicketAmountForOnePerson { get; set; }
        public string OrganiserEmail { get; set; }
    }
}