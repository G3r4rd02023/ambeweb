namespace Ambe.Frontend.Models
{
    public class AlumnosDTO
    {
        public int IdRegistroViaje { get; set; }
        public int IdViaje { get; set; }
        public int IdPersonaAlumno { get; set; }

        public string NombreAlumno { get; set; } = null!;
        public string CreadoPor { get; set; } = null!;
        public DateTime FechaDeCreacion { get; set; }
    }
}
