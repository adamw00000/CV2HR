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
        private static readonly List<JobOffer> jobOffers = new List<JobOffer>()
        {
            new JobOffer(){Id = 1, JobTitle="Backend Developer", Salary=15000},
            new JobOffer(){Id = 2, JobTitle="Frontend Developer", Salary=5000},
            new JobOffer(){Id = 3, JobTitle="Manager", Salary=20000},
            new JobOffer(){Id = 4, JobTitle="Teacher", Salary=0},
            new JobOffer(){Id = 5, JobTitle="Cook", Salary=500}
        };

        [Route("JobOffer/Index")]
        public IActionResult Index()
        {
            return View(jobOffers);
        }

        [Route("JobOffer/Details")]
        public IActionResult Details(int id)
        {
            return View(jobOffers.FirstOrDefault(job => job.Id == id));
        }
    }
}