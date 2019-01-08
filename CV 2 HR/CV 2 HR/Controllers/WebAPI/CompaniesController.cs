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
    [Authorize(Policy = "Admin")]
    public class CompaniesController: ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        /// <summary>
        /// Adds a new company to the list
        /// </summary>
        /// <param name="newCompany">Name of the new company</param>
        /// <returns>A job offer page</returns>
        [HttpPost]
        public async Task<IActionResult> Add([FromBody]Company newCompany)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model is not valid!");

            bool succeeded = await _companyService.AddCompanyAsync(newCompany);

            if (!succeeded)
            {
                return StatusCode(500);
            }

            return Ok("Add successful!");
        }

        /// <summary>
        /// Gets a list of all companies
        /// </summary>
        /// <returns>A job offer page</returns>
        [HttpGet]
        public async Task<IActionResult> Companies()
        {
            var companies = await _companyService.GetCompaniesAsync();

            return Ok(companies);
        }

    }
}