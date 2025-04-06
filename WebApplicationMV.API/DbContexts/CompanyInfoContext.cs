using Microsoft.EntityFrameworkCore;
using WebApplicationMV.API.Entities;

namespace WebApplicationMV.API.DbContexts
{
    public class CompanyInfoContext : DbContext
    {
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Country> Countries { get; set; }

        public CompanyInfoContext(DbContextOptions<CompanyInfoContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>()
                .HasData(
                new Company("Aspekt")
                {
                    CompanyId = 1
                }
                );

            modelBuilder.Entity<Country>()
               .HasData(
               new Country("Macedonia")
               {
                   CountryId = 1
               }
               );

            modelBuilder.Entity<Contact>()
               .HasData(
               new Contact("Magdalena", 1, 1)
               {
                   ContactId = 1
               }
               );

            base.OnModelCreating(modelBuilder);
        }
    }
}
 