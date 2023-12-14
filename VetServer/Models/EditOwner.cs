using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VetServer.Models
{
    public class EditOwner
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public decimal? Phone { get; set; }

        public string? Email { get; set; }
    }
}
