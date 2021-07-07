using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hushhlist.Models.Repositories;
using Hushhlist.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Hushhlist.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography.X509Certificates;
using System.ComponentModel;

namespace Hushhlist.Controllers
{
    public class NotificationController : Controller
    {
        private readonly NotificationsRepository _notificationRepository;

        public NotificationController(NotificationsRepository notificationsRepository)
        {
            _notificationRepository = notificationsRepository;
        }

        public IActionResult Index(string UserId)
        {
            NotificationsViewModel notificationsViewModel = new NotificationsViewModel()
            {
                Notifications = _notificationRepository.GetAllNotifications(UserId)
            };
            
            return View(notificationsViewModel);
        }
    }
}