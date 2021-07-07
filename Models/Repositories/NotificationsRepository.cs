using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hushhlist.Models.Repositories
{
    public class NotificationsRepository
    {
        private readonly AppDbContext _appDbContext;

        public NotificationsRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public List<Notification> GetAllNotifications(string UserId)
        {
            return _appDbContext.Notifications.Where(x => x.RecipientId == UserId).ToList();
        }

        public Notification GetNotificationById(int NotificationId)
        {
            return _appDbContext.Notifications.Single(x => x.NotificationId == NotificationId);
        }

        public void CreateNotification(string RecipientId, string SenderId, int NotificationType, int? GiftId, int? EventId, string Data)
        {
            Notification notification = new Notification()
            {
                RecipientId = RecipientId,
                SenderId = SenderId,
                TypeId = NotificationType,
                GiftId = GiftId,
                EventId = EventId,
                Data = Data,
                Read = false,
                Message = "Error"
            };

            _appDbContext.Notifications.Add(notification);
            _appDbContext.SaveChanges();

            notification.Message = WriteMessage(notification.NotificationId, notification.RecipientId, notification.SenderId, notification.TypeId, notification.GiftId, notification.EventId, notification.Data);

            _appDbContext.Notifications.Update(notification);
            _appDbContext.SaveChanges();
        }

        public string WriteMessage(int NotificationId, string RecipientId, string SenderId, int NotificationTypeId, int? GiftId, int? EventId, string Data)
        {
            Notification notification = GetNotificationById(NotificationId);

            switch (NotificationTypeId)
            {
                case 1:
                    notification.Message = "Someone has requested ownership transfer of an item you have reserved:" + GiftId;
                    break;
                case 2:
                    notification.Message = "Your request to reserve item " + GiftId + " has been " + Data;
                    break;
                case 3:
                    notification.Message = "There are only " + Data + " gifts left to buy for event " + EventId;
                    break;
                case 4:
                    notification.Message = "Someone has bought an item off your wishlist!";
                    break;
                case 5:
                    notification.Message = "You have been invited to " + SenderId + "'s event " + EventId;
                    break;
                case 6:
                    notification.Message = SenderId + " has accepted your invitation to event " + EventId;
                    break;
                case 7:
                    notification.Message = "Event coming up in " + Data + " days: " + EventId;
                    break;
                case 8:
                    notification.Message = "Event " + EventId + " has been " + Data;
                    break;
                case 9:
                    notification.Message = "Event " + EventId + "'s " + Data + "has changed to "; //Event.Date
                    break;
                case 10:
                    notification.Message = "Event " + "'s description has been updated";
                    break;
                case 11:
                    notification.Message = "Your event " + EventId + " has passed. See your final Hushhlist now";
                    break;
                case 12:
                    notification.Message = SenderId + " has just signed up to Hushhlist";
                    break;
                case 13:
                    notification.Message = SenderId + " has just changed their response to your event " + EventId + " to " + Data;
                    break;
                case 14:
                    notification.Message = SenderId + " has changed their contact details";
                    break;
                case 15:
                    notification.Message = "You have been removed from the guest list of event " + EventId;
                    break;
                default:
                    break;
            }

            return notification.Message;
        }
    }
}
