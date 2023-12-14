using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VetServer.Models.Database
{
    public class Doctors
    {

        [Key]
        [Column("doctor_id")]
        public int DoctorId { get; set; }

        [Column("doctor_name")]
        public string? DoctorName { get; set; }

        [Phone]
        [Column("doctor_phone")]
        public decimal? DoctorPhone { get; set; }

        [Column("doctor_email")]
        [EmailAddress]
        public string? DoctorEmail { get; set; }

        [Column("doctor_address")]
        public string? DoctorAddress { get; set; }

        [Column("doctor_pass_hash")]
        public string? DoctorPassHash { get; set; }

        [Column("is_admin")]
        public bool? IsAdmin { get; set; }
    }

}
