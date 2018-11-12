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

        public async Task<bool> AddJobApplicationAsync(JobApplication jobApplication)
        {
            await _context.JobApplications.AddAsync(jobApplication);
            var modified = await _context.SaveChangesAsync();

            return modified == 1;
        }

        public async Task<bool> AddJobOfferAsync(JobOffer jobOffer)
        {
            jobOffer.Created = DateTime.Now;
            jobOffer.Company = _context.Companies.FirstOrDefault(company => company.Id == jobOffer.CompanyId);

            await _context.JobOffers.AddAsync(jobOffer);
            var modified = await _context.SaveChangesAsync();

            return modified == 1;
        }

        public async Task<JobApplication> GetJobApplicationAsync(int id)
        {
            return await _context.JobApplications
                .FirstOrDefaultAsync(application => application.Id == id);
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
                .FirstOrDefaultAsync(offer => offer.Id == id);
        }

        public async Task<JobOffer> GetOfferWithApplicationsAsync(int id)
        {
            return await _context.JobOffers
                .Include(offer => offer.JobApplications)
                .FirstOrDefaultAsync(offer => offer.Id == id);
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
    }
}
