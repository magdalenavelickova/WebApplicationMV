using Microsoft.EntityFrameworkCore;
using WebApplicationMV.API.DbContexts;
using WebApplicationMV.API.Entities;
using WebApplicationMV.API.Models;

namespace WebApplicationMV.API.Services
{
    public class ContactRepository : IContactRepository
    {
        private readonly CompanyInfoContext _context;

        public ContactRepository(CompanyInfoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task AddContactAsync(Contact contact)
        {
            _context.Add(contact);
        }

        public async Task<bool> ContactExistsAsync(int contactId)
        {
            return await _context.Contacts.AnyAsync(c => c.ContactId == contactId);
        }

        public async Task<bool> CheckContactRelationsAsync(int contactId, int countryId)
        {
            return await _context.Contacts.AnyAsync(c => c.ContactId == contactId && c.CountryId == countryId);
        }

        public void DeleteContact(Contact contact)
        {
            _context.Contacts.Remove(contact);
        }

        public async Task<Contact> GetContactAsync(int contactId)
        {
            return await _context.Contacts.Where(c => c.ContactId == contactId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Contact>> GetContactsAsync()
        {
            return await _context.Contacts.OrderBy(c => c.ContactName).ToListAsync();
        }
        public async Task<(IEnumerable<Contact>, Pagination)> GetContactsPagedAsync(int pageNumber, int pageSize)
        {
            var contacts = await _context.Contacts.OrderBy(c => c.ContactName).ToListAsync();

            var numRecords = contacts.Count();

            var pages = new Pagination(numRecords, pageNumber, pageSize);

            var companiesToReturn = contacts.OrderBy(c => c.ContactName)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToList();

            return (companiesToReturn, pages);
        }

        public async Task<List<Contact>> FilterContacts(int? countryId, int? companyId)
        {
            IQueryable<Contact> query = _context.Contacts
                .Include(c => c.Company)
                .Include(c => c.Country);

            if (countryId.HasValue)
            {
                query = query.Where(c => c.CountryId == countryId.Value);
            }

            if (companyId.HasValue)
            {
                query = query.Where(c => c.CompanyId == companyId.Value);
            }

            return await query.ToListAsync();
        }
        public async Task<List<Contact>> GetContactsWithCompanyAndCountry()
        {
            var contacts = await _context.Contacts
                .Include(c => c.Company)
                .Include(c => c.Country)
                .ToListAsync();

            return contacts;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
