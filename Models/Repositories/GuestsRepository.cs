using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//namespace ytu_juniordevproject.Models.Repositories
namespace Hushhlist.Models.Repositories
{
    public class GuestsRepository
    {

        private readonly AppDbContext _appDbContext;

        public GuestsRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public List<Guest> GetAllGuestsForEvent(int EventId)
        {
            return _appDbContext.Guests.Where(x => x.EventId == EventId).ToList();
        }

        public Guest GetGuestById(string GuestId)
        {
            return _appDbContext.Guests.Single(x => x.GuestId == GuestId);
        }

        public string GetGuestIdByEmail(string Username)
        {
            var user = _appDbContext.Users.Single(x => x.Email == Username);
            return user.Id;
        }

        public void CreateGuest(Guest guest)
        {
            _appDbContext.Guests.Add(guest);
            _appDbContext.SaveChanges();
        }

        public void UpdateGuest(Guest guest)
        {
            Guest updatedGuest = GetGuestById(guest.GuestId);

            updatedGuest.FirstName = guest.FirstName;
            updatedGuest.LastName = guest.LastName;
            updatedGuest.LoginMethod = guest.LoginMethod;
            updatedGuest.Username = guest.Username;
            updatedGuest.IsOver18= guest.IsOver18;

            _appDbContext.Update(updatedGuest);
            _appDbContext.SaveChanges();
        }

        public void AcceptEventInvitation(string Email, int EventId, string UserId)
        {
            Guest guest = GetAllGuestsForEvent(EventId).Single(x => x.Username == Email);
            guest.UserId = UserId;
            guest.IsActive = true;

            _appDbContext.Update(guest);
            _appDbContext.SaveChanges();
        }
        
        public void DeleteGuest(string GuestId)
        {
            Guest guest = GetGuestById(GuestId);
            _appDbContext.Guests.Remove(guest);
            _appDbContext.SaveChanges();
        }

    }
}