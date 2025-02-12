using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using P7CreateRestApi.Controllers;
using P7CreateRestApi.Dtos;
using P7CreateRestApi.Domain;
using System.Text.Json;


namespace P7CreateRestApi.Tests.Services
{
    public class LoginControllerTest
    {
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<SignInManager<User>> _mockSignInManager;
        private readonly Mock<RoleManager<IdentityRole>> _mockRoleManager;
        private readonly IConfiguration _configuration;
        private readonly LoginController _controller;

        public LoginControllerTest()
        {
            // Créer les mocks pour les dépendances
            _mockUserManager = new Mock<UserManager<User>>
                (
                Mock.Of<IUserStore<User>>(),
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<IPasswordHasher<User>>(),
                new List<IUserValidator<User>>(),
                new List<IPasswordValidator<User>>(),
                Mock.Of<ILookupNormalizer>(),
                Mock.Of<IdentityErrorDescriber>(),
                Mock.Of<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<User>>>()
                );

            _mockSignInManager = new Mock<SignInManager<User>>
                (
                _mockUserManager.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<User>>(),
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<ILogger<SignInManager<User>>>(),
                Mock.Of<IAuthenticationSchemeProvider>(),
                Mock.Of<IUserConfirmation<User>>()
                );

            _mockRoleManager = new Mock<RoleManager<IdentityRole>>(
                Mock.Of<IRoleStore<IdentityRole>>(),
                new List<IRoleValidator<IdentityRole>>(),
                Mock.Of<ILookupNormalizer>(),
                Mock.Of<IdentityErrorDescriber>(),
                Mock.Of<ILogger<RoleManager<IdentityRole>>>()
                );

            // Configurer la configuration en mémoire
            var initialData = new Dictionary<string, string?>
            {
                { "Jwt:Key", "ThisIsASecretKeyThatIsLongEnough1234567890" },
                { "Jwt:Issuer", "TestIssuer" },
                { "Jwt:Audience", "TestAudience" }
            };
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(initialData)
                .Build();

            // Instancier le contrôleur avec les mocks
            _controller = new LoginController(
                _mockUserManager.Object,
                _mockSignInManager.Object,
                _configuration,
                _mockRoleManager.Object
            );
        }

        [Fact]
        public async Task Register_ValidUser_ReturnsOk()
        {
            // Arrange
            var registerDto = new RegisterModelDto
            {
                UserName = "testuser",
                Email = "testuser@test.com",
                Password = "Test@123"
            };

            // Configurer le mock UserManager pour simuler la création réussie de l'utilisateur
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _mockUserManager.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _mockRoleManager.Setup(x => x.RoleExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act 
            var result = await _controller.Register(registerDto); ;

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            // Convertir la réponse en JSON puis en Dictionary
            var json = JsonSerializer.Serialize(okResult.Value);
            var response = JsonSerializer.Deserialize<Dictionary<string, object>>(json);    

            // Vérifier le message
            Assert.NotNull(response);
            Assert.True(response.ContainsKey("Message"));
            Assert.Equal("Utilisateur enregistré avec succès avec le rôle : User", response["Message"].ToString());
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsOkWithToken()
        {
            // Arrange
            var loginDto = new LoginModelDto
            {
                UserName = "testuser",
                Password = "Test@123"
            };

            var testUser = new User
            {
                UserName = "testuser",
                Id = "tesuserid"
            };

            // Mocking UserManager
            _mockUserManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(testUser);

            _mockUserManager.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            _mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(["User"]);

            // Act 
            var result = await _controller.Login(loginDto);

            // Assert
            var okresult = Assert.IsType<OkObjectResult>(result);

            // Convertir la réponse en JSON puis en Dictionary
            var json = JsonSerializer.Serialize(okresult.Value);
            var response = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

            // Vérifier le token
            Assert.NotNull(response);
            Assert.True(response.ContainsKey("Token"));
            Assert.False(string.IsNullOrEmpty(response["Token"].ToString()));
        }

        [Fact]
        public async Task Logout_ReturnsOk()
        {
            // Arrange
            _mockSignInManager.Setup(x => x.SignOutAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Logout();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            // Convertir la réponse en JSON puis en Dictionary
            var json = JsonSerializer.Serialize(okResult.Value);
            var response = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

            // Vérifier le message de confirmation
            Assert.NotNull(response);
            Assert.True(response.ContainsKey("Message"));
            Assert.Equal("Logout successful", response["Message"].ToString());
        }
    }
}




