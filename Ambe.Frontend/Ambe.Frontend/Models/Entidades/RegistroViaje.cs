namespace Ambe.Frontend.Models.Entidades
{
    public class RegistroViaje
    {
        public int IdRegistroViaje { get; set; }

        public int IdPersonaAlumno { get; set; }

        public int IdRuta { get; set; }

        public int IdViaje { get; set; }

        public int IdInstituto { get; set; }

        public string Observaciones { get; set; } = null!;

        public string CreadoPor { get; set; } = null!;

        public DateTime FechaDeCreacion { get; set; }

        public string ModificadoPor { get; set; } = null!;

        public DateTime FechaDeModificacion { get; set; }
    }
}
