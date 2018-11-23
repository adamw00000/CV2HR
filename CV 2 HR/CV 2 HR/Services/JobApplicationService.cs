using CV_2_HR.EF_Core;
using CV_2_HR.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CV_2_HR.Services
{
    public class JobApplicationService: IJobApplicationService
    {
        private ApplicationDbContext _context;

        public JobApplicationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddJobApplicationAsync(JobApplication jobApplication)
        {
            await _context.JobApplications.AddAsync(jobApplication);
            var modified = await _context.SaveChangesAsync();

            return modified == 1;
        }

        public async Task<JobApplication> GetJobApplicationAsync(int id)
        {
            return await _context.JobApplications
                .FirstOrDefaultAsync(application => application.Id == id);
        }

        public async Task<IEnumerable<JobApplication>> GetOfferApplicationsAsync(int id)
        {
            return await _context.JobApplications
                .Where(application => application.OfferId == id)
                .ToListAsync();
        }
    }
}
