using WebApplicationMV.API.Entities;

namespace WebApplicationMV.API.Services
{
    public interface ICountryRepository
    {
        Task<IEnumerable<Country>> GetCountriesAsync();
        Task<(IEnumerable<Country>, Pagination)> GetCountriesPagedAsync(int pageNumber, int pageSize);
        Task<Country> GetCountryAsync(int countryId);
        Task<bool> CountryExistsAsync(int countryId);
        Task AddCountryAsync(Country country);
        void DeleteCountry(Country country);
        Task<Dictionary<string, int>> GetCompanyStatisticsByCountryId(int countryId);
        Task<bool> SaveChangesAsync();
    }
}
