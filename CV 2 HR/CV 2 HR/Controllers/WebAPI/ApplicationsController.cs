using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CV2HR.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CV2HR.Controllers.Api
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

        /// <summary>
        /// Gets list of applications for the job offer
        /// </summary>
        /// <param name="offerId">Job offer id</param>
        /// <returns>A list of applications</returns>
        [HttpGet("{offerId}")]
        public async Task<IActionResult> Applications(int offerId)
        {
            var applications = await _applicationService.GetOfferApplicationsAsync(offerId);

            return Ok(applications);
        }
    }
}