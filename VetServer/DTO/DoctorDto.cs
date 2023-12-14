using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VetServer.DTO
{
    public class DoctorDto
    {
       /* [Required]
        [StringLength(30, MinimumLength = 2)]
        public string Lastname { set; get; } = string.Empty;*/
        public int DoctorId { get; set; }
        public string? DoctorName { get; set; }

        public decimal? DoctorPhone { get; set; }

        public string? DoctorEmail { get; set; }

        public string? DoctorAddress { get; set; }

        public string? DoctorPassHash { get; set; }

        public bool? IsAdmin { get; set; }

    }
}
