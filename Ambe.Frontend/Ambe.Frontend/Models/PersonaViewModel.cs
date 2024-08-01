namespace Ambe.Frontend.Models
{
    public class PersonaViewModel
    {
        public int IdTipoPersona { get; set; }

        public int IdInstituto { get; set; }

        public string PrimerNombre { get; set; } = null!;

        public string SegundoNombre { get; set; } = null!;

        public string PrimerApellido { get; set; } = null!;

        public string SegundoApellido { get; set; } = null!;

        public DateTime FechaNacimiento { get; set; } 

        public string Genero { get; set; } = null!;

        public string Usuario { get; set; } = null!;

        public string NombreUsuario { get; set; } = null!;

        public string CorreoElectronico { get; set; } = null!;

        public string Contraseña { get; set; } = null!;

        public string Estado { get; set; } = null!;

        public int IdRol { get; set; }

        public DateTime FechaUltimaConexion { get; set; }

        public string CreadoPor { get; set; } = null!;

        public string ModificadoPor { get; set; } = null!;
    }
}
