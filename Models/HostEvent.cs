//using ExpressiveAnnotations.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace Hushhlist.Models
{
    public class HostEvent
    {
        [Key]
        public int EventId { get; set; }

        public string HostId { get; set; }

        [Required(ErrorMessage = "Please enter a Title for this event."), StringLength(255)] 
        [Display(Name = "Title")]
        public string EventTitle { get; set; }


        [DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}")]
        [Display(Name = "Date")]
        public DateTime? EventDate { get; set; } 

        [StringLength(450)]
        [Display(Name = "Description")]
        public string EventDescription { get; set; }
        public string Location { get; set; }
        //needs to be a hash!!

        //public string Location {get; set;}

        //needs to be stored in DB as a hash!!
        //[Required(ErrorMessage = "Please enter a Password for the event."), StringLength(20)]
        //[DataType(DataType.Password)]
        //[Display(Name = "Event Password")]
        //public string EventPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string EventPassword { get; set; }

        //[DataType(DataType.Password)]
        //[Display(Name = "Confirm password")]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        //public string ConfirmPassword { get; set; }



        //Default to 3, is this a controller thing?
        [Range(0, 10), DefaultValue(3)]// can only set between 0 and 10 gifts as reservable?
        [Display(Name = "Max reservable gifts")]
        public int MaxReservable { get; set; }

        //[custom validator - status required if you've put a date in
        //[RequiredIf("EventDate != null", ErrorMessage = "Please enter a status or remove date if TBC")]
        [Display(Name = "Status")]
        public string Status { get; set; }

        public List<Guest> Guests { get; set; }

        public List<Gift> Gifts { get; set; }
    }
}
