using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CV_2_HR.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CV_2_HR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly IJobApplicationService _applicationService;

        public ApplicationsController(IJobApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet("{offerId}")]
        public async Task<IActionResult> Applications(int offerId)
        {
            var applications = await _applicationService.GetOfferApplicationsAsync(offerId);

            return Ok(applications);
        }
    }
}