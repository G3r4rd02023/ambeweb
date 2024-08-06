using System.ComponentModel.DataAnnotations;

namespace Ambe.Frontend.Models.Entidades
{
    public class Rutas
    {
        public int IdRuta { get; set; }

        public int IdInstituto { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]        
        public string NombreRuta { get; set; } = null!;

        [Required(ErrorMessage = "El campo {0} es requerido")]        
        public string Origen { get; set; } = null!;

        [Required(ErrorMessage = "El campo {0} es requerido")]       
        public string Destino { get; set; } = null!;

        [Required(ErrorMessage = "El campo {0} es requerido")]        
        public string DistanciaRecorrida { get; set; } = null!;

        [Required(ErrorMessage = "El campo {0} es requerido")]        
        public string Colonias { get; set; } = null!;

        public string Departamento { get; set; } = null!;

        public string Municipio { get; set; } = null!;

        [Required(ErrorMessage = "El campo {0} es requerido")]       
        public string Descripcion { get; set; } = null!;

        public string Estado { get; set; } = null!;

        public string CreadoPor { get; set; } = null!;

        public DateTime FechaDeCreacion { get; set; }

        public string ModificadoPor { get; set; } = null!;

        public DateTime FechaDeModificacion { get; set; }
    }
}
