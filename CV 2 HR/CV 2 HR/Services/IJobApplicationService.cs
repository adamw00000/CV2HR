using CV2HR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CV2HR.Services
{
    public interface IJobApplicationService
    {
        Task<bool> AddJobApplicationAsync(JobApplication jobApplication);
        Task<JobApplication> GetJobApplicationAsync(int id);
        Task<IEnumerable<JobApplication>> GetOfferApplicationsAsync(int id);
    }
}
