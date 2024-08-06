namespace Ambe.Frontend.Models.Entidades
{
    public class BitacoraViaje
    {
        public int IdBitacoraViaje { get; set; }

        public int? IdViaje { get; set; }

        public string Descripcion { get; set; } = null!;

        public int? IdInstituto { get; set; }

        public string CreadoPor { get; set; } = null!;

        public DateTime? FechaDeCreacion { get; set; }

        public string ModificadoPor { get; set; } = null!;

        public DateTime? FechaDeModificacion { get; set; } = null!;
    }
}
