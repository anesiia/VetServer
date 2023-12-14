using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VetServer.DTO
{
    public class OwnerDto
    {
        public int OwnerId { get; set; }

        public string? OwnerName { get; set; }

        public string? OwnerPhone { get; set; }

        public string? OwnerEmail { get; set; }

        public string? OwnerPassHash { get; set; }
    }
}
