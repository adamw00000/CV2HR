using CV_2_HR.EF_Core;
using CV_2_HR.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CV_2_HR.Services
{
    public class JobOfferService : IJobOfferService
    {
        private ApplicationDbContext _context;

        public JobOfferService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddJobOfferAsync(JobOffer jobOffer)
        {
            jobOffer.Created = DateTime.Now;
            jobOffer.Company = _context.Companies.FirstOrDefault(company => company.Id == jobOffer.CompanyId);

            await _context.JobOffers.AddAsync(jobOffer);
            var modified = await _context.SaveChangesAsync();

            return modified == 1;
        }

        public async Task<IEnumerable<JobOffer>> GetJobOffersAsync()
        {
            return await _context.JobOffers
                .Include(offer => offer.Company)
                .ToListAsync();
        }

        public async Task<JobOffer> GetOfferAsync(int id)
        {
            return await _context.JobOffers
                .Where(offer => offer.Id == id)
                .Include(offer => offer.Company)
                .FirstOrDefaultAsync();
        }

        public async Task<JobOffer> GetOfferWithApplicationsAsync(int id)
        {
            return await _context.JobOffers
                .Where(offer => offer.Id == id)
                .Include(offer => offer.Company)
                .Include(offer => offer.JobApplications)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ModifyOffer(JobOffer newOffer)
        {
            var oldOffer = await GetOfferAsync(newOffer.Id);

            oldOffer.JobTitle = newOffer.JobTitle;
            oldOffer.Description = newOffer.Description;
            oldOffer.SalaryFrom = newOffer.SalaryFrom;
            oldOffer.SalaryTo = newOffer.SalaryTo;

            var modified = await _context.SaveChangesAsync();
            return modified == 1;
        }

        public async Task<bool> RemoveOffer(JobOffer newOffer)
        {
            _context.JobOffers.Remove(newOffer);
            
            var modified = await _context.SaveChangesAsync();
            return modified == 1;
        }

        public async Task<JobOfferPage> GetJobOffersSearchResultPageAsync(string searchstring, int pageNo)
        {
            int pages, records, pageSize;
            pageSize = 1;

            records = _context.JobOffers
                .Where(offer => offer.JobTitle.ToLower().Contains(searchstring.ToLower()))
                .Count();
            pages = (records / pageSize) + ((records % pageSize) > 0 ? 1 : 0);

            var offers = await _context.JobOffers
                .Where(offer => offer.JobTitle.ToLower().Contains(searchstring.ToLower()))
                .OrderBy(offer => offer.Created)
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .Include(offer => offer.Company)
                .ToListAsync();

            JobOfferPage page = new JobOfferPage
            {
                JobOffers = offers,
                Pages = pages
            };

            return page;
        }

        public async Task<JobOfferPage> GetJobOffersPageAsync(int pageNo)
        {
            int pages, records, pageSize;
            pageSize = 1;

            records = _context.JobOffers.Count();
            pages = (records / pageSize) + ((records % pageSize) > 0 ? 1 : 0);
            var offers = await _context.JobOffers
                .OrderBy(offer => offer.Created)
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .Include(offer => offer.Company)
                .ToListAsync();

            JobOfferPage page = new JobOfferPage
            {
                JobOffers = offers,
                Pages = pages
            };

            return page;
        }
    }
}
