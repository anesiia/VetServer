using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using VetServer.Data;
using VetServer.DTO;
using VetServer.Models;
using VetServer.Models.Database;
namespace VetServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly VetCareDbContext _context;
        private readonly ILogger<DoctorsController> _logger;

        public StatisticsController(VetCareDbContext context, ILogger<DoctorsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: /popular-animal-kinds
        [HttpGet("popular-animal-kinds")]
        public IActionResult GetPopularAnimalKinds()
        {
            try
            {
                var popularAnimalTypes = _context.Kinds
                    .Select(type => new KindRating
                    {
                        Name = type.KindName,
                        Count = _context.Patients.Count(p => p.kind_id == type.kind_id)
                    })
                    .OrderByDescending(info => info.Count)
                    .ToList();

                return Ok(popularAnimalTypes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while loading kinds rating");
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
                    .Where(c => c.AppointmentDate >= firstDayOfCurrentYear).AsEnumerable()
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

        [HttpGet("top-workers")]
        public IActionResult GetDoctorsStatistic()
        {
            try
            {
                var doctorsAppointmentsAmout = _context.Doctors
                    .Select(doctor => new DoctorRating
                    {
                        DoctorName = doctor.DoctorName,
                        Count = _context.Appointments.Count(c => c.doctor_id == doctor.DoctorId)
                    })
                    .OrderByDescending(info => info.Count)
                    .ToList();

                return Ok(doctorsAppointmentsAmout);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while loading top-workers");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("top-workers-in-month")]
        public IActionResult GetDoctorsStatisticByMonth()
        {
            try
            {
                DateTime currentDate = DateTime.Now;
                DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);

                // Фильтрация консультаций, проведенных за последний месяц
                var doctorsAppointmentsAmout = _context.Doctors
                    .Select(doctor => new DoctorRating
                    {
                        DoctorName = doctor.DoctorName,
                        Count = _context.Appointments
                            .Count(c => c.doctor_id == doctor.DoctorId && c.AppointmentDate >= firstDayOfMonth)
                    })
                    .OrderByDescending(info => info.Count)
                    .ToList();

                return Ok(doctorsAppointmentsAmout);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching doctor consultation info for the last month");
                return StatusCode(500, ex.Message);
            }
        }

    }
}
