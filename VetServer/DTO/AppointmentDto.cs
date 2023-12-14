using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VetServer.DTO
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }

        public DateTime AppointmentDate { get; set; }

        public TimeOnly AppointmentTime { get; set; }

        public int doctor_id { get; set; }

        public int patient_id { get; set; }

        public string? AppointmentDiagnose { get; set;}

        public string? AppointmentInfo { get; set;}

    }
}
