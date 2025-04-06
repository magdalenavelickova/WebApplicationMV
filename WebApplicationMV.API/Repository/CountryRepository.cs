using Microsoft.EntityFrameworkCore;
using WebApplicationMV.API.DbContexts;
using WebApplicationMV.API.Entities;

namespace WebApplicationMV.API.Services
{
    public class CountryRepository : ICountryRepository
    {
        private readonly CompanyInfoContext _context;
        public CountryRepository(CompanyInfoContext context) 
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<IEnumerable<Country>> GetCountriesAsync()
        {
            return await _context.Countries.OrderBy(c => c.CountryName).ToListAsync();
        }

        public async Task<(IEnumerable<Country>, Pagination)> GetCountriesPagedAsync(int pageNumber, int pageSize)
        {
            var countries = await _context.Countries.OrderBy(c => c.CountryName).ToListAsync();

            var totalItemCount = countries.Count();

            var pagination = new Pagination(
                totalItemCount, pageSize, pageNumber);

            var countriesToReturn = countries.OrderBy(c => c.CountryName)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToList();

            return (countriesToReturn, pagination);
        }

        public async Task<Country> GetCountryAsync(int countryId)
        {
            return await _context.Countries.Where(c => c.CountryId == countryId).FirstOrDefaultAsync();
        }

        public async Task<bool> CountryExistsAsync(int countryId)
        {
            return await _context.Countries.AnyAsync(c => c.CountryId == countryId);
        }

        public async Task AddCountryAsync(Country country)
        {
            _context.Countries.Add(country);
        }
        public void DeleteCountry(Country country)
        {
            _context.Countries.Remove(country);
        }

        public async Task<Dictionary<string, int>> GetCompanyStatisticsByCountryId(int countryId)
        {
            var country = await _context.Countries
                .FirstOrDefaultAsync(c => c.CountryId == countryId);

            if (country == null)
            {
                return null;
            }

            var companyStatistics = await _context.Contacts
                .Where(contact => contact.CountryId == countryId)
                .GroupBy(contact => contact.Company)
                .Select(group => new {
                    CompanyName = group.Key.CompanyName,
                    ContactCount = group.Count()
                })
                .ToListAsync();

            var result = companyStatistics.ToDictionary(
                item => item.CompanyName,
                item => item.ContactCount
            );

            return result;
        }
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
