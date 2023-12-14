using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VetServer.Models
{
    public class Kinds
    {
        [Key] // Указывает, что это первичный ключ
        [Column("kind_id")]
        public int KindId { get; set; }

        [Column("kind_name")] // Указывает на имя столбца в базе данных
        public string KindName { get; set; }
    }
}
