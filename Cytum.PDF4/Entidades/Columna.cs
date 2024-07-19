using System.Collections.Generic;

namespace Cytrum.PDF4.Entidades
{
    public class Columna
    {
        public List<RenglonColumna> RenglonesColumna { get; set; }
        public float EspacioX { get; set; }
        public Columna()
        {
            RenglonesColumna = new List<RenglonColumna>();
        }
    }
}