using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VetServer.Models
{
    public class EditDrugAmount
    {
        public int Id { get; set; }

        public int Quantity { get; set; }
    }
}
