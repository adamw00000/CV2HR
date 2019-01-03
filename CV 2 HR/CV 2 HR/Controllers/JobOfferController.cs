using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CV2HR.Models;
using CV2HR.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CV2HR.Controllers
{
    public class JobOfferController : Controller
    {
        private readonly IJobOfferService _offerService;
        private readonly ICompanyService _companyService;
        private readonly IUserManager _userManager;

        public JobOfferController(IJobOfferService offerService, ICompanyService companyService, IUserManager userManager)
        {
            _offerService = offerService;
            _companyService = companyService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var jobOffers = await _offerService.GetJobOffersAsync();

            return View(jobOffers);
        }

        //[HttpGet]
        //public async Task<IActionResult> Index([FromQuery(Name = "search")] string searchstring)
        //{
        //    if (String.IsNullOrEmpty(searchstring))
        //    {
        //        var jobOffers = await _offerService.GetJobOffersAsync();
        //        return View(jobOffers);
        //    }

        //    var searchResult = await _offerService.GetJobOffersSearchResultAsync(searchstring);

        //    return View(searchResult);
        //}

        public async Task<IActionResult> Details(int id)
        {
            var offer = await _offerService.GetOfferAsync(id);

            if (offer == null)
                return NotFound();

            return View(offer);
        }

        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
                return BadRequest();

            var offer = await _offerService.GetOfferAsync(id.Value);

            if (offer == null)
                return NotFound();

            var userId = _userManager.GetUserId();
            if (userId == offer.UserId)
                return View(offer);

            var adminAuthorizationResult = _userManager.AuthorizeUserAsync("Admin");
            if ((await adminAuthorizationResult).Succeeded)
                return View(offer);

            return RedirectToAction("Denied", "Session");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> Edit(JobOffer newOffer)
        {
            if (!ModelState.IsValid)
            {
                var offer = await _offerService.GetOfferAsync(newOffer.Id);
                newOffer.Company = offer.Company;
                return View(newOffer);
            }
            
            var userId = _userManager.GetUserId();
            var adminAuthorizationResult = _userManager.AuthorizeUserAsync("Admin");
            if (userId != newOffer.UserId && !(await adminAuthorizationResult).Succeeded)
                return RedirectToAction("Denied", "Session");

            bool succeeded = await _offerService.UpdateOffer(newOffer);

            if (!succeeded)
                return StatusCode(500);

            return RedirectToAction("Details", new { id = newOffer.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();

            var offer = await _offerService.GetOfferAsync(id.Value);

            if (offer == null)
                return NotFound();

            var userId = _userManager.GetUserId();
            var adminAuthorizationResult = _userManager.AuthorizeUserAsync("Admin");
            if (userId != offer.UserId && !(await adminAuthorizationResult).Succeeded)
                return RedirectToAction("Denied", "Session");

            bool succeeded = await _offerService.RemoveOffer(offer);

            if (!succeeded)
                return StatusCode(500);

            return RedirectToAction("Index");
        }

        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> Create()
        {
            var companies = await _companyService.GetCompaniesAsync();

            var model = new JobOfferCreateViewModel() { Companies = companies };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> Create(JobOfferCreateViewModel model)
        {
            var userId = _userManager.GetUserId();
            model.UserId = userId;

            ModelState.Clear();
            var result = TryValidateModel(model);
            if (!ModelState.IsValid)
            {
                var companies = await _companyService.GetCompaniesAsync();
                model.Companies = companies;
                return View(model);
            }
            
            bool succeeded = await _offerService.AddJobOfferAsync(model as JobOffer);

            if (!succeeded)
                return StatusCode(500);

            return RedirectToAction("Index");
        }
    }
}