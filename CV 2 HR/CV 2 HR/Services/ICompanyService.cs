using CV_2_HR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CV_2_HR.Services
{
    public interface ICompanyService
    {
        Task<IEnumerable<Company>> GetCompaniesAsync();
        Task<bool> AddCompanyAsync(Company newCompany);
        Task<bool> RemoveCompanyAsync(Company removedCompany);
    }
}
