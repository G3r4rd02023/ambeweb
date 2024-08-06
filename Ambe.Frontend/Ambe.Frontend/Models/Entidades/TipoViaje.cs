namespace Ambe.Frontend.Models.Entidades
{
    public class TipoViaje
    {
        public int IdTipoViaje { get; set; }

        public string Evento { get; set; } = null!;

        public string Descripcion { get; set; } = null!;

        public string CreadoPor { get; set; } = null!;

        public DateTime FechaDeCreacion { get; set; }

        public string ModificadoPor { get; set; } = null!;

        public DateTime FechaDeModificacion { get; set; }
    }
}
