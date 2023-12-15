using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VetServer.Data;
using VetServer.DTO;
using VetServer.Models;
using VetServer.Models.Database;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VetServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly VetCareDbContext _context;
        private readonly ILogger<DrugsController> _logger;

        public PatientsController(VetCareDbContext context, ILogger<DrugsController> logger)
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

        // POST: /add-patient
        [HttpPost("add-patient")]
        public IActionResult AddPatient([FromBody] AddPatient model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var ownerExists = _context.Owners.Any(owner => owner.owner_id == model.owner_id);
                if (!ownerExists)
                {
                    return BadRequest("Owner with the specified ID does not exist");
                }

                var animalTypeExists = _context.Kinds.Any(type => type.kind_id == model.kind_id);
                if (!animalTypeExists)
                {
                    return BadRequest("Animal type with the specified ID does not exist");
                }

                // Создание нового пациента
                var newPatient = new Patients
                {
                    PatientName = model.Name,
                    owner_id = model.owner_id,
                    kind_id = model.kind_id, 
                    PatientAge = model.Age,
                    PatientSex = model.Sex
                };

                // Добавление пациента в базу данных
                _context.Patients.Add(newPatient);
                _context.SaveChanges();

                return Ok("Patient added successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a patient");
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE /delete-patient/5
        [HttpDelete("delete-patient/{patientId}")]
        public IActionResult DeletePatient(int patientId)
        {
            try
            {
                var patientToDelete = _context.Patients.Find(patientId);

                if (patientToDelete == null)
                {
                    return NotFound("Patient not found");
                }

                _context.Patients.Remove(patientToDelete);
                _context.SaveChanges();

                return Ok("Patient deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a patient");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
