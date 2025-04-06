using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationMV.API.Entities
{
    public class Company
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyId { get; set; }

        [Required]
        [MaxLength(50)]
        public string CompanyName { get; set; }
        public List<Contact> Contacts { get; set; } = new List<Contact>();

        public Company(string companyName) 
        {
            CompanyName = companyName;
        }
    }
}
