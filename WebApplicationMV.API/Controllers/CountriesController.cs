using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Diagnostics.Metrics;
using System.Text.Json;
using System.Threading.Tasks;
using WebApplicationMV.API.Entities;
using WebApplicationMV.API.Models;
using WebApplicationMV.API.Services;

namespace WebApplicationMV.API.Controllers
{
    [ApiController]
    [Route("api/countries")]
    public class CountriesController : ControllerBase
    {
        private readonly ILogger<CountriesController> _logger;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CountriesController(ILogger<CountriesController> logger, ICountryRepository countryRepository, IMapper mapper) 
        {
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _countryRepository = countryRepository ?? throw new ArgumentNullException(nameof(countryRepository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountryDto>>> GetCountries()
        {
            try
            {
                var countries = await _countryRepository.GetCountriesAsync();

                return Ok(_mapper.Map<IEnumerable<CountryDto>>(countries));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(
                    "Exeption while getting countries",
                    ex);

                return StatusCode(500,
                    "A problem occured while handeling your request");
            }
        }

        [HttpGet("paged")]
        public async Task<ActionResult<IEnumerable<CountryDto>>> GetCountriesPaged(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var (countriesToReturn, pagination) = await _countryRepository
                    .GetCountriesPagedAsync(pageNumber, pageSize);

                Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(pagination));

                return Ok(_mapper.Map<IEnumerable<CountryDto>>(countriesToReturn));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(
                    "Exeption while getting countries",
                    ex);

                return StatusCode(500,
                    "A problem occured while handeling your request");
            }
        }

        [HttpGet("countryId", Name = "GetCountry")]
        public async Task<ActionResult<CountryDto>> GetCountry(int countryId)
        {
            try
            {
                if (!await _countryRepository.CountryExistsAsync(countryId))
                {
                    _logger.LogInformation($"Country with country id {countryId} is not found");
                    return NotFound($"Country not found");
                }

                var countryToReturn = await _countryRepository.GetCountryAsync(countryId);

                return Ok(_mapper.Map<CountryDto>(countryToReturn));
            }
            catch(Exception ex)
            {
                _logger.LogCritical(
                    $"Exeption while getting country with country id {countryId}", 
                    ex);

                return StatusCode(500, 
                    "A problem occured while handeling your request");
            }
        }

        [HttpGet("{countryId}/company-statistics")]
        public async Task<ActionResult<Dictionary<string, int>>> GetCompanyStatistics(int countryId)
        {
            var statistics = await _countryRepository.GetCompanyStatisticsByCountryId(countryId);

            if (statistics == null)
            {
                return NotFound();
            }

            return Ok(statistics);
        }

        [HttpPost]
        public async Task<ActionResult<CountryDto>> CreateCountry(CountryForInsertDto countryToInsertDto)
        {
            try
            {
                var country = _mapper.Map<Country>(countryToInsertDto);
                await _countryRepository.AddCountryAsync(country);

                await _countryRepository.SaveChangesAsync();

                return CreatedAtRoute("GetCountry", 
                    new { id = country.CountryId },
                    countryToInsertDto);
            } 
            catch (Exception ex)
            {
                _logger.LogCritical(
                    "Exeption while creating country",
                    ex);

                return StatusCode(500,
                    "A problem occured while handeling your request");
            }
        } 

        [HttpPut("countryId", Name = "GetCountry")]
        public async Task<ActionResult> UpdateCountry(CountryDto countryDto)
        {
            try
            {
                var country = await _countryRepository.GetCountryAsync(countryDto.CountryId);
                if (country == null)
                {
                    _logger.LogInformation($"Country with country id {countryDto.CountryId} is not found");
                    return NotFound("Country not found");
                }

                _mapper.Map(countryDto, country);
                await _countryRepository.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(
                    "Exeption while updating country",
                    ex);

                return StatusCode(500,
                    "A problem occured while handeling your request");
            }
        }

        [HttpDelete("countryId")]
        public async Task<ActionResult> DeleteCountry(int countryId)
        {
            try
            {
                if (!await _countryRepository.CountryExistsAsync(countryId))
                {
                    _logger.LogInformation($"Country with country id {countryId} is not found");
                    return NotFound();
                }

                var country = await _countryRepository.GetCountryAsync(countryId);

                _countryRepository.DeleteCountry(country);

                await _countryRepository.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(
                    "Exeption while removing country",
                    ex);

                return StatusCode(500,
                    "A problem occured while handeling your request");
            }
        }
    }
}
