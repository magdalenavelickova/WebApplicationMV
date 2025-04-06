using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationMV.API.Entities
{
    public class Country
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CountryId { get; set; }

        [Required]
        [MaxLength(50)]
        public string CountryName { get; set; }
        public List<Contact> Contacts { get; set; } = new List<Contact>();

        public Country(string countryName) 
        { 
           CountryName = countryName;
        }
    }
}
