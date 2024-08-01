namespace Ambe.Frontend.Models.Entidades
{
    public class Bitacora
    {
        public int IdBitacora { get; set; }
        
        public int IdUsuario { get; set; }

        public int IdInstituto { get; set; }

        public string TipoAccion { get; set; } = null!;

        public string Tabla { get; set; } = null!;

        public DateTime Fecha { get; set; }
    }
}
