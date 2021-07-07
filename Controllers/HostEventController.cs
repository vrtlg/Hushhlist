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
//using Hushhlist.Models.Helpers;

namespace Hushhlist.Controllers
{
    [Authorize]
    public class HostEventController : Controller
    {
        private readonly HostEventsRepository _hostEventsRepository;
        //private readonly HostEventsHelper _hostEventsHelper;
        private readonly GiftsRepository _giftsRepository;
        private readonly GuestsRepository _guestsRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        //public HostEventController(HostEventsRepository eventsRepository, HostEventsHelper hostEventsHelper, UserManager<IdentityUser> userManager, GiftsRepository giftsRepository, GuestsRepository guestsRepository)
        public HostEventController(HostEventsRepository eventsRepository, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, GiftsRepository giftsRepository, GuestsRepository guestsRepository)
        {
            _hostEventsRepository = eventsRepository;
            //_hostEventsHelper = hostEventsHelper;
            _userManager = userManager;
            _giftsRepository = giftsRepository;
            _guestsRepository = guestsRepository;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            string[] roles = new string[]{ "Host", "Guest" };

            foreach (var role in roles)
            {
                if (await _roleManager.FindByNameAsync(role) == null)
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            //await _userManager.AddToRoleAsync(await _userManager.GetUserAsync(User), "Guest");
            

            string UserId = _userManager.GetUserId(User);

            HostEventsViewModel hostEventsViewModel = new HostEventsViewModel()
            {
                EventsHosting = _hostEventsRepository.GetAllEventsHosting(UserId).ToList(),
                EventsAttending = _hostEventsRepository.GetAllEventsAttending(UserId).ToList(),
                EventInvitations = _hostEventsRepository.GetEventInvitations(_userManager.GetUserName(User)).ToList()
            };

            return View(hostEventsViewModel);
        }

        public IActionResult Details(int EventId)
        {
            HostEventsViewModel hostEventsViewModel = new HostEventsViewModel
            {
                HostEvent = _hostEventsRepository.GetHostEvent(EventId),
                Guests = _guestsRepository.GetAllGuestsForEvent(EventId),
                Gifts = _giftsRepository.GetAllGiftsForEvent(EventId)

            };
            return View(hostEventsViewModel);
        }

        public IActionResult Create()
        {
            List<SelectListItem> Status = new List<SelectListItem>
            {
                new SelectListItem() { Text = "TBA", Value = "TBA" },
                new SelectListItem() { Text = "Upcoming", Value = "Upcoming" }
            };

            HostEventsViewModel hostEventsViewModel = new HostEventsViewModel()
            {
                Status = Status
            };

            return View(hostEventsViewModel);
        }

        [HttpPost]
        public IActionResult Create(string HostId, HostEventsViewModel hostEventsViewModel)
        {

            if (ModelState.IsValid)
            {
                PasswordHasher passwordHasher = new PasswordHasher();
                //var password = passwordHasher.HashToString(hostEventsViewModel.HostEvent.EventPassword);

                HostEvent hostEvent = new HostEvent()
                {
                    EventTitle = hostEventsViewModel.HostEvent.EventTitle,
                    EventDescription = hostEventsViewModel.HostEvent.EventDescription,
                    EventDate = hostEventsViewModel.HostEvent.EventDate,
                    //EventPassword = password,
                    EventPassword = passwordHasher.HashToString(hostEventsViewModel.HostEvent.EventPassword),
                    Location = hostEventsViewModel.HostEvent.Location,
                    HostId = _userManager.GetUserId(User),
                    MaxReservable = hostEventsViewModel.HostEvent.MaxReservable,
                    Status = hostEventsViewModel.HostEvent.Status,

            };

                if (hostEventsViewModel.HostEvent.Status == null)
                {
                    hostEvent.Status = "TBC";
                };

                
                _hostEventsRepository.CreateHostEvent(hostEvent);
                return RedirectToAction("Index");
            }
            return View("Create");
        }

        [HttpGet]
        public IActionResult Edit(int EventId)
        {
            HostEvent hostEvent = _hostEventsRepository.GetHostEvent(EventId);

            List<SelectListItem> Status = new List<SelectListItem>
            {
                new SelectListItem() { Text = "TBA", Value = "TBA" },
                new SelectListItem() { Text = "Upcoming", Value = "Upcoming" },
                new SelectListItem() { Text = "Postponed", Value = "Postponed" },
                new SelectListItem() { Text = "Cancelled", Value = "Cancelled" }
            };

            HostEventsViewModel hostEventsViewModel = new HostEventsViewModel()
            {
                HostEvent = hostEvent,
                EventId = hostEvent.EventId,
                Status = Status
            };

            return View( hostEventsViewModel);
        }

        [HttpPost]
        public IActionResult Edit(HostEventsViewModel hostEventsViewModel)
        {

            if (ModelState.IsValid)
            {
                _hostEventsRepository.UpdateHostEvent(hostEventsViewModel.HostEvent);
                return RedirectToAction("Index", hostEventsViewModel);
            }
            else
            {
                return View(hostEventsViewModel);
            }
        }

        public IActionResult Delete(int EventId)
        {
            _hostEventsRepository.DeleteHostEvent(EventId);
            return RedirectToAction("Index");
        }
    }
}
