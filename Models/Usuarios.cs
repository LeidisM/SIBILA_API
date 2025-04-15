using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SIBILA_API.Models.Enums;

namespace SIBILA_API.Models
{
    public class Usuarios
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public TipoDocumentoUsuarioEnum TipoDocumento { get; set; }
        public string Documento { get; set; }
       public string CorreoElectronico { get; set; }
        public string? Contrasena { get; set; }
        public int RolId { get; set; }
        [ForeignKey(nameof(RolId))]
        public Roles? Rol{ get; set; }


    }
}
