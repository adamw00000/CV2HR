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
    [Authorize]
    public class JobApplicationController : Controller
    {
        private readonly IJobApplicationService _applicationService;
        private readonly IJobOfferService _offerService;
        private readonly IBlobService _blobService;

        public JobApplicationController(IJobApplicationService applicationService, IJobOfferService offerService, IBlobService blobService)
        {
            _applicationService = applicationService;
            _offerService = offerService;
            _blobService = blobService;
        }
        
        public async Task<IActionResult> Add(int id)
        {
            var viewModel = new JobApplicationCreateViewModel { OfferId = id };

            var offer = await _offerService.GetOfferAsync(id);
            viewModel.Offer = offer;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(JobApplicationCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var offer = await _offerService.GetOfferAsync(viewModel.OfferId);
                viewModel.Offer = offer;
                return View(viewModel);
            }

            _blobService.ValidateFile(viewModel.CvFile, ModelState);

            if (!ModelState.IsValid)
            {
                var offer = await _offerService.GetOfferAsync(viewModel.OfferId);
                viewModel.Offer = offer;
                return View(viewModel);
            }

            var fileName = _blobService.GetFileName(viewModel);
            var succeeded = await _blobService.AddFile(viewModel.CvFile, fileName);

            if (!succeeded)
                return StatusCode(500);

            JobApplication jobApplication = viewModel as JobApplication;
            jobApplication.CvFileName = fileName;

            succeeded = await _applicationService.AddJobApplicationAsync(jobApplication);

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