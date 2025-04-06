using System.ComponentModel.DataAnnotations;

namespace WebApplicationMV.API.Models
{
    public class CountryDto
    {
        public int CountryId { get; set; }
        public string? CountryName { get; set; }
    }
}
