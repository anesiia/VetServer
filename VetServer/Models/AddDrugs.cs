using System.ComponentModel.DataAnnotations.Schema;

namespace VetServer.Models
{
    public class AddDrugs
    {
        [Column("drug_name")]
        public string DrugName { get; set; }

        [Column("drug_quantity")]
        public int DrugQuantity { get; set; }
    }
}
