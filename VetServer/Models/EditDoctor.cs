using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VetServer.Models
{
    public class EditDoctor
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public bool? IsAdmin { get; set; }
    }
}
