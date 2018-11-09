using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CV_2_HR.Models;
using CV_2_HR.Services;
using Microsoft.AspNetCore.Mvc;

namespace CV_2_HR.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService service;

        public AdminController(IAdminService companyService)
        {
            service = companyService;
        }

        public async Task<IActionResult> Index()
        {
            var companies = await service.GetCompaniesAsync();

            return View(companies);
        }

        public async Task<IActionResult> AddCompany()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCompany(Company newCompany)
        {
            if (!ModelState.IsValid)
                return View(newCompany);

            bool succeeded = await service.AddCompanyAsync(newCompany);
            
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

            bool succeeded = await service.RemoveCompany(removedCompany);

            if (!succeeded)
            {
                return StatusCode(500);
            }

            return RedirectToAction("Index");
        }
    }
}