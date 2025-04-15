using System.ComponentModel.DataAnnotations.Schema;

namespace SIBILA_API.Models
{
    public class Prestamos
    {
        public int Id { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public DateTime? FechaDevolucion { get; set; }
        public int UsuarioId { get; set; }
        [ForeignKey(nameof(UsuarioId))]
        public Usuarios Usuario { get; set; }
        public int LibroId { get; set; }
        [ForeignKey(nameof(LibroId))]
        public Libro Libro { get; set; }
    }
}
