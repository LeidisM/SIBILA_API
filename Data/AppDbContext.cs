using Microsoft.EntityFrameworkCore;
using SIBILA_API.Models;
using System.Collections.Generic;

namespace SIBILA_API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Libro> Libros { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Roles> Roles { get; set; } // roles en relación
    }
}
