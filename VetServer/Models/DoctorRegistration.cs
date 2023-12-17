using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VetServer.Models
{
    public class DoctorRegistration
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string? Name { get; set; }

        [RegularExpression(@"^\d{12}$")]
        public decimal? Phone { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string? Address { get; set; }

        [Required]
        [MinLength(8)]
        [RegularExpression(@"^(?=.*\d).*$")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

    }
}
