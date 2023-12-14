using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VetServer.Models.Database
{
    public class Drugs
    {
        [Key]
        [Column("drug_id")]
        public int DrugId { get; set; }

        [Column("drug_name")]
        public string DrugName { get; set; }

        [Column("drug_quantity")]
        public int DrugQuantity { get; set; }

    }
}
