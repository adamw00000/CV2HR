using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CV_2_HR.Models;
using Microsoft.AspNetCore.Mvc;

namespace CV_2_HR.Controllers
{
    public class JobOfferController : Controller
    {
        private static readonly List<Company> companies = new List<Company>
        {
            new Company(){Id = 1, Name = "Predica"},
            new Company(){Id = 2, Name = "Microsoft"},
            new Company(){Id = 3, Name = "GitHub"}
        };

        private static readonly List<JobOffer> jobOffers = new List<JobOffer>
        {
            new JobOffer()
            {
                Id = 1,
                JobTitle = "Backend Developer",
                Company = companies.FirstOrDefault(c => c.Name == "Predica"),
                Created = DateTime.Now.AddDays(-2),
                Description = "aaa bbbbb cccccccc dddddddddd aaaaaaaa bbbbb ccccc ddd aaa bbbbb cccccccc dddddddddd aaaaaaaa bbbbb ccccc ddd aaa bbbbb cccccccc dddddddddd aaaaaaaa bbbbb ccccc ddd aaa bbbbb cccccccc dddddddddd aaaaaaaa bbbbb ccccc ddd",
                Location = "Poland",
                SalaryFrom = 15000,
                SalaryTo = 20000,
                ValidUntil = DateTime.Now.AddDays(10)
            },
            new JobOffer()
            {
                Id = 2,
                JobTitle = "Frontend Developer",
                Company = companies.FirstOrDefault(c => c.Name == "Microsoft"),
                Created = DateTime.Now.AddDays(-7),
                Description = "bbbbbbbbbbbbbbbbbbb aaaaaaaa bbbbb ccccc ddd aaa bbbbb cccccccc xxxxxxxxxxxxxxxxxxxxxxxxxxxxx cccccccc dddddddddd bbbbbbbbbbbbb",
                Location = "Poland",
                SalaryFrom = 5000,
                SalaryTo = 7000,
                ValidUntil = DateTime.Now.AddDays(3)
            }
        };

        public IActionResult Index()
        {
            return View(jobOffers);
        }
        
        public IActionResult Details(int id)
        {
            return View(jobOffers.FirstOrDefault(job => job.Id == id));
        }
        
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return BadRequest();

            JobOffer jobOffer = jobOffers.FirstOrDefault(offer => offer.Id == id);
            if (jobOffer == null)
                return NotFound();

            return View(jobOffer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(JobOffer newOffer)
        {
            if (!ModelState.IsValid)
                return View();

            var oldOffer = jobOffers.FirstOrDefault(offer => offer.Id == newOffer.Id);
            oldOffer.JobTitle = newOffer.JobTitle;
            oldOffer.Description = newOffer.Description;
            oldOffer.SalaryFrom = newOffer.SalaryFrom;
            oldOffer.SalaryTo = newOffer.SalaryTo;
            return RedirectToAction("Details", new { id = oldOffer.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();

            var jobOffer = jobOffers.FirstOrDefault(offer => offer.Id == id);
            if (jobOffer != null)
            {
                jobOffers.Remove(jobOffer);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            var model = new JobOfferCreateViewModel() { Companies = companies };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JobOfferCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.Id = jobOffers.Max(offer => offer.Id) + 1;
            model.Created = DateTime.Now;
            model.Company = companies.FirstOrDefault(company => company.Id == model.CompanyId);

            jobOffers.Add(model as JobOffer);

            return RedirectToAction("Index");
        }
        
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery(Name = "search")]string query)
        {
            if (string.IsNullOrEmpty(query))
                return View(jobOffers);

            var queryResult = jobOffers.FindAll(offer => offer.JobTitle.ToLower().Contains(query.ToLower()));

            return View(queryResult);
        }
    }
}