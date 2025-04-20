using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIBILA_API.Controllers;
using SIBILA_API.Data;
using SIBILA_API.Models;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace SIBILA_API.Tests.Controllers
{
    public class LibrosControllerTests
    {
        private readonly AppDbContext _context;
        private readonly LibrosController _controller;

        public LibrosControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_Libros")
                .Options;

            _context = new AppDbContext(options);

            // Limpia la DB antes de cada prueba
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            // Seed de datos
            _context.Libros.AddRange(
                new Libro { 
                    Id = 1, 
                    Titulo = "Libro Uno", 
                    Autor = "Autor A", 
                    Editorial = "Editorial A",
                    Estado = Models.Enums.EstadoLibroEnum.Activo,
                    ISBN = "1234567890",
                    Subcategoria = "Subcategoria A",
                    TipoMaterial = "Tipo A"
                },
                new Libro { 
                    Id = 2, 
                    Titulo = "Libro Dos", 
                    Autor = "Autor B",
                    Editorial = "Editorial B",
                    Estado = Models.Enums.EstadoLibroEnum.Activo,
                    ISBN = "2345678901",
                    Subcategoria = "Subcategoria B",
                    TipoMaterial = "Tipo B"
                }
            );
            _context.SaveChanges();

            _controller = new LibrosController(_context);
        }

        [Fact]
        public async Task GetLibros_ReturnsAllLibros()
        {
            var result = await _controller.GetLibros();

            var actionResult = Assert.IsType<ActionResult<IEnumerable<Libro>>>(result);
            var libros = Assert.IsType<List<Libro>>(actionResult.Value);
            Assert.Equal(2, libros.Count);
        }

        [Fact]
        public async Task GetLibro_ExistingId_ReturnsLibro()
        {
            var result = await _controller.GetLibro(1);

            var actionResult = Assert.IsType<ActionResult<Libro>>(result);
            var libro = Assert.IsType<Libro>(actionResult.Value);
            Assert.Equal("Libro Uno", libro.Titulo);
        }

        [Fact]
        public async Task GetLibro_NonExistingId_ReturnsNotFound()
        {
            var result = await _controller.GetLibro(999);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PostLibro_ValidLibro_ReturnsCreatedLibro()
        {
            var nuevoLibro = new Libro
            {
                Titulo = "Libro Nuevo",
                Autor = "Autor X"
            };

            var result = await _controller.PostLibro(nuevoLibro);

            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var libro = Assert.IsType<Libro>(actionResult.Value);
            Assert.Equal("Libro Nuevo", libro.Titulo);

            Assert.Equal(3, _context.Libros.Count());
        }

        [Fact]
        public async Task PutLibro_ValidId_UpdatesLibro()
        {
            var libroActualizado = new Libro
            {
                Id = 1,
                Titulo = "Libro Uno Modificado",
                Autor = "Autor A"
            };

            _context.ChangeTracker.Clear(); // Evita tracking duplicado
            var result = await _controller.PutLibro(1, libroActualizado);

            Assert.IsType<NoContentResult>(result);

            var libroEnDb = await _context.Libros.FindAsync(1);
            Assert.Equal("Libro Uno Modificado", libroEnDb.Titulo);
        }

        [Fact]
        public async Task PutLibro_InvalidId_ReturnsBadRequest()
        {
            var libro = new Libro
            {
                Id = 1,
                Titulo = "Algo",
                Autor = "Alguien"
            };

            var result = await _controller.PutLibro(99, libro);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteLibro_ExistingId_RemovesLibro()
        {
            var result = await _controller.DeleteLibro(1);
            Assert.IsType<NoContentResult>(result);

            var libro = await _context.Libros.FindAsync(1);
            Assert.Null(libro);
        }

        [Fact]
        public async Task DeleteLibro_NonExistingId_ReturnsNotFound()
        {
            var result = await _controller.DeleteLibro(999);
            Assert.IsType<NotFoundResult>(result);
        }
    }

}
