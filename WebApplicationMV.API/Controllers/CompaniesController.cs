using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.ComponentModel.Design;
using System.Text.Json;
using WebApplicationMV.API.Entities;
using WebApplicationMV.API.Models;
using WebApplicationMV.API.Services;

namespace WebApplicationMV.API.Controllers
{
    [ApiController]
    [Route("api/companies")]
    public class CompaniesController : ControllerBase
    {
        private readonly ILogger<CompaniesController> _logger;
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public CompaniesController(ILogger<CompaniesController> logger, ICompanyRepository companyRepository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanies()
        {
            try
            {
                var companies = await _companyRepository.GetCompaniesAsync();

                return Ok(_mapper.Map<IEnumerable<CompanyDto>>(companies));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(
                    "Exeption while getting companies",
                    ex);

                return StatusCode(500,
                    "A problem occured while handeling your request");
            }
        }
        [Authorize]
        [HttpGet("secure")]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompaniesSecure()
        {
            try
            {
                var companies = await _companyRepository.GetCompaniesAsync();

                return Ok(_mapper.Map<IEnumerable<CompanyDto>>(companies));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(
                    "Exeption while getting companies",
                    ex);

                return StatusCode(500,
                    "A problem occured while handeling your request");
            }
        }

        [HttpGet("paged")]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompaniesPaged(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var (companies, pagination) = await _companyRepository
                    .GetCompaniesPagedAsync(pageNumber, pageSize);

                Response.Headers.Add("X-Pagination",
                    JsonSerializer.Serialize(pagination));

                return Ok(_mapper.Map<IEnumerable<CompanyDto>>(companies));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(
                    "Exeption while getting companies",
                    ex);

                return StatusCode(500,
                    "A problem occured while handeling your request");
            }
        }

        [HttpGet("companyId", Name = "GetCompany")]
        public async Task<ActionResult<CompanyDto>> GetCompany(int companyId)
        {
            try
            {
                if (!await _companyRepository.CompanyExistsAsync(companyId))
                {
                    _logger.LogInformation($"Company with company id {companyId} is not found");
                    return NotFound();
                }

                var companyToReturn = await _companyRepository.GetCompanyAsync(companyId);

                return Ok(_mapper.Map<CompanyDto>(companyToReturn));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(
                    $"Exeption while getting company with company id {companyId}",
                    ex);

                return StatusCode(500,
                    "A problem occured while handeling your request");
            }
        }

        [HttpPost]
        public async Task<ActionResult<CompanyDto>> CreateCompany(CompanyForInsertDto companyForInsertDto)
        {
            try
            {
                var company = _mapper.Map<Company>(companyForInsertDto);
                await _companyRepository.AddCompanyAsync(company);

                return CreatedAtRoute("GetCompany",
                    new { companyId = company.CompanyId },
                    companyForInsertDto);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(
                    "Exeption while creating company",
                    ex);

                return StatusCode(500,
                    "A problem occured while handeling your request");
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateCompany(CompanyDto companyDto)
        {
            try
            {
                var company = await _companyRepository.GetCompanyAsync(companyDto.CompanyId);
                if (company == null)
                {
                    _logger.LogInformation($"Company with comapny id {companyDto.CompanyId} is not found");
                    return NotFound();
                }

                _mapper.Map(companyDto, company);

                await _companyRepository.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(
                    "Exeption while updating company",
                    ex);

                return StatusCode(500,
                    "A problem occured while handeling your request");
            }
        }
    }
}
