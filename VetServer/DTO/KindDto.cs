using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VetServer.DTO
{
    public class KindDto
    {
        public int KindId { get; set; }
        public string KindName { get; set; }
    }
}
