using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hushhlist.Models
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<HostEvent> HostEvents { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Gift> Gifts { get; set; }
        public DbSet<Notification> Notifications { get; set; }
    }

}
