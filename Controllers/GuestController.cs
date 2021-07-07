using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hushhlist.Models.ViewModels;
using Hushhlist.Models.Repositories;
using Hushhlist.Models;
using Microsoft.AspNetCore.Identity;

namespace Hushhlist.Controllers
{
    public class GuestController : Controller
    {
        private readonly GuestsRepository _guestsRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public GuestController(GuestsRepository guestsRepository, UserManager<IdentityUser> userManager)
        {
            _guestsRepository = guestsRepository;
            _userManager = userManager;
        }

        public IActionResult Index(int EventId)
        {
            GuestsViewModel guestsViewModel = new GuestsViewModel()
            {
                Guests = _guestsRepository.GetAllGuestsForEvent(EventId)
            };
            return View(guestsViewModel);
        }

        public IActionResult Create(int EventId)
        {
            GuestsViewModel guestsViewModel = new GuestsViewModel()
            {
                EventId = EventId
            };
            return View(guestsViewModel);
        }

        [HttpPost]
        public IActionResult Create(GuestsViewModel guestsViewModel)
        {
            if (ModelState.IsValid)
            {
                Guest guest = new Guest()
                {
                    GuestId = _guestsRepository.GetGuestIdByEmail(guestsViewModel.Guest.Username),
                    EventId = guestsViewModel.EventId,
                    FirstName = guestsViewModel.Guest.FirstName,
                    LastName = guestsViewModel.Guest.LastName,
                    LoginMethod = guestsViewModel.Guest.LoginMethod,
                    Username = guestsViewModel.Guest.Username,
                    IsOver18 = guestsViewModel.Guest.IsOver18,
                    IsActive = false
                };

                _guestsRepository.CreateGuest(guest);
                return RedirectToAction("Details", "HostEvent", new { guestsViewModel.EventId });
            }
            else
            {
                return View("Create");
            }
        }

        public IActionResult Edit(string GuestId, int EventId)
        {
            Guest guest = _guestsRepository.GetGuestById(GuestId);

            GuestsViewModel guestsViewModel = new GuestsViewModel()
            {
                EventId = EventId,
                Guest = guest,
                GuestId = guest.GuestId
            };
            return View(guestsViewModel);
        }


        [HttpPost]
        public IActionResult Edit(GuestsViewModel guestsViewModel)
        {
            if (ModelState.IsValid)
            {
                _guestsRepository.UpdateGuest(guestsViewModel.Guest);
                return RedirectToAction("Details", "HostEvent", new { guestsViewModel.Guest.EventId });
            }

            return View("Edit");
        }

        public IActionResult Delete(int EventId, string GuestId)
        {
            _guestsRepository.DeleteGuest(GuestId);
            return RedirectToAction("Details", "HostEvent", new { EventId });
        }

        public IActionResult AcceptEventInvitation(int EventId)
        {
            _guestsRepository.AcceptEventInvitation(_userManager.GetUserName(User), EventId, _userManager.GetUserId(User));
            return RedirectToAction("Index", "HostEvent");
        }
    }
}