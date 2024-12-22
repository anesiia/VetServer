using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using VetServer.Data;
using VetServer.DTO;
using VetServer.Models;
using VetServer.Models.Database;

namespace VetServer.Controllers
{
    // api/Doctors
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly VetCareDbContext _context;
        private readonly ILogger<DoctorsController> _logger;
        private readonly IPasswordHasher<Doctors> _passwordHasher;

        public DoctorsController(VetCareDbContext context, ILogger<DoctorsController> logger, IPasswordHasher<Doctors> passwordHasher)
        {
            _context = context;
            _logger = logger;
            _passwordHasher = passwordHasher;
        }

        // GET: api/Doctors/all-doctors
        [HttpGet("all-doctors")]
        public async Task<IActionResult> GetDoctors()
        {
            var doctors = _context.Doctors
                .OrderBy(d => d.DoctorName)
                .Select(d => new DoctorDto
                {
                    DoctorId = d.DoctorId,
                    DoctorName = d.DoctorName,
                    DoctorEmail = d.DoctorEmail,
                    DoctorPhone = d.DoctorPhone
                }).ToList();
            return Ok(doctors);
        }

        // GET: api/Doctors/doctor-details/5
        [HttpGet("doctor-details/{id}")]
        public async Task<IActionResult> GetDoctorInfo(int id)
        {
            try
            {
                var doctor = await _context.Doctors.FindAsync(id);

                if (doctor == null)
                {
                    return NotFound("there is no doctor");
                }
                var doctorDto = new DoctorDto
                {
                    DoctorId = doctor.DoctorId,
                    DoctorName = doctor.DoctorName,
                    DoctorPhone = doctor.DoctorPhone,
                    DoctorEmail = doctor.DoctorEmail,
                    DoctorAddress = doctor.DoctorAddress,
                    IsAdmin = doctor.IsAdmin

                };

                return Ok(doctorDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during doctor info loading");
                return StatusCode(500, ex.Message);
            }

        }

        // POST: api/Doctors/register
        [HttpPost("add-new-doctor")]
        public async Task<ActionResult> AddDoctor(DoctorRegistration model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _context.Doctors.AnyAsync(d => d.DoctorEmail == model.Email))
                {
                    return BadRequest("Doctor with such email already exists");
                }

                var newDoctor = new Doctors
                {
                    DoctorName = model.Name,
                    DoctorEmail = model.Email,
                    DoctorPhone = model.Phone,
                    DoctorAddress = model.Address,
                };

                newDoctor.DoctorPassHash = _passwordHasher.HashPassword(newDoctor, model.Password);

                await _context.Doctors.AddAsync(newDoctor);
                await _context.SaveChangesAsync();

                return Ok("Doctor registration was successful");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during doctor register");
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/Doctors/login
        [HttpPost("login")]
        public async Task<IActionResult> DoctorLogin(DoctorLoginModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.DoctorEmail == model.Email);

                if (doctor == null)
                {
                    return BadRequest(ModelState);
                }

                var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(doctor, doctor.DoctorPassHash, model.PassHash);

                if (passwordVerificationResult != PasswordVerificationResult.Success)
                {
                    return BadRequest(ModelState);
                }

                return Ok(doctor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login");
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/Doctors/delete-doctor/5
        [HttpDelete("delete-doctor/{id}")]
        public IActionResult DeleteDoctor(int id)
        {
            try
            {
                var doctorToDelete = _context.Doctors.Find(id);

                if (doctorToDelete == null)
                {
                    return NotFound($"Doctor with ID {id} not found");
                }

                _context.Doctors.Remove(doctorToDelete);
                _context.SaveChanges();

                return Ok($"Doctor with ID {id} deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting doctor with ID {id}");
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/Doctors/update-doctor-info/5
        [HttpPut("update-doctor-info/{id}")]
        public IActionResult UpdateDoctor(int id, [FromBody] EditDoctor model)
        {
            try
            {
                var doctorToUpdate = _context.Doctors.Find(id);

                if (doctorToUpdate == null)
                {
                    return NotFound($"Doctor with ID {id} not found");
                }

                doctorToUpdate.DoctorName = model.Name;
                doctorToUpdate.DoctorEmail = model.Email;
                doctorToUpdate.DoctorPhone = model.Phone;
                doctorToUpdate.DoctorAddress = model.Address;
                doctorToUpdate.IsAdmin = model.IsAdmin;

                _context.SaveChanges();

                return Ok($"Doctor with ID {id} updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating doctor with ID {id}");
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Appointments/CountByDoctorAndDate
        [HttpGet("CountByDoctorAndDate")]
        public ActionResult<int> GetAppointmentCountByDoctorAndDate(int doctorId, DateTime appointmentDate)
        {
            var doctorIdParam = new SqlParameter("@doctor_id", doctorId);
            var appointmentDateParam = new SqlParameter("@appointment_date", appointmentDate.Date);

            var sql = "SELECT dbo.GetAppointmentCountByDoctorAndDate(@doctor_id, @appointment_date)";
            var count = _context.AppointmentCount.FromSqlRaw(sql, doctorIdParam, appointmentDateParam).AsEnumerable().FirstOrDefault();


            Console.WriteLine($"Returned count: {count.Counte}");

            return (int)count.Counte;
        }
    }
}

