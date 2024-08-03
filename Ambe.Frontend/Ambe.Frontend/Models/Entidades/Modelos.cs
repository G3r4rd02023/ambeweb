namespace Ambe.Frontend.Models.Entidades
{
    public class Modelos
    {
        public int IdModelo { get; set; }

        public int IdMarca { get; set; }

        public string NombreModelo { get; set; } = null!;

        public string Estado { get; set; } = null!;

        public string CreadoPor { get; set; } = null!;

        public DateTime FechaDeCreacion { get; set; }

        public string ModificadoPor { get; set; } = null!;

        public DateTime FechaDeModificacion { get; set; }
    }
}
