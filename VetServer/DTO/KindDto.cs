using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VetServer.DTO
{
    public class KindDto
    {
        public int kind_id { get; set; }
        public string KindName { get; set; }
    }
}
