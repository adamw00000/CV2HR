using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CV_2_HR.Models;
using CV_2_HR.Services;
using Microsoft.AspNetCore.Mvc;

namespace CV_2_HR.Controllers
{
    public class JobApplicationController : Controller
    {
        private IJobApplicationService _applicationService;
        private IJobOfferService _offerService;

        public JobApplicationController(IJobApplicationService applicationService, IJobOfferService offerService)
        {
            _applicationService = applicationService;
            _offerService = offerService;
        }

        public async Task<IActionResult> Add(int id)
        {
            var jobApplication = new JobApplication { OfferId = id };

            var offer = await _offerService.GetOfferAsync(id);
            jobApplication.Offer = offer;
            return View(jobApplication);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(JobApplication jobApplication)
        {
            if (!ModelState.IsValid)
            {
                var offer = await _offerService.GetOfferAsync(jobApplication.OfferId);
                jobApplication.Offer = offer;
                return View(jobApplication);
            }

            bool succeeded = await _applicationService.AddJobApplicationAsync(jobApplication);

            if (!succeeded)
                return StatusCode(500);

            return RedirectToAction("Details", "JobOffer", new { Id = jobApplication.OfferId });
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return BadRequest();

            var application = await _applicationService.GetJobApplicationAsync(id.Value);

            return View(application);
        }
    }
}