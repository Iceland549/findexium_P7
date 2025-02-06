using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using P7CreateRestApi.Dtos;
using P7CreateRestApi.Repositories;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using P7CreateRestApi.Domain;
using System.Security.Claims;
using System.Text;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace P7CreateRestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;

        public LoginController(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration configuration,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModelDto model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return BadRequest(new { Message = "Cet email est déjà utilisé." });
            }

            var User = new User
            {
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(User, model.Password);

            if (result.Succeeded)
            {
                var role = "User";
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }

                await _userManager.AddToRoleAsync(User, role);

                return Ok(new { Message = $"Utilisateur enregistré avec succès avec le rôle : {role}" });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return BadRequest(ModelState);
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModelDto model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (string.IsNullOrEmpty(model.UserName))
            {
                return BadRequest("Le nom d'utilisateur ne peut être vide.");
            }
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles != null)
                {
                    var role = roles.ToList().Contains("Admin") ? "Admin" : "User";
                    var token = GenerateJwtToken(user, role);
                    return Ok(new { Token = token });
                }

            }

            return Unauthorized();
        }


        private string GenerateJwtToken(IdentityUser User, string role)
        {

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, User.UserName), // Identifie l'utilisateur par son nom d'utilisateur
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Identifiant unique du token
                new(ClaimTypes.NameIdentifier, User.Id), // Identifie l'utilisateur par son ID unique
                new(ClaimTypes.Role, role) // Rôle utilisateur pour gérer les autorisations
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(60); 

            var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: expires,
            signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { Message = "Logout successful" });
        }
    }
}
