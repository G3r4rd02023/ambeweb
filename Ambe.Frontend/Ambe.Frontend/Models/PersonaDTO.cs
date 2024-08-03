namespace Ambe.Frontend.Models
{
    public class PersonaDTO 
    {
        public int IdPersona { get; set; }
        public int IdTipoPersona { get; set; }
        public string Descripcion { get; set; } = null!;
        public string NombreCompleto { get; set; } = null!;
        public DateTime FechaCreacion { get; set; }
        public string Estado { get; set; } = null!;
    }
}
