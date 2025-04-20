using Microsoft.EntityFrameworkCore;
using SIBILA_API.Models;
using System.Collections.Generic;

namespace SIBILA_API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public virtual DbSet<Libro> Libros { get; set; }
        public virtual DbSet<Usuarios> Usuarios { get; set; }
        public virtual DbSet<Roles> Roles { get; set; } // roles en relación
        public virtual DbSet<Prestamos> Prestamos { get; set; } // prestamos en relación
    }
}
