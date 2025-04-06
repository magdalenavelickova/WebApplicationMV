using AutoMapper;

namespace WebApplicationMV.API.Profiles
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile() 
        {
            CreateMap<Entities.Company, Models.CompanyDto>();
            CreateMap<Models.CompanyDto, Entities.Company>();
            CreateMap<Models.CompanyForInsertDto, Entities.Company>();
        }
    }
}
