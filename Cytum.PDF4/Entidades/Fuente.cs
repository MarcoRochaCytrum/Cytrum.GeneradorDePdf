using Cytrum.PDF4.Enumeraciones;

namespace Cytrum.PDF4.Entidades
{
    public class Fuente
    {
        public string Nombre { get; set; }
        public string Color { get; set; }
        public int Tamano { get; set; }
        public Alineacion Alineacion { get; set; }
        public TipoLetra TipoLetra { get; set; }
        public Fuente()
        {
            Alineacion = Alineacion.Izquierda;
            TipoLetra = TipoLetra.Normal;
        }
    }
}