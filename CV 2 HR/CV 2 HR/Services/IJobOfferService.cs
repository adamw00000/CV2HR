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
        Task<bool> UpdateOffer(JobOffer newOffer);
        Task<bool> RemoveOffer(JobOffer newOffer);
        Task<JobOfferPage> GetJobOffersSearchResultPageAsync(string searchstring, int pageNo);
        Task<JobOfferPage> GetJobOffersPageAsync(int pageNo);
    }
}
