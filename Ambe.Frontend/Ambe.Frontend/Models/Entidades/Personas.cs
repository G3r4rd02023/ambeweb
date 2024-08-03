namespace Ambe.Frontend.Models.Entidades
{
    public class Personas
    {
        public int IdPersona { get; set; }

        public int IdTipoPersona { get; set; }

        public int IdInstituto { get; set; }

        public string PrimerNombre { get; set; } = null!;

        public string SegundoNombre { get; set; } = null!;

        public string PrimerApellido { get; set; } = null!;

        public string SegundoApellido { get; set; } = null!;

        public DateTime FechaNacimiento { get; set; } 

        public string Genero { get; set; } = null!;

        public string CreadoPor { get; set; } = null!;

        public DateTime FechaCreacion { get; set; }

        public string ModificadoPor { get; set; } = null!;

        public DateTime FechaModificacion { get; set; }

        public string Estado { get; set; } = null!;

        public string NombreCompleto => $"{PrimerNombre} {PrimerApellido}";
    }
}
