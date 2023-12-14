using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VetServer.Models.Database
{
    public class Cages
    {
        [Key]
        [Column("cage_id")]
        public int CageId { get; set; }

        [Column("cage_temperature")]
        public float CageTemperature { get; set; }

        [Column("cage_oxygen")]
        public float CageOxygen { get; set; }

        [ForeignKey("PatientId")]
        public int PatientId { get; set; }

    }
}
