using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VetServer.Models
{
    public class AddPatient
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string? Name { get; set; }

        [Required]
        [Range(1, 50)]
        public int Age { get; set; }

        [Required]
        [StringLength(1, MinimumLength = 1)]
        public string? Sex { get; set; }

        [Required]
        public int owner_id { get; set; }

        [Required]
        public int kind_id { get; set; }
    }
}
