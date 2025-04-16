using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIBILA_API.Data;
using SIBILA_API.Models;
using SIBILA_API.Models.Dtos;

namespace SIBILA_API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PrestamosController
    {
        private readonly AppDbContext _context;

        public PrestamosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<IEnumerable<Prestamos>> GetPrestamos()
        {
            return await _context.Prestamos.Include(p => p.Usuario).Include(p => p.Libro).Where(x => x.FechaDevolucion == null).ToListAsync();
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<Prestamos> GetPrestamo(int id)
        {
            return await _context.Prestamos.Include(p => p.Usuario).Include(p => p.Libro).FirstOrDefaultAsync(x => x.Id == id);
        }

        [HttpPost("{id}")]        
        public async Task<bool> Devolver(int id)
        {
            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo == null)
            {
                return false;
            }

            // Asignar la fecha actual como fecha de devolución
            prestamo.FechaDevolucion = DateTime.Now;

            try
            {
                _context.Update(prestamo);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return true;
        }

        [HttpPost]
        public async Task<bool> CrearPrestamo(CrearPrestamoDto prestamoDto)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Documento == prestamoDto.DocumentoUsuario);
            if (usuario == null)
            {
                return false;
            }
            var libro = await _context.Libros.FirstOrDefaultAsync(l => l.ISBN == prestamoDto.ISBNLibro);
            if (libro == null)
            {
                return false;
            }

            var prestamo = new Prestamos();
            prestamo.Usuario = usuario;
            prestamo.Libro = libro;
            prestamo.FechaPrestamo = DateTime.Now;

            _context.Prestamos.Add(prestamo);
            await _context.SaveChangesAsync();
            return true;
        }

        [HttpPost]
        public async Task<bool> EditarPrestamo(CrearPrestamoDto prestamoDto)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Documento == prestamoDto.DocumentoUsuario);
            if (usuario == null)
            {
                return false;
            }
            var libro = await _context.Libros.FirstOrDefaultAsync(l => l.ISBN == prestamoDto.ISBNLibro);
            if (libro == null)
            {
                return false;
            }

            var prestamo = await _context.Prestamos.FirstOrDefaultAsync(x => x.Id == prestamoDto.Id);

            if(prestamo == null)
            {
                return false;
            }

            prestamo.Libro = libro;
            prestamo.Usuario = usuario;
            _context.Update(prestamo);
            await _context.SaveChangesAsync();
            return true;
        }

        [HttpGet]
        public async Task<List<Prestamos>> GetDevoluciones()
        {
            return await _context.Prestamos
                .Include(p => p.Usuario)
                .Include(p => p.Libro)
                .Where(p => p.FechaDevolucion != null) // Filtra solo los préstamos devueltos
                .ToListAsync();
        }
    }
}
