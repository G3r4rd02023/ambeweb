namespace Ambe.Frontend.Models.Entidades
{
    public class Parametros
    {
        public int IdParametro { get; set; }

        public string Parametro { get; set; } = null!;

        public string Valor { get; set; } = null!;

        public int IdUsuario { get; set; }
    }
}
