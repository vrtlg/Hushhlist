using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hushhlist.Models.Repositories
{
    public class GiftsRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly NotificationsRepository _notificationsRepository;
        private readonly GuestsRepository _guestsRepository;
        private readonly HostEventsRepository _hostEventsRepository;

        public GiftsRepository(AppDbContext appDbContext, NotificationsRepository notificationsRepository, GuestsRepository guestsRepository, HostEventsRepository hostEventsRepository)
        {
            _appDbContext = appDbContext;
            _notificationsRepository = notificationsRepository;
            _guestsRepository = guestsRepository;
            _hostEventsRepository = hostEventsRepository;
        }

        public Gift GetGiftById(int GiftId)
        {
            return _appDbContext.Gifts.Single(x => x.GiftId == GiftId);
        }

        public List<Gift> GetAllGifts()
        {
            return _appDbContext.Gifts.OrderByDescending(x => x.IsPriority).ToList();
        }

        public List<Gift> GetAllGiftsForEvent(int EventId)
        {
            return _appDbContext.Gifts.Where(x => x.EventId == EventId).ToList();
        }

        public void CreateGift(Gift gift)
        {
            _appDbContext.Gifts.Add(gift);
            _appDbContext.SaveChanges();
        }

        public void UpdateGift(int GiftId, Gift gift)
        {
            Gift updatedGift = GetGiftById(GiftId);

            updatedGift.ItemName = gift.ItemName;
            updatedGift.ItemDescription = gift.ItemDescription;
            updatedGift.URL = gift.URL;
            updatedGift.IsPriority = gift.IsPriority;
            updatedGift.IsAgeRestricted = gift.IsAgeRestricted;

            _appDbContext.Gifts.Update(updatedGift);
            _appDbContext.SaveChanges();
        }

        public void DeleteGift(int GiftId)
        {
            Gift updatedGift = GetGiftById(GiftId);
            _appDbContext.Gifts.Remove(updatedGift);
            _appDbContext.SaveChanges();
        }

        public void ReserveGift(int GiftId, string GuestId)
        {
            Gift updatedGift = GetGiftById(GiftId);
            updatedGift.Status = "Reserved";
            updatedGift.GuestId = GuestId;

            _appDbContext.Gifts.Update(updatedGift);
            _appDbContext.SaveChanges();
        }

        public void MarkAsBought(int GiftId, string GuestId)
        {
            Gift updatedGift = GetGiftById(GiftId);
            updatedGift.Status = "Bought";
            updatedGift.GuestId = GuestId;
            

            var GiftsLeft = GetAllGiftsForEvent(updatedGift.EventId).Where(x => x.Status == null).Count();
            List<Guest> EventGuests = _guestsRepository.GetAllGuestsForEvent(updatedGift.EventId);

            if (GiftsLeft == 5 || GiftsLeft == 3 || GiftsLeft == 1)
            {
                foreach (var guest in EventGuests)
                {
                    _notificationsRepository.CreateNotification(guest.UserId, null, 3, GiftId, updatedGift.EventId, GiftsLeft.ToString());
                }
                
            }
            _notificationsRepository.CreateNotification(_hostEventsRepository.GetHost(updatedGift.EventId), null, 4, null, updatedGift.EventId, null);
            _appDbContext.Gifts.Update(updatedGift);
            _appDbContext.SaveChanges();
        }

        public void AssignGift(int GiftId, string Status, string GuestId) // move to helper later?
        {
            Gift updatedGift = GetGiftById(GiftId);
            updatedGift.Status = Status;
            updatedGift.GuestId = GuestId;

            _appDbContext.Gifts.Update(updatedGift);
            _appDbContext.SaveChanges();
        }

        public void UnassignGift(int GiftId) //move to helper later?
        {
            Gift updatedGift = GetGiftById(GiftId);
            updatedGift.Status = null;
            updatedGift.GuestId = null;

            _appDbContext.Gifts.Update(updatedGift);
            _appDbContext.SaveChanges();
        }

        public bool CheckGiftLimit(string UserId, int EventId)
        {
            HostEvent hostEvent = _appDbContext.HostEvents.Single(x => x.EventId == EventId);
            //int numReserved = _appDbContext.Gifts.Count(x => x.EventId == EventId && x.GuestId == UserId && x.Status == "Reserved");
            //int numBought = _appDbContext.Gifts.Count(x => x.EventId == EventId && x.GuestId == UserId && x.Status == "Bought");
            int numOwned = _appDbContext.Gifts.Count(x => x.EventId == EventId && x.GuestId == UserId);
            if(hostEvent.MaxReservable > numOwned)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SendOwnershipRequest(string UserId, int GiftId)
        {
            Gift gift = GetGiftById(GiftId);
            //string hostId = _appDbContext.HostEvents.Include(x => x.HostId).Single(x => x.EventId == gift.EventId).ToString();
            _notificationsRepository.CreateNotification(gift.GuestId, UserId, 1, GiftId, gift.EventId, null);
        }
    }
}
