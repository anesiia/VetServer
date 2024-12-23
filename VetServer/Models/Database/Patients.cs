﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VetServer.Models.Database
{
    public class Patients
    {
        [Key]
        [Column("patient_id")]
        public int PatientId { get; set; }

        [Column("patient_name")]
        public string PatientName { get; set; }

        [Column("patient_age")]
        public int PatientAge { get; set; }

        [Column("patient_sex")]
        public string PatientSex { get; set; }

        [ForeignKey("owner_id")]
        public int owner_id { get; set; }

        [ForeignKey("kind_id")]
        public int kind_id { get; set; }

    }
}
