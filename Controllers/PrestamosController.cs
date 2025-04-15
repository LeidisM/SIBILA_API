using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIBILA_API.Data;
using SIBILA_API.Models;

namespace SIBILA_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

        [HttpPost]        
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

        //// POST: api/Usuarios
        //[HttpPost]
        //public async Task<ActionResult<Usuarios>> PostPrestamo(Prestamos usuario)
        //{
        //    var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Documento == prestamoVm.Documento);
        //    if (usuario == null)
        //    {
        //        TempData["ErrorMessage"] = "Usuario no encontrado.";
        //        return View(prestamoVm);
        //    }

        //    // Busca el libro en la base de datos según su ISBN.
        //    var libro = await _context.Libros.FirstOrDefaultAsync(l => l.ISBN == prestamoVm.ISBN);
        //    if (libro == null)
        //    {
        //        TempData["ErrorMessage"] = "Libro no encontrado.";
        //    }
        //    // Crea un nuevo préstamo con la fecha actual.
        //    var prestamo = new Prestamos();
        //    prestamo.Usuario = usuario;
        //    prestamo.Libro = libro;
        //    prestamo.FechaPrestamo = DateTime.Now;

        //    _context.Prestamos.Add(prestamo);
        //    await _context.SaveChangesAsync();
        //    return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
        //}

        //// PUT: api/Usuarios/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutUsuario(int id, Usuarios usuario)
        //{
        //    if (id != usuario.Id)
        //        return BadRequest();

        //    _context.Entry(usuario).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!_context.Usuarios.Any(e => e.Id == id))
        //            return NotFound();
        //        else
        //            throw;
        //    }

        //    return NoContent();
        //}
    }
}
