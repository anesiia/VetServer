using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VetServer.Models
{
    public class Patients
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

        [ForeignKey("owner_id")]
        public int owner_id { get; set; }

        [ForeignKey("KindId")]
        public int KindId { get; set; }

    }
}
