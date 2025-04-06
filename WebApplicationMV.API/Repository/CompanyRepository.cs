using Microsoft.EntityFrameworkCore;
using WebApplicationMV.API.DbContexts;
using WebApplicationMV.API.Entities;

namespace WebApplicationMV.API.Services
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly CompanyInfoContext _context;
        public CompanyRepository(CompanyInfoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task AddCompanyAsync(Company company)
        {
            _context.Companies.Add(company);
            _context.SaveChanges();
        }

        public async Task<bool> CompanyExistsAsync(int companyId)
        {
            return await _context.Companies.AnyAsync(c => c.CompanyId == companyId);
        }

        public void DeleteCompany(Company company)
        {
            _context.Companies.Remove(company);
            _context.SaveChanges();
        }
        public async Task<IEnumerable<Company>> GetCompaniesAsync()
        {
            return await _context.Companies.OrderBy(c => c.CompanyName).ToListAsync();
        }

        public async Task<(IEnumerable<Company>, Pagination)> GetCompaniesPagedAsync(int pageNumber, int pageSize)
        {
            var companies  = await _context.Companies.OrderBy(c => c.CompanyName).ToListAsync();

            var numRecords = companies.Count();

            var pages = new Pagination(numRecords, pageNumber, pageSize);

            var companiesToReturn = companies.OrderBy(c => c.CompanyName)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToList();

            return (companiesToReturn, pages);
        }

        public async Task<Company> GetCompanyAsync(int companyId)
        {
            return await _context.Companies.Where(c => c.CompanyId == companyId).FirstOrDefaultAsync();
        }
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
