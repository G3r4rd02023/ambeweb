namespace Ambe.Frontend.Models.Entidades
{
    public class Contactos
    {
        public int IdContacto { get; set; }

        public int IdPersona { get; set; }

        public int IdInstituto { get; set; }

        public string Telefono { get; set; } = null!;

        public string Email { get; set; } = null!;

        public int IdTipoContacto { get; set; }

        public string Direccion { get; set; } = null!;

        public string Bloque { get; set; } = null!;

        public string Avenida { get; set; } = null!;

        public string Calle { get; set; } = null!;

        public string Casa { get; set; } = null!;

        public decimal Latitud { get; set; }

        public decimal Longitud { get; set; }

    }
}
