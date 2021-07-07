using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hushhlist.Models;
using Hushhlist.Models.Repositories;
using Hushhlist.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hushhlist.Controllers
{
    public class GiftController : Controller
    {
        private readonly GiftsRepository _giftsRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public GiftController(GiftsRepository giftsRepository, UserManager<IdentityUser> userManager)
        {
            _giftsRepository = giftsRepository;
            _userManager = userManager;
        }

        public IActionResult Index(int EventId)
        {
            HostEventsViewModel hostEventsViewModel = new HostEventsViewModel()
            {
                Gifts = _giftsRepository.GetAllGiftsForEvent(EventId)
            };

            return View(hostEventsViewModel);
        }

        public IActionResult Details(int GiftId)
        {
            return View(_giftsRepository.GetGiftById(GiftId));
        }

        public IActionResult Create(int EventId)
        {
            GiftsViewModel giftsViewModel = new GiftsViewModel()
            {
                EventId = EventId
            };

            return View(giftsViewModel);
        }

        [HttpPost]
        public IActionResult Create(GiftsViewModel giftsViewModel)
        {
            Gift gift = new Gift()
            {
                EventId = giftsViewModel.EventId,
                ItemName = giftsViewModel.Gift.ItemName,
                ItemDescription = giftsViewModel.Gift.ItemDescription,
                IsAgeRestricted = giftsViewModel.Gift.IsAgeRestricted,
                IsPriority = giftsViewModel.Gift.IsPriority
            };

            _giftsRepository.CreateGift(gift);
            return RedirectToAction("HostEvent", "Details", gift.EventId);
        }

        public IActionResult Edit(int GiftId)
        {
            return View(_giftsRepository.GetGiftById(GiftId));
        }

        [HttpPost]
        public IActionResult Edit(int GiftId, Gift gift)
        {
            if (ModelState.IsValid)
            {
                _giftsRepository.UpdateGift(GiftId, gift);
                return RedirectToAction("Index");
            }
            return View("Edit"); //check this is the correct return - need modelState for validation
        }

        public IActionResult Delete(int GiftId)
        {
            _giftsRepository.DeleteGift(GiftId);
            return RedirectToAction("Index");
        }

        // ---------------------------------------- Guest only -----------------------------------

        public IActionResult Index_Guest()
        {
            HostEventsViewModel hostEventsViewModel = new HostEventsViewModel()
            {
                Gifts = _giftsRepository.GetAllGifts()
            };

            return View(hostEventsViewModel);
        }
        
        public IActionResult ReserveGift(int EventId, int GiftId)
        {
            HostEventsViewModel hostEventsViewModel = new HostEventsViewModel()
            {
                Gifts = _giftsRepository.GetAllGiftsForEvent(EventId)
            };

            if (_giftsRepository.CheckGiftLimit(_userManager.GetUserId(User), EventId))
            {
                _giftsRepository.ReserveGift(GiftId, _userManager.GetUserId(User));
                return View("Index", hostEventsViewModel);
            }
            else
            {
                ModelState.AddModelError("", "You have already claimed the maximum number of gifts permitted");
                return View("Index", hostEventsViewModel);
            }
        }

        public IActionResult MarkAsBought(int EventId, int GiftId)
        {
            Gift gift = _giftsRepository.GetGiftById(GiftId);
            var userId = _userManager.GetUserId(User);

            HostEventsViewModel hostEventsViewModel = new HostEventsViewModel()
            {
                Gifts = _giftsRepository.GetAllGiftsForEvent(EventId)
            };

            if(gift.Status == "Reserved" && gift.GuestId == userId)
            {
                _giftsRepository.MarkAsBought(gift.GiftId, userId);
            }
            else if (_giftsRepository.CheckGiftLimit(_userManager.GetUserId(User), EventId))
            {
                _giftsRepository.MarkAsBought(GiftId, _userManager.GetUserId(User));
                return View("Index", hostEventsViewModel);
            }
            else
            {
                ModelState.AddModelError("", "You have already reserved the maximum number of gifts permitted");
            }

            return View("Index", hostEventsViewModel);
        }

        public IActionResult Assign(int EventId, int GiftId, string Status)
        {
            HostEventsViewModel hostEventsViewModel = new HostEventsViewModel()
            {
                Gifts = _giftsRepository.GetAllGiftsForEvent(EventId)
            };

            if (_giftsRepository.CheckGiftLimit(_userManager.GetUserId(User), EventId))
            {
                _giftsRepository.AssignGift(GiftId, Status, _userManager.GetUserId(User));
                return View("Index", hostEventsViewModel);
            }
            else
            {
                ModelState.AddModelError("", "You have already reserved the maximum number of gifts permitted");
                return View("Index", hostEventsViewModel);
            }
        }

        public IActionResult Unassign(int EventId, int GiftId)
        {
            _giftsRepository.UnassignGift(GiftId);

            HostEventsViewModel hostEventsViewModel = new HostEventsViewModel()
            {
                Gifts = _giftsRepository.GetAllGiftsForEvent(EventId)
            };

            return View("Index", hostEventsViewModel);
        }

        public IActionResult RequestOwnership(int EventId, int GiftId)
        {
            HostEventsViewModel hostEventsViewModel = new HostEventsViewModel()
            {
                Gifts = _giftsRepository.GetAllGiftsForEvent(EventId)
            };

            _giftsRepository.SendOwnershipRequest(_userManager.GetUserId(User), GiftId);
            ModelState.AddModelError("", "Your request has been sent");

            return View("Index", hostEventsViewModel);
        }
    }
}