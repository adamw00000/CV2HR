using CV_2_HR.EF_Core;
using CV_2_HR.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CV_2_HR.Services
{
    public class AdminService: IAdminService
    {
        private readonly ApplicationDbContext context;

        public AdminService(ApplicationDbContext dbContext)
        {
            context = dbContext;
        }

        public async Task<IEnumerable<Company>> GetCompaniesAsync()
        {
            return await context.Companies.ToListAsync();
        }

        public async Task<bool> AddCompanyAsync(Company newCompany)
        {
            await context.Companies.AddAsync(newCompany);
            int modified = await context.SaveChangesAsync();

            return modified == 1;
        }

        public async Task<bool> RemoveCompany(Company removedCompany)
        {
            context.Companies.Remove(removedCompany);
            int modified = await context.SaveChangesAsync();

            return modified == 1;
        }
    }
}
