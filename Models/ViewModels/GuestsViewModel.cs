using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hushhlist.Models;

//namespace ytu_juniordevproject.Models.ViewModels
namespace Hushhlist.Models.ViewModels
{
    public class GuestsViewModel
    {
        public string GuestId { get; set; }
        public int EventId { get; set; }
        public List<Guest> Guests { get; set; } 
        public Guest Guest { get; set; }
        public List<HostEvent> HostEvents { get; set; }
        public HostEvent HostEvent { get; set; }
        public List<Gift> Gifts { get; set; }
        public Gift Gift { get; set; }

        public bool IsOver18  { get; set; }

    }
}