using CV2HR.EF_Core;
using CV2HR.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CV2HR.Services
{
    public class CompanyService: ICompanyService
    {
        private readonly ApplicationDbContext _context;

        public CompanyService(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<IEnumerable<Company>> GetCompaniesAsync()
        {
            return await _context.Companies.ToListAsync();
        }

        public async Task<bool> AddCompanyAsync(Company newCompany)
        {
            await _context.Companies.AddAsync(newCompany);
            int modified = await _context.SaveChangesAsync();

            return modified == 1;
        }

        public async Task<bool> RemoveCompanyAsync(Company removedCompany)
        {
            _context.Companies.Remove(removedCompany);
            int modified = await _context.SaveChangesAsync();

            return modified == 1;
        }
    }
}
