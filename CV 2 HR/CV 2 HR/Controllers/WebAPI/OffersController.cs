using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CV_2_HR.Models;
using CV_2_HR.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CV_2_HR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OffersController : ControllerBase
    {
        private readonly IJobOfferService _offerService;

        public OffersController(IJobOfferService offerService)
        {
            _offerService = offerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobOffer>>> Offers()
        {
            var offers = await _offerService.GetJobOffersAsync();
            return Ok(offers);
        }

        [HttpGet("{searchString}")]
        public async Task<ActionResult<IEnumerable<JobOffer>>> Offers(string searchString)
        {
            var offers = await _offerService.GetJobOffersSearchResultAsync(searchString);
            return Ok(offers);
        }
    }
}