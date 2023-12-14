using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetServer.Data;
using VetServer.DTO;
using VetServer.Models;
using VetServer.Models.Database;

namespace VetServer.Controllers
{
    // api/Drugs
    [ApiController]
    [Route("api/[controller]")]
    public class DrugsController : Controller
    {
        private readonly VetCareDbContext _context;
        private readonly ILogger<DrugsController> _logger;

        public DrugsController(VetCareDbContext context, ILogger<DrugsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: /all-drugs
        [HttpGet("all-drugs")]
        public async Task<IActionResult> GetDrugs()
        {
            try
            {
                var drugs = _context.Drugs.Select(d => new DrugDto
                {
                    DrugId = d.DrugId,
                    DrugName = d.DrugName,
                    DrugQuantity = d.DrugQuantity
                }).ToList();
                return Ok(drugs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during all drugs loading");
                return StatusCode(500, ex.Message);
            }
            
        }

        // GET: Drugs/drug-details/5
        [HttpGet("drug-details/{id}")]
        public async Task<IActionResult> GetDrugInfo(int id)
        {
            try
            {
                var drug = await _context.Drugs.FindAsync(id);

                if (drug == null)
                {
                    return NotFound("There is no drug with the provided ID");
                }
                var drugDto = new DrugDto
                {
                    DrugId = drug.DrugId,
                    DrugName = drug.DrugName,
                    DrugQuantity = drug.DrugQuantity
                };

                return Ok(drugDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during drug loading info");
                return StatusCode(500, ex.Message);
            }

        }

        // PUT: /update-drug-amount/5
        [HttpPut("update-drug-amount/{id}")]
        public async Task<IActionResult> UpdateDrugAmount(EditDrugAmount model)
        {
            try
            {
                var drug = await _context.Drugs.FindAsync(model.Id);
                if (drug == null)
                {
                    return NotFound("There is no drug with the provided ID");
                }

                drug.DrugQuantity = model.Quantity;
                _context.Update(drug);
                await _context.SaveChangesAsync();

                return Ok($"Drug amount {drug.DrugQuantity} updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during drug updating amount");
                return StatusCode(500, ex.Message);
            }
        }

        // POST: /add-new-drug
        [HttpPost("add-new-drug")]
        public async Task<IActionResult> AddDrug(AddDrugs model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _context.Drugs.AnyAsync(d => d.DrugName == model.Name))
                {
                    return BadRequest("Drug with such name already exists");
                }

                var newDrug= new Drugs
                {
                    DrugName = model.Name,
                    DrugQuantity = model.Quantity
                };

                await _context.Drugs.AddAsync(newDrug);
                await _context.SaveChangesAsync();

                return Ok($"{newDrug.DrugQuantity} - {newDrug.DrugName} were added successfully");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during adding new drug");
                return StatusCode(500, ex.Message);
           
            }
        }

        // Delete: /delete-drug/5
        [HttpDelete, ActionName("delete-drug")]
        public async Task<IActionResult> DeleteDrug(int id)
        {
            try
            {
                var drug = await _context.Drugs.FindAsync(id);
                if (drug == null)
                {
                    return BadRequest(ModelState);                
                }
                _context.Drugs.Remove(drug);
                await _context.SaveChangesAsync();

                return Ok($"{drug.DrugName} was successfully deleted");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during deleting drug");
                return StatusCode(500, ex.Message);

            }
            
        }
    }
}
