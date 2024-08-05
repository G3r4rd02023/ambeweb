using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ambe.Frontend.Models.Entidades
{
    public class Unidades
    {
        public int IdUnidad { get; set; }

        public string NumeroUnidad { get; set; } = null!;

        public string Placa { get; set; } = null!;

        public string Color { get; set; } = null!;

        public int Capacidad { get; set; }

        public string Chasis { get; set; } = null!;

        public int IdPersonaConductor { get; set; }

        public int IdModelo { get; set; }

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
