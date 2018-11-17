using CV_2_HR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CV_2_HR.Services
{
    public interface IJobApplicationService
    {
        Task<bool> AddJobApplicationAsync(JobApplication jobApplication);
        Task<JobApplication> GetJobApplicationAsync(int id);
    }
}
