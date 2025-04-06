using AutoMapper;
using WebApplicationMV.API.Entities;
using WebApplicationMV.API.Models;

namespace WebApplicationMV.API.Profiles
{
    public class ContactProfile : Profile
    {
        public ContactProfile() 
        {
            CreateMap<Contact, ContactDto>();
            CreateMap<ContactDto,Contact>();
            CreateMap<ContactForInsertDto, Contact>();

            CreateMap<Contact, ContactDetailsDto>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company != null ? src.Company.CompanyName : null))
                .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country != null ? src.Country.CountryName : null));
        }
    }
}
