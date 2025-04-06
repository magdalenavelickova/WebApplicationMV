using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Text.Json;
using WebApplicationMV.API.Entities;
using WebApplicationMV.API.Models;
using WebApplicationMV.API.Services;

namespace WebApplicationMV.API.Controllers
{
    [ApiController]
    [Route("api/contacts")]
    public class ContactsController : ControllerBase
    {
        private readonly ILogger<ContactsController> _logger;
        private readonly IContactRepository _contactRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public ContactsController(ILogger<ContactsController> logger,
            IContactRepository contactRepository,
            ICountryRepository countryRepository,
            ICompanyRepository companyRepository,
            IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _contactRepository = contactRepository ?? throw new ArgumentNullException(nameof(contactRepository));
            _countryRepository = countryRepository ?? throw new ArgumentNullException(nameof(countryRepository));
            _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactDto>>> GetContacts()
        {
            try
            {
                var contacts = await _contactRepository.GetContactsAsync();

                return Ok(_mapper.Map<IEnumerable<ContactDto>>(contacts));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(
                    "Exeption while getting contacts",
                    ex);

                return StatusCode(500,
                    "A problem occured while handeling your request");
            }
        }

        [HttpGet("paged")]
        public async Task<ActionResult<IEnumerable<ContactDto>>> GetContactsPaged(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var (contacts, pagination) = await _contactRepository
                    .GetContactsPagedAsync(pageNumber, pageSize);

                Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(pagination));

                return Ok(_mapper.Map<IEnumerable<ContactDto>>(contacts));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(
                    "Exeption while getting contacts",
                    ex);

                return StatusCode(500,
                    "A problem occured while handeling your request");
            }
        }

        [HttpGet("contactId", Name = "GetContact")]
        public async Task<ActionResult<ContactDto>> GetContact(int contactId)
        {
            try
            {
                if (!await _contactRepository.ContactExistsAsync(contactId))
                {
                    _logger.LogInformation($"Contact with contact id {contactId} is not found");
                    return NotFound();
                }

                var contactToReturn = await _contactRepository.GetContactAsync(contactId);

                return Ok(_mapper.Map<ContactDto>(contactToReturn));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(
                    $"Exeption while getting contact with contactid id {contactId}",
                    ex);

                return StatusCode(500,
                    "A problem occured while handeling your request");
            }
        }

        [HttpGet("filter")]
        public async Task<IActionResult> FilterContacts([FromQuery] int? countryId, [FromQuery] int? companyId)
        {
            var contacts = await _contactRepository.FilterContacts(countryId, companyId);
            var contactToReturn = _mapper.Map<List<ContactDetailsDto>>(contacts);

            return Ok(contactToReturn);
        }

        [HttpGet("details")]
        public async Task<ActionResult<List<ContactDetailsDto>>> GetContactsWithDetails()
        {
            var contacts = await _contactRepository.GetContactsWithCompanyAndCountry();
            var contactToReturn = _mapper.Map<IEnumerable<ContactDetailsDto>>(contacts);

            return Ok(contactToReturn);
        }

        [HttpPost]
        public async Task<ActionResult<ContactDto>> CreateContact(ContactForInsertDto contactForInsert)
        {
            try
            {
                if (!await _countryRepository.CountryExistsAsync(contactForInsert.Countryid))
                {
                    return NotFound("Country doesnt exist");
                }

                if (!await _companyRepository.CompanyExistsAsync(contactForInsert.CompanyId))
                {
                    return NotFound("Company doesnt exist");
                }

                var contact = _mapper.Map<Contact>(contactForInsert);
                await _contactRepository.AddContactAsync(contact);

                await _contactRepository.SaveChangesAsync();

                return CreatedAtRoute("GetContact", contact.ContactId, contactForInsert);
            }
            catch (Exception ex)
            {
                Log.Logger.Fatal(
                    "Exeption while creating",
                    ex);

                return StatusCode(500,
                    "A problem occured while handeling your request");
            }
        }

        [HttpPut("contactId")]
        public async Task<ActionResult> UpdateContact(ContactDto contactDto)
        {
            try
            {
                if (!await _contactRepository.CheckContactRelationsAsync(contactDto.ContactId, contactDto.Countryid)) 
                {
                    return NotFound();
                }

                var contact = await _contactRepository.GetContactAsync(contactDto.ContactId);

                _mapper.Map(contactDto, contact);
                await _contactRepository.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Logger.Fatal(
                    "Exeption while updating contact",
                    ex);

                return StatusCode(500,
                    "A problem occured while handeling your request");
            }
        }


        [HttpDelete("contactId")]
        public async Task<ActionResult> DeleteContact(int contactId)
        {
            try
            {
                if (!await _contactRepository.ContactExistsAsync(contactId))
                {
                    Log.Logger.Information($"Contact with contact id {contactId} is not found");
                    return NotFound();
                }

                var contact = await _contactRepository.GetContactAsync(contactId);

                _contactRepository.DeleteContact(contact);

                await _contactRepository.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Logger.Fatal(
                    "Exeption while removing contact",
                    ex);

                return StatusCode(500,
                    "A problem occured while handeling your request");
            }
        }
    }
}
