using CV2HR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CV2HR.Services
{
    public interface ICompanyService
    {
        Task<IEnumerable<Company>> GetCompaniesAsync();
        Task<bool> AddCompanyAsync(Company newCompany);
        Task<bool> RemoveCompanyAsync(Company removedCompany);
    }
}
