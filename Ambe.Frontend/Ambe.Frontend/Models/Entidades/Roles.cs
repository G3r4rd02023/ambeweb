namespace Ambe.Frontend.Models.Entidades
{
    public class Roles
    {
        public int IdRol { get; set; }

        public int IdInstituto { get; set; }

        public string Descripcion { get; set; } = null!;

        public string CreadoPor { get; set; } = null!;

        public DateTime FechaCreacion { get; set; }

        public string ModificadoPor { get; set; } = null!;

        public DateTime FechaModificacion { get; set; }
    }
}
