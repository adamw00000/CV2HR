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
        /// <param name="searchString">Search string to match</param>
        /// <param name="pageNo">Page number, if 0, returns a list of all offers</param>
        /// <returns>A list of job offers</returns>
        [HttpGet]
        public async Task<IActionResult> Offers([FromQuery]string searchString = "", [FromQuery]int pageNo = 0)
        {
            if (pageNo == 0)
            {
                var allOffers = await _offerService.GetJobOffersAsync();
                return Ok(allOffers);
            }

            if (searchString == "")
            {
                var page = await _offerService.GetJobOffersPageAsync(pageNo);
                return Ok(page);
            }

            var searchPage = await _offerService.GetJobOffersSearchResultPageAsync(searchString, pageNo);
            return Ok(searchPage);
        }
    }
}