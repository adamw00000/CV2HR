using CV_2_HR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CV_2_HR.Services
{
    public interface IJobOfferService
    {
        Task<bool> AddJobOfferAsync(JobOffer jobOffer);
        Task<IEnumerable<JobOffer>> GetJobOffersAsync();
        Task<JobOffer> GetOfferAsync(int id);
        Task<JobOffer> GetOfferWithApplicationsAsync(int id);
        Task<bool> ModifyOffer(JobOffer newOffer);
        Task<bool> RemoveOffer(JobOffer newOffer);
    }
}
