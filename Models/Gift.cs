using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hushhlist.Models
{
    public class Gift
    {
        [Key]
        public int GiftId { get; set; }

        //[Required] - will this be auto-populated?
        public int EventId { get; set; }

        [Required(ErrorMessage = "Please enter an gift item"), StringLength(255)]
        [Display(Name = "Item")]
        public string ItemName { get; set; }

        [StringLength(450)]
        [Display(Name = "Description")]
        public string ItemDescription { get; set; }

        [Url, StringLength(200)]
        [Display(Name = "Link to item")]
        public string URL { get; set; }

        [Display(Name = "Age Restricted?")]
        public bool IsAgeRestricted { get; set; }

        
        public string GuestId { get; set; }
        [Display(Name ="Priority gift?")]
        public bool IsPriority { get; set; }

        public string Status { get; set; }
    }
}
