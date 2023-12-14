using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VetServer.Models
{
    public class OwnerRegistration
    {
        [Required]
        public string? Name { get; set; }

        [RegularExpression(@"^\d{12}$")]
        public decimal? Phone { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [MinLength(8)]
        [RegularExpression(@"^(?=.*\d).*$")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

    }
}
