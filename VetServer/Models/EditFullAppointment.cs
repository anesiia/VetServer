using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VetServer.Models
{
    public class EditFullAppointment
    {

        public DateTime AppointmentDate { get; set; }

        public TimeOnly AppointmentTime { get; set; }

        public int doctor_id { get; set; }

        public int patient_id { get; set; }

        public string? AppointmentDiagnose { get; set; }

        public string? AppointmentInfo { get; set; }
    }
}
