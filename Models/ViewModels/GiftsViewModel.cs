using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hushhlist.Models.ViewModels
{
    public class GiftsViewModel
    {
        public int EventId { get; set; }
        public Gift Gift { get; set; }
        public List<Gift> Gifts { get; set; }
    }
}
