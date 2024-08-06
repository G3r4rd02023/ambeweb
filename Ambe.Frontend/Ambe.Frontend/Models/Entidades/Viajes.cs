using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ambe.Frontend.Models.Entidades
{
    public class Viajes
    {
        public int IdViaje { get; set; }

        public DateTime Fecha { get; set; }

        public string HoraInicio { get; set; } = null!;

        public string HoraFinal { get; set; } = null!;

        public double LatitudActual { get; set; }

        public double LongitudActual { get; set; }

        public int IdPersonaConductor { get; set; }

        public int IdPersonaNinera { get; set; }

        public int IdUnidad { get; set; }

        public int IdInstituto { get; set; }

        public int IdTipoViaje { get; set; }

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

