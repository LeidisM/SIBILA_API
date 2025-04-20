using Microsoft.EntityFrameworkCore;
using SIBILA_API.Controllers;
using SIBILA_API.Data;
using SIBILA_API.Models;
using SIBILA_API.Models.Dtos;
using SIBILA_API.Models.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;


namespace SIBILA_API.Tests.Controllers
{
    public class PrestamosControllerTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // DB nueva en cada test
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task CrearPrestamo_ReturnsTrue_WhenValidDto()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new PrestamosController(context);

            var usuario = new Usuarios { Documento = "123", Nombre = "Juan", Apellido = "Pérez", TipoDocumento = TipoDocumentoUsuarioEnum.CC, CorreoElectronico = "test@correo.com", RolId = 1 };
            var libro = new Libro { ISBN = "ISBN123", Titulo = "Libro A", Estado = EstadoLibroEnum.Activo };
            context.Usuarios.Add(usuario);
            context.Libros.Add(libro);
            await context.SaveChangesAsync();

            var dto = new CrearPrestamoDto
            {
                DocumentoUsuario = "123",
                ISBNLibro = "ISBN123"
            };

            // Act
            var result = await controller.CrearPrestamo(dto);

            // Assert
            Assert.True(result);
            Assert.Single(context.Prestamos);
        }

        [Fact]
        public async Task CrearPrestamo_ReturnsFalse_WhenUserNotFound()
        {
            var context = GetInMemoryDbContext();
            var controller = new PrestamosController(context);

            var dto = new CrearPrestamoDto
            {
                DocumentoUsuario = "999",
                ISBNLibro = "ISBN123"
            };

            var result = await controller.CrearPrestamo(dto);
            Assert.False(result);
        }

        [Fact]
        public async Task Devolver_ReturnsTrue_WhenPrestamoExists()
        {
            var context = GetInMemoryDbContext();
            var controller = new PrestamosController(context);

            var usuario = new Usuarios { Documento = "456", Nombre = "Ana", Apellido = "Gómez", TipoDocumento = TipoDocumentoUsuarioEnum.TI, CorreoElectronico = "ana@correo.com", RolId = 2 };
            var libro = new Libro { ISBN = "ISBN456", Titulo = "Libro B", Estado = EstadoLibroEnum.Inactivo };
            context.Usuarios.Add(usuario);
            context.Libros.Add(libro);
            await context.SaveChangesAsync();

            var prestamo = new Prestamos
            {
                UsuarioId = usuario.Id,
                LibroId = libro.Id,
                FechaPrestamo = DateTime.Now
            };
            context.Prestamos.Add(prestamo);
            await context.SaveChangesAsync();

            var result = await controller.Devolver(prestamo.Id);
            var updatedPrestamo = await context.Prestamos.FindAsync(prestamo.Id);

            Assert.True(result);
            Assert.NotNull(updatedPrestamo.FechaDevolucion);
        }

        [Fact]
        public async Task GetPrestamos_ReturnsOnlyActivePrestamos()
        {
            var context = GetInMemoryDbContext();
            var controller = new PrestamosController(context);

            var usuario = new Usuarios { Documento = "0001", Nombre = "Prueba", Apellido = "Activo", TipoDocumento = TipoDocumentoUsuarioEnum.CC, CorreoElectronico = "activo@test.com", RolId = 1 };
            var libro = new Libro { ISBN = "ACTIVO", Titulo = "Libro Activo", Estado = EstadoLibroEnum.Activo };
            context.Usuarios.Add(usuario);
            context.Libros.Add(libro);
            await context.SaveChangesAsync();

            var prestamoActivo = new Prestamos { UsuarioId = usuario.Id, LibroId = libro.Id, FechaPrestamo = DateTime.Now };
            var prestamoDevuelto = new Prestamos { UsuarioId = usuario.Id, LibroId = libro.Id, FechaPrestamo = DateTime.Now, FechaDevolucion = DateTime.Now };
            context.Prestamos.Add(prestamoActivo);
            context.Prestamos.Add(prestamoDevuelto);
            await context.SaveChangesAsync();

            var result = await controller.GetPrestamos();

            Assert.Single(result); // solo 1 sin fecha de devolución
            Assert.Contains(result, p => p.FechaDevolucion == null);
        }

        [Fact]
        public async Task GetDevoluciones_ReturnsOnlyReturnedPrestamos()
        {
            var context = GetInMemoryDbContext();
            var controller = new PrestamosController(context);

            var usuario = new Usuarios { Documento = "0002", Nombre = "Prueba", Apellido = "Devuelto", TipoDocumento = TipoDocumentoUsuarioEnum.CC, CorreoElectronico = "devuelto@test.com", RolId = 1 };
            var libro = new Libro { ISBN = "DEVUELTO", Titulo = "Libro Devuelto", Estado = EstadoLibroEnum.Activo };
            context.Usuarios.Add(usuario);
            context.Libros.Add(libro);
            await context.SaveChangesAsync();

            var prestamoNoDevuelto = new Prestamos { UsuarioId = usuario.Id, LibroId = libro.Id, FechaPrestamo = DateTime.Now };
            var prestamoDevuelto = new Prestamos { UsuarioId = usuario.Id, LibroId = libro.Id, FechaPrestamo = DateTime.Now, FechaDevolucion = DateTime.Now };
            context.Prestamos.Add(prestamoNoDevuelto);
            context.Prestamos.Add(prestamoDevuelto);
            await context.SaveChangesAsync();

            var result = await controller.GetDevoluciones();

            Assert.Single(result); // solo 1 con fecha de devolución
            Assert.Contains(result, p => p.FechaDevolucion != null);
        }

        [Fact]
        public async Task EditarPrestamo_UpdatesPrestamoCorrectly()
        {
            var context = GetInMemoryDbContext();
            var controller = new PrestamosController(context);

            var usuario = new Usuarios { Documento = "321", Nombre = "Carlos", Apellido = "López", TipoDocumento = TipoDocumentoUsuarioEnum.CC, CorreoElectronico = "carlos@correo.com", RolId = 1 };
            var libro1 = new Libro { ISBN = "ISBN1", Titulo = "Libro 1", Estado = EstadoLibroEnum.Activo };
            var libro2 = new Libro { ISBN = "ISBN2", Titulo = "Libro 2", Estado = EstadoLibroEnum.Activo };
            context.Usuarios.Add(usuario);
            context.Libros.AddRange(libro1, libro2);
            await context.SaveChangesAsync();

            var prestamo = new Prestamos
            {
                UsuarioId = usuario.Id,
                LibroId = libro1.Id,
                FechaPrestamo = DateTime.Now
            };
            context.Prestamos.Add(prestamo);
            await context.SaveChangesAsync();

            var dto = new CrearPrestamoDto
            {
                Id = prestamo.Id,
                DocumentoUsuario = usuario.Documento,
                ISBNLibro = libro2.ISBN
            };

            var result = await controller.EditarPrestamo(dto);

            var updated = await context.Prestamos.Include(p => p.Libro).FirstOrDefaultAsync(p => p.Id == prestamo.Id);

            Assert.True(result);
            Assert.Equal("Libro 2", updated.Libro.Titulo);
        }
    }

}
