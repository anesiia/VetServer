using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VetServer.Models
{
    public class DoctorRegistration
    {
        [Required]
        public string? Name { get; set; }

        [RegularExpression(@"^\d{12}$")]
        public decimal? Phone { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Address { get; set; }

        [Required]
        [MinLength(8)]
        [RegularExpression(@"^(?=.*\d).*$")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

    }
}
