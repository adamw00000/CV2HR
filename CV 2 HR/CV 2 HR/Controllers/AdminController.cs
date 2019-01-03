using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CV2HR.Models;
using CV2HR.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CV2HR.Controllers
{
    [Authorize(Policy = "Admin")]
    public class AdminController : Controller
    {
        private readonly ICompanyService _companyService;

        public AdminController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        public async Task<IActionResult> Index()
        {
            var companies = await _companyService.GetCompaniesAsync();

            return View(companies);
        }

        public IActionResult AddCompany()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCompany(Company newCompany)
        {
            if (!ModelState.IsValid)
                return View(newCompany);

            bool succeeded = await _companyService.AddCompanyAsync(newCompany);
            
            if (!succeeded)
            {
                return StatusCode(500);
            }

            return RedirectToAction("Index");
        }
        
        public async Task<IActionResult> RemoveCompany(Company removedCompany)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index");

            bool succeeded = await _companyService.RemoveCompanyAsync(removedCompany);

            if (!succeeded)
            {
                return StatusCode(500);
            }

            return RedirectToAction("Index");
        }
    }
}