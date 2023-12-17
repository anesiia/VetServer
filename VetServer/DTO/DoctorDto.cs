using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VetServer.DTO
{
    public class DoctorDto
    {
        public int DoctorId { get; set; }
        public string? DoctorName { get; set; }

        public decimal? DoctorPhone { get; set; }

        public string? DoctorEmail { get; set; }

        public string? DoctorAddress { get; set; }

        public string? DoctorPassHash { get; set; }

        public bool? IsAdmin { get; set; }

    }
}
