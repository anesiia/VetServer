using System.ComponentModel.DataAnnotations;

namespace VetServer.Models
{
    public class OwnerLoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
