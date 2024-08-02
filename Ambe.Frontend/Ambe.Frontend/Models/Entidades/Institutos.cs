namespace Ambe.Frontend.Models.Entidades
{
    public class Institutos
    {
        public int IdInstituto { get; set; }

        public string NombreInstituto { get; set; } = null!;

        public string Rtn { get; set; } = null!;

        public string Telefono { get; set; } = null!;

        public string Direccion { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Descripcion { get; set; } = null!;

        public string CreadoPor { get; set; } = null!;

        public DateTime FechaCreacion { get; set; }

        public string ModificadoPor { get; set; } = null!;

        public DateTime FechaModificacion { get; set; }
    }
}
