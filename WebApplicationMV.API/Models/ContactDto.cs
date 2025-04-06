using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;

namespace WebApplicationMV.API.Models
{
    public class ContactDto
    {
        public int ContactId { get; set; }
        public string? ContactName { get; set; }
        public int CompanyId { get; set; }
        public int Countryid { get; set; }
    }
}
