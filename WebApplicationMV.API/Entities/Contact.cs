using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;

namespace WebApplicationMV.API.Entities
{
    public class Contact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContactId { get; set; }

        [Required]
        [MaxLength(50)]
        public string ContactName { get; set; }

        [ForeignKey("CompanyId")]
        public int CompanyId { get; set; }
        public Company? Company { get; set; }

        [ForeignKey("CountryId")]
        public int CountryId { get; set; }
        public Country? Country { get; set; }

        public Contact(string contactName, int companyId, int countryId)
        {
            ContactName = contactName;
            CompanyId = companyId;
            CountryId = countryId;
        }
    }
}
