using System.Threading.Tasks;
using WebApplicationMV.API.Entities;

namespace WebApplicationMV.API.Services
{
    public interface IContactRepository
    {
        Task<IEnumerable<Contact>> GetContactsAsync();
        Task<(IEnumerable<Contact>, Pagination)> GetContactsPagedAsync(int pageNumber, int pageSize);
        Task<Contact> GetContactAsync(int contactId);
        Task<bool> ContactExistsAsync(int contactId);
        Task<bool> CheckContactRelationsAsync(int contactId, int countryId);
        Task AddContactAsync(Contact contact);
        void DeleteContact(Contact contact);
        Task<List<Contact>> FilterContacts(int? countryId, int? companyId);
        Task<List<Contact>> GetContactsWithCompanyAndCountry();
        Task<bool> SaveChangesAsync();
    }
}
