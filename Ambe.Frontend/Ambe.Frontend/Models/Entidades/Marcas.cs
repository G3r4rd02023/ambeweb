using System.ComponentModel.DataAnnotations;

namespace Ambe.Frontend.Models.Entidades
{
    public class Marcas
    {
        public int IdMarca { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]        
        public string NombreMarca { get; set; } = null!;

        public string Estado { get; set; } = null!;

        public int IdInstituto { get; set; }

        public string CreadoPor { get; set; } = null!;

        public DateTime FechaDeCreacion { get; set; }

        public string ModificadoPor { get; set; } = null!;

        public DateTime FechaDeModificacion { get; set; }
    }
}
