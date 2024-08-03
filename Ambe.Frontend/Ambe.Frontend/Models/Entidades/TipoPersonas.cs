namespace Ambe.Frontend.Models.Entidades
{
    public class TipoPersonas
    {
        public int IdTipoPersona { get; set; }

        public int IdInstituto { get; set; }

        public string TipoPersona { get; set; } = null!;

        public string Descripcion { get; set; } = null!;

        public string CreadoPor { get; set; } = null!;

        public DateTime FechaCreacion { get; set; }

        public string ModificadoPor { get; set; } = null!;

        public DateTime FechaModificacion { get; set; }

        public string Estado { get; set; } = null!;
    }
}
