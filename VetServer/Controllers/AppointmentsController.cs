using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetServer.Data;
using VetServer.DTO;
using VetServer.Models;
using VetServer.Models.Database;

//GetAppointmentInfo
//UpdateAppointmentInfo
//GetDoctorAppointmentsNextWeek
//GetDoctorAppointments
//GetOwnerAppointments
//GetPastOwnerAppointments
//GetFutureOwnerAppointments

namespace VetServer.Controllers
{
    //api/Appointments
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : Controller
    {
        private readonly VetCareDbContext _context;
        private readonly ILogger<AppointmentsController> _logger;

        public AppointmentsController(VetCareDbContext context, ILogger<AppointmentsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        

        // GET: Appointments/GetVisitDetails/5
        [HttpGet("appointment-details/{id}")]
        public async Task<IActionResult> GetAppointmentInfo(int id)
        {
            try
            {
                var appointment = await _context.Appointments.FindAsync(id);

                if (appointment == null)
                {
                    return NotFound("there is no appointment");
                }
                var appointmentDto = new AppointmentDto
                {
                    AppointmentId = appointment.AppointmentId,
                    AppointmentDate = appointment.AppointmentDate,
                    AppointmentTime = appointment.AppointmentTime,
                    doctor_id = appointment.doctor_id,
                    patient_id = appointment.patient_id,
                    AppointmentDiagnose = appointment.AppointmentDiagnose,
                    AppointmentInfo = appointment.AppointmentInfo

                };

                return Ok(appointmentDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during appointment info loading");
                return StatusCode(500, ex.Message);
            }
            
        }


        [HttpPut("update-appointment/{id}")]
        public async Task<IActionResult> UpdateAppointmentInfo(int id, string diagnose, string info)
        {
            try
            {
                var appointment = await _context.Appointments.FindAsync(id);
                if (appointment == null)
                {
                    return NotFound("There is no visit with the provided ID.");
                }

                appointment.AppointmentDiagnose = diagnose;
                appointment.AppointmentInfo = info;

                _context.Update(appointment);
                await _context.SaveChangesAsync();

                return Ok("Visit details updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during appointment updating");
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("doctor-week-schedule/{doctorId}")]
        public async Task<IActionResult> GetDoctorAppointmentsNextWeek(int doctorId)
        {
            try
            {
                DateTime today = DateTime.Now.Date;
                DateTime nextWeek = today.AddDays(7);

                var doctorSchedule = _context.Appointments
                    .Where(s => s.doctor_id == doctorId && s.AppointmentDate >= today && s.AppointmentDate < nextWeek)
                    .Select(s => new AppointmentDto
                    {
                        AppointmentId = s.AppointmentId,
                        AppointmentDate = s.AppointmentDate,
                        AppointmentTime = s.AppointmentTime,
                        doctor_id = s.doctor_id,
                        patient_id = s.patient_id,
                        AppointmentDiagnose = s.AppointmentDiagnose,
                        AppointmentInfo = s.AppointmentInfo
                    })
                    .ToList();

                return Ok(doctorSchedule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during doctor week schedule loading");
                return StatusCode(500, ex.Message);
            }
        }

        // GET: /GetDoctorAppointments/5
        [HttpGet("doctor-all-appointments/{doctorId}")]
        public async Task<IActionResult> GetDoctorAppointments(int doctorId)
        {
            try
            {
                var doctorSchedule = _context.Appointments
                    .Where(s => s.doctor_id == doctorId)
                    .Select(s => new AppointmentDto
                    {
                        AppointmentId = s.AppointmentId,
                        AppointmentDate = s.AppointmentDate,
                        AppointmentTime = s.AppointmentTime,
                        doctor_id = s.doctor_id,
                        patient_id = s.patient_id,
                        AppointmentDiagnose = s.AppointmentDiagnose,
                        AppointmentInfo = s.AppointmentInfo
                    })
                    .ToList();

                return Ok(doctorSchedule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during doctor summary schedule loading");
                return StatusCode(500, ex.Message);
            }
        }


        // GET: api/Appointments/GetOwnerAppointments/{ownerId}
        [HttpGet("pet-owner-appointments/{ownerId}")]
        public async Task<ActionResult<IEnumerable<Appointments>>> GetOwnerAppointments(int ownerId)
        {
            try
            {
                var appointments = await _context.Appointments
                    .Join(_context.Patients,
                        appointment => appointment.patient_id,
                        patient => patient.PatientId,
                        (appointment, patient) => new { Appointment = appointment, Patient = patient })
                    .Join(_context.Owners,
                        combined => combined.Patient.owner_id,
                        owner => owner.owner_id,
                        (combined, owner) => new { combined.Appointment, combined.Patient, Owner = owner })
                    .Where(combined => combined.Owner.owner_id == ownerId)
                    .Select(combined => combined.Appointment)
                    .ToListAsync();

                return Ok(appointments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during pet owner appointments loading");
                return StatusCode(500, ex.Message);
            }

        }

        // GET: api/Appointments/GetPastOwnerAppointments/{ownerId}
        [HttpGet("pet-owner-old-appointments/{ownerId}")]
        public async Task<ActionResult<IEnumerable<Appointments>>> GetPastOwnerAppointments(int ownerId)
        {
            try
            {
                var today = DateTime.Today;

                var appointments = await _context.Appointments
                    .Join(_context.Patients,
                        appointment => appointment.patient_id,
                        patient => patient.PatientId,
                        (appointment, patient) => new { Appointment = appointment, Patient = patient })
                    .Join(_context.Owners,
                        combined => combined.Patient.owner_id,
                        owner => owner.owner_id,
                        (combined, owner) => new { combined.Appointment, combined.Patient, Owner = owner })
                    .Where(combined => combined.Owner.owner_id == ownerId && combined.Appointment.AppointmentDate < today)
                    .Select(combined => combined.Appointment)
                    .ToListAsync();

                return Ok(appointments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during PAST pet owner appointments loading");
                return StatusCode(500, ex.Message);
            }
        }


        // GET: api/Appointments/GetFutureOwnerAppointments/{ownerId}
        [HttpGet("pet-owner-future-appointments/{ownerId}")]
        public async Task<ActionResult<IEnumerable<Appointments>>> GetFutureOwnerAppointments(int ownerId)
        {
            try
            {
                var today = DateTime.Today;

                var appointments = await _context.Appointments
                    .Join(_context.Patients,
                        appointment => appointment.patient_id,
                        patient => patient.PatientId,
                        (appointment, patient) => new { Appointment = appointment, Patient = patient })
                    .Join(_context.Owners,
                        combined => combined.Patient.owner_id,
                        owner => owner.owner_id,
                        (combined, owner) => new { combined.Appointment, combined.Patient, Owner = owner })
                    .Where(combined => combined.Owner.owner_id == ownerId && combined.Appointment.AppointmentDate >= today)
                    .Select(combined => combined.Appointment)
                    .ToListAsync();

                return Ok(appointments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during FUTURE pet owner appointments loading");
                return StatusCode(500, ex.Message);
            }
        }


        // POST: Appointment/create-appointment
        [HttpPost("create-appointment")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> MakeNewAppointment( DateTime appointmentDate, TimeOnly appointmentTime, int doctorId, int patientId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                    
                }

                var newAppointment = new Appointments
                {
                    AppointmentDate = appointmentDate,
                    AppointmentTime = appointmentTime,
                    doctor_id = doctorId,
                    patient_id = patientId,

                };

                _context.Add(newAppointment);
                await _context.SaveChangesAsync();
                return Ok("Appointment was succesfully created");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during creating NEW appointment");
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/Appointments/DeleteAppointment/{appointmentId}
        [HttpDelete("delete-appointment/{appointmentId}")]
        public async Task<IActionResult> DeleteAppointment(int appointmentId)
        {
            try
            {
                var appointmentToDelete = await _context.Appointments.FindAsync(appointmentId);

                if (appointmentToDelete == null)
                {
                    return NotFound($"Appointment with ID {appointmentId} not found");
                }

                _context.Appointments.Remove(appointmentToDelete);
                await _context.SaveChangesAsync();

                return Ok($"Appointment with ID {appointmentId} deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during DELETING appointment");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("year-consultations-per-month")]
        public IActionResult GetYearConsultationsPerMonth()
        {
            try
            {
                DateTime currentDate = DateTime.Now;
                DateTime firstDayOfCurrentYear = new DateTime(currentDate.Year, 1, 1);

                var consultationsPerMonth = _context.Appointments
                    .Where(c => c.AppointmentDate >= firstDayOfCurrentYear)
                    .GroupBy(c => new { c.AppointmentDate.Year, c.AppointmentDate.Month })
                    .Select(group => new AppointmentsYearStatistics
                    {
                        Month = $"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(group.Key.Month)} {group.Key.Year}",
                        ConsultationCount = group.Count()
                    })
                    .OrderBy(group => group.Month)
                    .ToList();

                return Ok(consultationsPerMonth);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching consultations per month");
                return StatusCode(500, ex.Message);
            }
        }

    }
}
