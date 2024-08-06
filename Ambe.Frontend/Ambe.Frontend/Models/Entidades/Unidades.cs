using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ambe.Frontend.Models.Entidades
{
    public class Unidades
    {
        public int IdUnidad { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]        
        public string NumeroUnidad { get; set; } = null!;

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [RegularExpression(@"^[A-Za-z]{3}\d{4}$", ErrorMessage = "El formato debe ser 3 letras seguidas de 4 números.")]
        public string Placa { get; set; } = null!;

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Solo se permiten letras.")]
        public string Color { get; set; } = null!;

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Solo se permiten números.")]
        public int Capacidad { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Chasis { get; set; } = null!;

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int IdPersonaConductor { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int IdModelo { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int IdMarca { get; set; }

        public int IdInstituto { get; set; }

        public string CreadoPor { get; set; } = null!;

        public DateTime FechaDeCreacion { get; set; }

        public string ModificadoPor { get; set; } = null!;

        public DateTime FechaDeModificacion { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? Marcas { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? Modelos { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? Conductores { get; set; }

    }
}
