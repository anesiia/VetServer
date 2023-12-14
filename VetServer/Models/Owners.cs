using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VetServer.Models
{
    public class Owners
    {
        [Key]
        [Column("owner_id")]
        public int owner_id { get; set; }

        [Column("owner_name")] 
        public string OwnerName { get; set; }

        [Phone]
        [Column("owner_phone")]
        public string OwnerPhone { get; set; }

        [Column("owner_email")]
        [EmailAddress]
        public string OwnerEmail { get; set; }

        [Column("owner_pass_hash")]
        public string OwnerPassHash { get; set; }
    }
}

