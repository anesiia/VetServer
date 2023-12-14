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

        // GET: Drugs/GetDrugs
        [HttpGet("GetDrugs")]
        public IActionResult GetDrugs()
        {
            var drugs = _context.Drugs.Select(d => new DrugDto
            {
                DrugId = d.DrugId,
                DrugName = d.DrugName,
                DrugQuantity = d.DrugQuantity
            }).ToList();
            return Ok(drugs);
        }

        // GET: Drugs/GetDrugInfo/5
        [HttpGet("GetDrugInfo/{id}")]
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
                return StatusCode(500, ex.Message);
            }

        }

        // PUT: Drugs/UpdateDrugAmount/5
        [HttpPut("UpdateDrugAmount/{id}")]
        public async Task<IActionResult> UpdateDrugAmount(int id, int amount)
        {
            try
            {
                var drug= await _context.Drugs.FindAsync(id);
                if (drug == null)
                {
                    return NotFound("There is no drug with the provided ID");
                }

                drug.DrugQuantity = amount;
                _context.Update(drug);
                await _context.SaveChangesAsync();

                return Ok($"{drug.DrugName} amount {amount} updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: Drugs/AddDrug
        [HttpPost("AddDrug")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDrug([Bind("DrugName,DrugQuantity")] AddDrugs drugs)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(drugs);
                    await _context.SaveChangesAsync();
                    return Ok($"{drugs.DrugQuantity} - {drugs.DrugName} were added successfully");
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            { 
                return StatusCode(500, ex.Message);
           
            }
        }

        // Delete: Drugs/DeleteDrug/5
        [HttpDelete, ActionName("DeleteDrug")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDrug(int id)
        {
            try
            {
                var drug = await _context.Drugs.FindAsync(id);
                if (drug != null)
                {
                    _context.Drugs.Remove(drug);
                    await _context.SaveChangesAsync();
                    return Ok($"{drug.DrugName} was successfully deleted");
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);

            }
            
        }

        /*
        private bool DrugsExists(int id)
        {
            return _context.Drug.Any(e => e.DrugId == id);
        }*/
    }
}
