using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VetServer.DTO
{
    public class AddAppointmentDto
    {
        /*public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeOnly AppointmentTime { get; set; }
        public string? Diagnose { get; set; }
        public string? Info { get; set; }*/

        [Column("appointment_date", TypeName = "date")]
        public DateTime AppointmentDate { get; set; }

        [Column("appointment_time", TypeName = "time(7)")]
        public TimeOnly AppointmentTime { get; set; }

        [ForeignKey("doctor_id")]
        public int doctor_id { get; set; }

        [ForeignKey("patient_id")]
        public int patient_id { get; set; }

        [Column("appointment_diagnose")]
        public string? AppointmentDiagnose { get; set; }

        [Column("appointment_info")]
        public string? AppointmentInfo { get; set; }
    }
}
