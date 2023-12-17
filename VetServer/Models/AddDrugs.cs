using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VetServer.Models
{
    public class AddDrugs
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string? Name { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
