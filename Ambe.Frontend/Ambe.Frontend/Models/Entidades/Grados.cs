﻿namespace Ambe.Frontend.Models.Entidades
{
    public class Grados
    {
        public int IdGrado { get; set; }

        public int IdInstituto { get; set; }

        public string Grado { get; set; } = null!;

        public string Estado { get; set; } = null!;

        public string CreadoPor { get; set; } = null!;

        public DateTime FechaDeCreacion { get; set; }

        public string ModificadoPor { get; set; } = null!;

        public DateTime FechaDeModificacion { get; set; }
    }
}
