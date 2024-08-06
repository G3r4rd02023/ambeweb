using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ambe.Frontend.Models.Entidades
{
    public class Viajes
    {
        public int IdViaje { get; set; }

        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Solo se permiten números.")]
        public string HoraInicio { get; set; } = null!;

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Solo se permiten números.")]
        public string HoraFinal { get; set; } = null!;

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public double LatitudActual { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public double LongitudActual { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int IdPersonaConductor { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int IdPersonaNinera { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int IdUnidad { get; set; }

        public int IdInstituto { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int IdTipoViaje { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Comentarios { get; set; } = null!;

        public string Estado { get; set; } = null!;

        public string CreadoPor { get; set; } = null!;

        public DateTime FechaDeCreacion { get; set; }

        public string ModificadoPor { get; set; } = null!;

        public DateTime FechaDeModificacion { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? Nineras { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? TipoViajes { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? Conductores { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? Unidades { get; set; }

    }
}

