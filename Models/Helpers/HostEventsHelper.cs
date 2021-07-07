using Hushhlist.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hushhlist.Models.Helpers
{
    public class HostEventsHelper
    {
        private readonly AppDbContext _appDbContext;
        private readonly HostEventsRepository _hostEventsRepository;

        public HostEventsHelper(AppDbContext appDbContext, HostEventsRepository hostEventsRepository)
        {
            _appDbContext = appDbContext;
            _hostEventsRepository = hostEventsRepository;
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
                EventsAttendingList.Add(_hostEventsRepository.GetHostEvent(id));
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
                EventsAttendingList.Add(_hostEventsRepository.GetHostEvent(id));
            }

            return EventsAttendingList;
        }
    }
}
