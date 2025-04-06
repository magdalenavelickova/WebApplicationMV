using WebApplicationMV.API.Entities;

namespace WebApplicationMV.API.Services
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<Company>> GetCompaniesAsync();
        Task<(IEnumerable<Company>, Pagination)> GetCompaniesPagedAsync(int pageNumber, int pageSize);
        Task<Company> GetCompanyAsync(int companyId);
        Task<bool> CompanyExistsAsync(int companyId);
        Task AddCompanyAsync(Company company);
        void DeleteCompany(Company company);
        Task<bool> SaveChangesAsync();
    }
}
