using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hushhlist.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        [Required]
        public string RecipientId { get; set; }
        public string SenderId { get; set; } 
        [Required]
        public int TypeId { get; set; }
        [Required]
        public string Message { get; set; }
        public int? GiftId { get; set; }
        public int? EventId { get; set; }
        public string Data { get; set; }
        [Required]
        public bool Read { get; set; }
    }
}
