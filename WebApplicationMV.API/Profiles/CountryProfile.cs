using AutoMapper;

namespace WebApplicationMV.API.Profiles
{
    public class CountryProfile : Profile
    {
        public CountryProfile() 
        {
            CreateMap<Entities.Country, Models.CountryDto>();
            CreateMap<Models.CountryDto, Entities.Country>();
            CreateMap<Models.CountryForInsertDto, Entities.Country>();
        }
    }
}
