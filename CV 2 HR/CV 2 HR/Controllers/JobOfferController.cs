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
        private IJobOfferService _offerService;
        private ICompanyService _companyService;

        public JobOfferController(IJobOfferService offerService, ICompanyService companyService)
        {
            _offerService = offerService;
            _companyService = companyService;
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

            return View(offer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> Edit(JobOffer newOffer)
        {
            if (!ModelState.IsValid)
                return View(newOffer);
            
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