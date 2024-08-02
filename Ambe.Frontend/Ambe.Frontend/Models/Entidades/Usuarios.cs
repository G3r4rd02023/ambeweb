using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ambe.Frontend.Models.Entidades
{
    public class Usuarios
    {
        public int IdUsuario { get; set; }

        public int IdPersona { get; set; }

        public string Usuario { get; set; } = null!;

        public int IdInstituto { get; set; }

        public string NombreUsuario { get; set; } = null!;

        public string CorreoElectronico { get; set; } = null!;

        public string Contraseña { get; set; } = null!;

        public string Estado { get; set; } = null!;

        public int IdRol { get; set; }

        public DateTime FechaUltimaConexion { get; set; }

        public string CreadoPor { get; set; } = null!;

        public DateTime FechaCreacion { get; set; }

        public string ModificadoPor { get; set; } = null!;

        public DateTime FechaModificacion { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? Roles { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? Estados { get; set; }
    }
}
