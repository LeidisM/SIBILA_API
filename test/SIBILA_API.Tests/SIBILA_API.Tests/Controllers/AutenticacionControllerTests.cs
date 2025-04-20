using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SIBILA_API.Controllers;
using SIBILA_API.Data;
using SIBILA_API.Models;

namespace SIBILA_API.Tests.Controllers
{
    public class AutenticacionControllerTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        private IConfiguration GetFakeConfiguration()
        {
            var settings = new Dictionary<string, string>
            {
                { "JwtSettings:SecretKey", "clave-super-secreta-para-pruebas-unitarias" }
            };

            return new ConfigurationBuilder().AddInMemoryCollection(settings).Build();
        }

        [Fact]
        public async Task Login_ReturnsToken_WhenCredentialsAreValid()
        {
            var context = GetInMemoryDbContext();
            var config = GetFakeConfiguration();

            var rol = new Roles { Id = 1, Nombre = "Administrador" };
            var user = new Usuarios
            {
                Nombre = "Test",
                Apellido = "Apiellido1",
                Documento = "12345678",
                TipoDocumento = Models.Enums.TipoDocumentoUsuarioEnum.CC,
                CorreoElectronico = "test@example.com",
                Contrasena = "1234",
                Rol = rol
            };

            context.Roles.Add(rol);
            context.Usuarios.Add(user);
            await context.SaveChangesAsync();

            var controller = new AutenticacionController(context, config);
            var loginRequest = new LoginRequest { Email = "test@example.com", Password = "1234" };

            var result = await controller.Login(loginRequest) as OkObjectResult;

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Contains("Token", result.Value.ToString());
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenUserNotFound()
        {
            var context = GetInMemoryDbContext();
            var controller = new AutenticacionController(context, GetFakeConfiguration());

            var loginRequest = new LoginRequest { Email = "notfound@example.com", Password = "wrong" };
            var result = await controller.Login(loginRequest);

            var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, unauthorized.StatusCode);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenPasswordIsIncorrect()
        {
            var context = GetInMemoryDbContext();
            var config = GetFakeConfiguration();

            var rol = new Roles { Id = 1, Nombre = "Administrador" };
            var user = new Usuarios
            {
                Nombre = "Test",
                Apellido = "Apiellido1",
                Documento = "12345678",
                TipoDocumento = Models.Enums.TipoDocumentoUsuarioEnum.CC,
                CorreoElectronico = "test@example.com",
                Contrasena = "correct",
                RolId = rol.Id,
                Rol = rol
            };

            context.Roles.Add(rol);
            context.Usuarios.Add(user);
            await context.SaveChangesAsync();

            var controller = new AutenticacionController(context, config);
            var loginRequest = new LoginRequest { Email = "test@example.com", Password = "wrong" };

            var result = await controller.Login(loginRequest);

            var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, unauthorized.StatusCode);
        }

        [Fact]
        public async Task Login_ReturnsBadRequest_WhenModelIsInvalid()
        {
            var context = GetInMemoryDbContext();
            var controller = new AutenticacionController(context, GetFakeConfiguration());
            controller.ModelState.AddModelError("Email", "Requerido");

            var loginRequest = new LoginRequest { Email = "", Password = "" }; // Modelo vacío
            var result = await controller.Login(loginRequest);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }
    }

}
