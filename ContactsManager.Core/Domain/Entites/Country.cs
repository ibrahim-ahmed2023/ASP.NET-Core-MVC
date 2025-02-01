using System.ComponentModel.DataAnnotations;

namespace Entities
{
    /// <summary>
    /// Domain Model For storing the country details
    /// </summary>
    public class Country
    {
        [Key]
        public Guid CountryId { get; set; }
        public string? CountryName { get; set; }
        public ICollection<Person>? persons { get; set; }

    }
}
