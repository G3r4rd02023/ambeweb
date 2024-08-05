namespace Ambe.Frontend.Models
{
    public class UnidadesDTO
    {
        public int IdUnidad { get; set; }

        public string NumeroUnidad { get; set; } = null!;

        public string Placa { get; set; } = null!;

        public int IdPersonaConductor { get; set; }

        public int Capacidad { get; set; }

        public string NombreConductor { get; set; } = null!;

    }
}
