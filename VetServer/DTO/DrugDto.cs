using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VetServer.DTO
{
    public class DrugDto
    {
        public int DrugId { get; set; }

        public string DrugName { get; set; }

        public int DrugQuantity { get; set; }

    }
}
