using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VetServer.Models.Database
{
    public class AppontmentsDrugs
    {
        [Key]
        [Column("appontmentdrug_id")]
        public int AppontmentDrugId { get; set; }

        [Column("info")]
        public string Info { get; set; }

        [ForeignKey("drug_id")]
        public int DrugId { get; set; }

        [ForeignKey("appointment_id")]
        public int AppointmentId { get; set; }


    }
}
