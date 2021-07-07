using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Hushhlist.Models
{
    public class Guest
    {
        [Key]
        public string GuestId { get; set; }
        public string UserId { get; set; }

        [Required, StringLength(255)]
        [Display(Name="First name")] //or 'first name'
        public string FirstName { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name="Last name/Initial")] //need to be able to distinguish guests with similar names for the host
        public string LastName { get; set; }
        
        [Required, StringLength(50)]
        public string Username { get; set; }

        [Required, StringLength(100)] //drop-down?
        [Display(Name="Login method")]
        public string LoginMethod { get; set; } //notificationmethod rather than loginmethod?

        [Display(Name="Over 18?")]
        //[some sort of server side check where they enter their birth date but it isn't stored?
        public bool IsOver18 { get; set; }

        public bool IsActive { get; set; }
        public int EventId { get; set; }
  //      public int GuestStatusId { get; set; }
    }

    //public enum GuestStatus 
    //{ 
    //    Attending = 1,
    //    Declined = 2,
    //    AwaitingResponse = 3
    //}

}

