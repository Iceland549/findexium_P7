using P7CreateRestApi.Domain;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using P7CreateRestApi.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace P7CreateRestApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _repository;
        private readonly ILogger<UserController> _logger;

        public UserController(UserRepository repository, ILogger<UserController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllAsync()
        {
            _logger.LogInformation("GET /api/User called at {time}", DateTime.UtcNow); // Log d'information

            try
            {
                var users = await _repository.GetAllAsync();
                _logger.LogInformation("Successfully retrieved users at {time}", DateTime.UtcNow); // Log après récupération des données
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving users at {time}", DateTime.UtcNow); // Log en cas d'erreur
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<User>> GetByIdAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        //[HttpPost]
        //public async Task<ActionResult<User>> CreateAsync([FromBody] User user)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    await _repository.AddAsync(user);
        //    return CreatedAtAction(nameof(GetByIdAsync), new { id = user.Id }, user);
        //}

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UserDto userDto)
        {
            if (id != userDto.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var originalUser = await _repository.GetByIdAsync(id);
                if (originalUser == null)
                {
                    return NotFound();
                }
                originalUser.UserName = userDto.Username;
                originalUser.FullName = userDto.Fullname;
                originalUser.Role = userDto.Role;

                await _repository.UpdateAsync(originalUser);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                await _repository.DeleteAsync(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
