using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ContactsManager.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser , ApplicationRole , Guid>
    {
        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<Country> countries { get; set; }


        public ApplicationDbContext(DbContextOptions options):base(options)
        {
            
        }

        [Obsolete]
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().ToTable("countries");
            modelBuilder.Entity<Person>().ToTable("Persons");

            //seed data
            var countriesJson = File.ReadAllText("countries.json");
            var countries = JsonSerializer.Deserialize<List<Country>>(countriesJson);
            foreach (var country in countries!)
            {
                modelBuilder.Entity<Country>().HasData(country);
            }     
            
            var personsJson = File.ReadAllText("persons.json");
            var persons = JsonSerializer.Deserialize<List<Person>>(personsJson);
            foreach (var person in persons!)
            {
                modelBuilder.Entity<Person>().HasData(person);
            }

            //Fluent API
            modelBuilder.Entity<Person>().Property(temp => temp.TIN)
              .HasColumnName("TaxIdentificationNumber")
              .HasColumnType("varchar(8)")
              .HasDefaultValue("ABC12345");

            modelBuilder.Entity<Person>().ToTable(b=>b
                .HasCheckConstraint("CHK_TIN", "len([TaxIdentificationNumber]) = 8"));

        }

        public List<Person> sp_GetAllPersons()
        {
           return Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]").ToList();
        }

        public int sp_InsertPerson(Person person)
        {
            SqlParameter[] parameters = [
            new SqlParameter("@PersonID", person.PersonID),
            new SqlParameter("@PersonName", person.PersonName),
            new SqlParameter("@Email", person.Email),
            new SqlParameter("@DateOfBirth", person.DateOfBirth),
            new SqlParameter("@Gender", person.Gender),
            new SqlParameter("@CountryID", person.CountryID),
            new SqlParameter("@Address", person.Address),
            new SqlParameter("@ReceiveNewsLetters", person.ReceiveNewsLetters)
          ];

            return Database.ExecuteSqlRaw("EXECUTE [dbo].[InsertPerson] @PersonID, @PersonName, @Email, @DateOfBirth, @Gender, @CountryID, @Address, @ReceiveNewsLetters", parameters);
        }
    }
}
