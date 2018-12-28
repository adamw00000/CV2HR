using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CV_2_HR.Models;
using CV_2_HR.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CV_2_HR.Controllers
{
    public class JobOfferController : Controller
    {
        private readonly IJobOfferService _offerService;
        private readonly ICompanyService _companyService;
        private readonly IAuthorizationService _authorizationService;

        public JobOfferController(IJobOfferService offerService, ICompanyService companyService, IAuthorizationService authorizationService)
        {
            _offerService = offerService;
            _companyService = companyService;
            _authorizationService = authorizationService;
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

            var userId = HttpContext.User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            if (userId == offer.UserId)
                return View(offer);

            var adminAuthorizationResult = _authorizationService.AuthorizeAsync(User, "Admin");
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
            
            var userId = HttpContext.User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var adminAuthorizationResult = _authorizationService.AuthorizeAsync(User, "Admin");
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

            var userId = HttpContext.User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var adminAuthorizationResult = _authorizationService.AuthorizeAsync(User, "Admin");
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
            var userId = HttpContext.User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
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