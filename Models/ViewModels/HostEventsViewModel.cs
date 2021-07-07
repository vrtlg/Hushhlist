using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hushhlist.Models.ViewModels
{
    public class HostEventsViewModel
    {
        public int EventId { get; set; }
        public List<HostEvent> EventsHosting { get; set; }
        public List<HostEvent> EventsAttending { get; set; }
        public List<HostEvent> EventInvitations { get; set; }
        public HostEvent HostEvent { get; set; }
        public List<Guest> Guests { get; set; }
        public Guest Guest { get; set; }
        public List<Gift> Gifts { get; set; }
        public Gift Gift { get; set; }
        public List<SelectListItem> Status { get; set; }
    }
}
