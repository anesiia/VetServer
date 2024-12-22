using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VetServer.Models
{
    public class OwnersPatients
    {
        [Key]
        [Column("patient_id")]
        public int PatientId { get; set; }

        [Column("patient_name")]
        public string PatientName { get; set; }

        [Column("patient_age")]
        public int PatientAge { get; set; }

        [Column("patient_sex")]
        public string PatientSex { get; set; }

        [Column("kind_name")]
        public string kind_name { get; set; }

    }
}
