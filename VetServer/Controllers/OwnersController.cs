using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VetServer.Data;
using VetServer.DTO;
using VetServer.Models;

namespace VetServer.Controllers
{
    //api/Owners
    [Route("api/[controller]")]
    [ApiController]
    public class OwnersController : ControllerBase
    {
        private readonly VetCareDbContext _context;
        private readonly IPasswordHasher<Owners> _passwordHasher;
        private readonly ILogger<OwnersController> _logger;

        public OwnersController(VetCareDbContext context, IPasswordHasher<Owners> passwordHasher, ILogger<OwnersController> logger)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _logger = logger; // ?? throw new ArgumentNullException(nameof(logger));
        }


        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser(Owners owner)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); 
                    

                }
                var hashedPassword = _passwordHasher.HashPassword(owner, owner.OwnerPassHash);
                owner.OwnerPassHash = hashedPassword;

                _context.Owners.Add(owner);
                await _context.SaveChangesAsync();

                return Ok("User registration successful");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during user register");
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] OwnerLoginModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _context.Owners.FirstOrDefaultAsync(ow => ow.OwnerEmail == model.Email);

                if (user == null)
                {
                    return BadRequest(ModelState);
                }

                var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.OwnerPassHash, model.Password);

                if (passwordVerificationResult != PasswordVerificationResult.Success)
                {
                    return BadRequest(ModelState);
                }

                return Ok(user);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login");
                return StatusCode(500, ex.Message);
            }
        }

        /*
        // GET: api/<OwnersController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<OwnersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<OwnersController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<OwnersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<OwnersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }*/
    }
}
