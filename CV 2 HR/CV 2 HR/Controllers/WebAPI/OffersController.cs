using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CV2HR.Models;
using CV2HR.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CV2HR.Controllers.Api
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

        /// <summary>
        /// Gets a list of all job offers
        /// </summary>
        /// <returns>A list of job offers</returns>
        [HttpGet]
        public async Task<IActionResult> Offers()
        {
            var offers = await _offerService.GetJobOffersAsync();
            return Ok(offers);
        }

        /// <summary>
        /// Gets a portion of job offers matching the search string, paged
        /// </summary>
        /// <param name="searchString">Search string to match</param>
        /// <param name="pageNo">Page number</param>
        /// <returns>A job offer page</returns>
        [HttpGet("{searchString}/{pageNo}")]
        public async Task<IActionResult> Offers(string searchString, int pageNo)
        {
            var offers = await _offerService.GetJobOffersSearchResultPageAsync(searchString, pageNo);
            return Ok(offers);
        }

        /// <summary>
        /// Gets a portion of all job offers, paged
        /// </summary>
        /// <param name="pageNo">Page number</param>
        /// <returns>A job offer page</returns>
        [HttpGet("{pageNo}")]
        public async Task<IActionResult> Offers(int pageNo)
        {
            var page = await _offerService.GetJobOffersPageAsync(pageNo);
            return Ok(page);
        }
    }
}