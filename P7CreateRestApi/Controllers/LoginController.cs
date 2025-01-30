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
using Microsoft.Extensions.Logging;

namespace P7CreateRestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly UserRepository _userRepository;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<LoginController> _logger;

        public LoginController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration, UserRepository userRepository,
            RoleManager<IdentityRole> roleManager,
            ILogger<LoginController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _userRepository = userRepository;
            _roleManager = roleManager;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModelDto model)
        {
            _logger.LogInformation("Registration attempt for user: {UserName}", model.UserName);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for registration attempt: {Errors}",
                    string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                return BadRequest(ModelState);
            }

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                _logger.LogWarning("Registration attempt with existing email: {Email}", model.Email);
                return BadRequest(new { Message = "Cet email est déjà utilisé." });
            }

            var identityUser = new IdentityUser
            {
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(identityUser, model.Password);

            if (result.Succeeded)
            {
                var role = "User";
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    _logger.LogInformation("Creating new role: {Role}", role);
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }

                await _userManager.AddToRoleAsync(identityUser, role);

                var appUser = new User
                {
                    UserName = model.UserName,
                    FullName = model.FullName,
                    Role = role
                };
                await _userRepository.AddAsync(appUser);

                _logger.LogInformation("User registered successfully: {UserName} with role: {Role}", model.UserName, role);
                return Ok(new { Message = $"Utilisateur enregistré avec succès avec le rôle : {role}" });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
                _logger.LogWarning("User registration error: {Error}", error.Description);
            }

            _logger.LogError("User registration failed for: {UserName}", model.UserName);
            return BadRequest(ModelState);
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModelDto model)
        {
            _logger.LogInformation("Login attempt for user: {UserName}", model.UserName);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for login attempt");
                return BadRequest(ModelState);
            }
            if (string.IsNullOrEmpty(model.UserName))
            {
                _logger.LogWarning("Login attempt with empty username");
                return BadRequest("Le nom d'utilisateur ne peut être vide.");
            }
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault() ?? "User";
                var token = GenerateJwtToken(user, role);
                _logger.LogInformation("User {UserName} logged in successfully", model.UserName);
                return Ok(new { Token = token });
            }

            _logger.LogWarning("Failed login attempt for user: {UserName}", model.UserName);
            return Unauthorized();
        }


        private string GenerateJwtToken(IdentityUser User, string role)
        {
            _logger.LogInformation("Generating JWT for user {UserName} with role {Role}", User.UserName, role);

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
