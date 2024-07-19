using System.Collections.Generic;

namespace Cytrum.PDF4.Entidades
{
    public class Detalle
    {
        public List<Columna> Columnas { get; set; }
        public float EspacioX { get; set; }
        public Detalle()
        {
            Columnas = new List<Columna>();
        }
    }
}