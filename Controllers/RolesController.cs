using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIBILA_API.Data;
using SIBILA_API.Models;

namespace SIBILA_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RolesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Roles>>> GetRoles()
        {
            return await _context.Roles.ToListAsync();
        }       
    }
}
