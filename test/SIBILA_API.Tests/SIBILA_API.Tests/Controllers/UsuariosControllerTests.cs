using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using SIBILA_API.Controllers;
using SIBILA_API.Data;
using SIBILA_API.Models;

namespace SIBILA_API.Tests.Controllers
{
    public class UsuariosControllerTests
    {
        private readonly UsuariosController _controller;
        private readonly AppDbContext _context;

        public UsuariosControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            _context.Usuarios.AddRange(
                new Usuarios { 
                    Id = 1, 
                    Nombre = "Usuario1",
                    Apellido = "Apiellido1",
                    CorreoElectronico = "usuario1@correo.com",
                    Documento = "12345678",
                    TipoDocumento = Models.Enums.TipoDocumentoUsuarioEnum.CC,
                    RolId = 1, Rol = new Roles { Id = 1, Nombre = "Admin" } 
                },
                new Usuarios { 
                    Id = 2, 
                    Nombre = "Usuario2",
                    Apellido = "Apiellido2",
                    CorreoElectronico = "usuario2@correo.com",
                    Documento = "23456789",
                    TipoDocumento = Models.Enums.TipoDocumentoUsuarioEnum.CC,
                    RolId = 2, Rol = new Roles { Id = 2, Nombre = "Usuario" } 
                }
            );
            _context.SaveChanges();

            _controller = new UsuariosController(_context);
        }

        [Fact]
        public async Task GetUsuarios_ReturnsAllUsuarios()
        {
            var result = await _controller.GetUsuarios();

            var actionResult = Assert.IsType<ActionResult<IEnumerable<Usuarios>>>(result);
            var returnValue = Assert.IsType<List<Usuarios>>(actionResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetUsuario_ExistingId_ReturnsUsuario()
        {
            // Arrange
            var testId = 1;

            // Act
            var result = await _controller.GetUsuario(testId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Usuarios>>(result);
            var usuario = Assert.IsType<Usuarios>(actionResult.Value);
            Assert.Equal(testId, usuario.Id);
        }

        [Fact]
        public async Task GetUsuario_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var testId = 99;

            // Act
            var result = await _controller.GetUsuario(testId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PostUsuario_ValidUsuario_ReturnsCreatedAtAction()
        {
            var newUsuario = new Usuarios { 
                Id = 3, 
                Nombre = "Usuario3",
                Apellido = "Apiellido3",
                CorreoElectronico = "usuario3@correo.com",
                Documento = "34567891",
                TipoDocumento = Models.Enums.TipoDocumentoUsuarioEnum.CC,
                RolId = 1 
            };

            var result = await _controller.PostUsuario(newUsuario);

            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var usuario = Assert.IsType<Usuarios>(actionResult.Value);
            Assert.Equal(3, usuario.Id);

            var allUsers = _context.Usuarios.ToList();
            Assert.Equal(3, allUsers.Count);
        }

        [Fact]
        public async Task PutUsuario_ValidId_ReturnsNoContent()
        {
            // Arrange
            var existingId = 1;
            var updatedUsuario = new Usuarios
            {
                Id = existingId,
                Nombre = "Usuario1 Actualizado",
                RolId = 1
            };

            _context.ChangeTracker.Clear(); // ← Aquí reseteas el seguimiento

            // Act
            var result = await _controller.PutUsuario(existingId, updatedUsuario);

            // Assert
            Assert.IsType<NoContentResult>(result);

            // Verifica que se haya actualizado
            var usuarioEnDb = await _context.Usuarios.FindAsync(existingId);
            Assert.Equal("Usuario1 Actualizado", usuarioEnDb.Nombre);
        }

        [Fact]
        public async Task PutUsuario_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            var existingId = 1;
            var updatedUsuario = new Usuarios { Id = 2, Nombre = "Usuario1 Actualizado", RolId = 1 };

            // Act
            var result = await _controller.PutUsuario(existingId, updatedUsuario);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteUsuario_ExistingId_ReturnsNoContent()
        {
            // Arrange
            var testId = 1;

            // Act
            var result = await _controller.DeleteUsuario(testId);

            // Assert
            Assert.IsType<NoContentResult>(result);

            // Verifica que el usuario haya sido eliminado de la base de datos
            var usuarioEliminado = await _context.Usuarios.FindAsync(testId);
            Assert.Null(usuarioEliminado);
        }

        [Fact]
        public async Task DeleteUsuario_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var testId = 99;

            // Act
            var result = await _controller.DeleteUsuario(testId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}