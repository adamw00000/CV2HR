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
        private readonly IUserManager _userManager;

        public JobApplicationController(IJobApplicationService applicationService, IJobOfferService offerService, 
            IBlobService blobService, IUserManager userManager)
        {
            _applicationService = applicationService;
            _offerService = offerService;
            _blobService = blobService;
            _userManager = userManager;
        }
        
        public async Task<IActionResult> Add(int id)
        {
            var viewModel = new JobApplicationCreateViewModel { OfferId = id };

            var offer = await _offerService.GetOfferAsync(id);
            if (offer == null)
                return NotFound();

            viewModel.Offer = offer;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(JobApplicationCreateViewModel viewModel)
        {
            if (viewModel == null)
                throw new NullReferenceException();

            var userId = _userManager.GetUserId();
            viewModel.UserId = userId;

            ModelState.ClearValidationState("UserId");
            var result = TryValidateModel(viewModel);
            _blobService.ValidateFile(viewModel.CvFile, ModelState);

            if (!result || !ModelState.IsValid)
            {
                var offer = await _offerService.GetOfferAsync(viewModel.OfferId);
                viewModel.Offer = offer;
                return View(viewModel);
            }

            var fileName = _blobService.GetFileName(viewModel);
            var uri = await _blobService.AddFile(viewModel.CvFile, fileName);

            if (uri == null)
                return StatusCode(500);

            JobApplication jobApplication = viewModel as JobApplication;
            jobApplication.CvUri = uri.ToString();

            var succeeded = await _applicationService.AddJobApplicationAsync(jobApplication);

            if (!succeeded)
            {
                await _blobService.RemoveFile(fileName);
                return StatusCode(500);
            }

            return RedirectToAction("Details", "JobOffer", new { Id = jobApplication.OfferId });
        }
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return BadRequest();

            var application = await _applicationService.GetJobApplicationAsync(id.Value);
            if (application == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId();

            if (userId == application.UserId)
                return View(application);

            var user = _userManager.GetUser();

            var managerAuthorizationResult = _userManager.AuthorizeUserAsync("Manager");
            var adminAuthorizationResult = _userManager.AuthorizeUserAsync("Admin");
            if ((await adminAuthorizationResult).Succeeded || 
                ((await managerAuthorizationResult).Succeeded && application.Offer.UserId == userId))
                return View(application);

            return RedirectToAction("Denied", "Session");
        }
    }
}