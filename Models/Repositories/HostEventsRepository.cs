
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using Hushhlist.Models.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hushhlist.Models.Repositories
{
    public class HostEventsRepository
    {
        private readonly AppDbContext _appDbContext;
        //private readonly HostEventsHelper _hostEventsHelper;
        //private readonly UserManager<IdentityUser> _userManager;

        //public HostEventsRepository(AppDbContext appDbContext, UserManager<IdentityUser> userManager)
        //public HostEventsRepository(AppDbContext appDbContext, HostEventsHelper hostEventsHelper)
        public HostEventsRepository(AppDbContext appDbContext)
        {
            //_userManager = userManager;
            _appDbContext = appDbContext;
            //_hostEventsHelper = hostEventsHelper;
        }

        public HostEvent GetHostEvent(int EventId)
        {
            return _appDbContext.HostEvents.Single(x => x.EventId == EventId);
        }

        public List<HostEvent> GetAllHostEvents()
        {
            return _appDbContext.HostEvents.OrderBy(x => x.EventDate).ToList();
        }

        public void CreateHostEvent(HostEvent hostEvent)
        {
            if (hostEvent.Status != "Upcoming")
            {
                hostEvent.EventDate = null;
            }
            _appDbContext.HostEvents.Add(hostEvent);
            _appDbContext.SaveChanges();
        }

        public void UpdateHostEvent(HostEvent hostEvent)
        {
            HostEvent updatedHostEvent = GetHostEvent(hostEvent.EventId);

            updatedHostEvent.EventTitle = hostEvent.EventTitle;
            updatedHostEvent.Location = hostEvent.Location;
            updatedHostEvent.EventDescription = hostEvent.EventDescription;
            if (hostEvent.Status == "Upcoming")
            {
                updatedHostEvent.EventDate = hostEvent.EventDate;
            }
            else
            {
                updatedHostEvent.EventDate = null;
            }
            updatedHostEvent.EventPassword = hostEvent.EventPassword;
            updatedHostEvent.MaxReservable = hostEvent.MaxReservable;
            updatedHostEvent.Status = hostEvent.Status;

            _appDbContext.Update(updatedHostEvent);
            _appDbContext.SaveChanges();
        }

        public void DeleteHostEvent(int EventId)
        {
            HostEvent hostEvent = GetHostEvent(EventId);
            List<Guest> guests = _appDbContext.Guests.Where(x => x.EventId == EventId).ToList();
            foreach (var guest in guests)
            {
                _appDbContext.Guests.Remove(guest);
            }
            _appDbContext.HostEvents.Remove(hostEvent);
            _appDbContext.SaveChanges();
        }

        public List<HostEvent> GetAllEventsHosting(string UserId)
        {
            return _appDbContext.HostEvents.Where(x => x.HostId == UserId).ToList();
        }

        public List<HostEvent> GetAllEventsAttending(string UserId)
        {
            var AllGuests = _appDbContext.Guests.Where(x => x.UserId == UserId).Where(x => x.IsActive == true).ToList();
            var EventIds = new List<int>();

            foreach (var guest in AllGuests)
            {
                EventIds.Add(guest.EventId);
            }

            var EventsAttendingList = new List<HostEvent>();

            foreach (var id in EventIds)
            {
                EventsAttendingList.Add(GetHostEvent(id));
            }

            return EventsAttendingList;
        }

        public List<HostEvent> GetEventInvitations(string Email)
        {
            var AllGuests = _appDbContext.Guests.Where(x => x.Username == Email).Where(x => x.IsActive == false).ToList();
            var EventIds = new List<int>();

            foreach (var guest in AllGuests)
            {
                EventIds.Add(guest.EventId);
            }

            var EventsAttendingList = new List<HostEvent>();

            foreach (var id in EventIds)
            {
                EventsAttendingList.Add(GetHostEvent(id));
            }

            return EventsAttendingList;
        }

        public string GetHost(int EventId)
        {
            HostEvent hostevent = _appDbContext.HostEvents.Single(x => x.EventId == EventId);
            return hostevent.HostId;
        }
    }
}
