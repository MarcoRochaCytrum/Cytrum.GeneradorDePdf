using System.Collections.Generic;

namespace Cytrum.PDF4.Entidades
{
    public class Configuracion
    {
        public string TextoRelleno { get; set; }
        public List<Acrofield> Acrofields { get; set; }
        public Numerador Numerador { get; set; }
        public Imagen CodigoQr { get; set; }
        public List<Configuracion.PdfAdicional> PdfAdicionales { get; set; }
        public List<HojaCaratula> HojasCaratula { get; set; }
        public List<Detalle> Detalles { get; set; }
        public FormatoFactura UnicaHoja { get; set; }
        public FormatoFactura PrimeraHoja { get; set; }
        public FormatoFactura SegundaHoja { get; set; }
        public FormatoFactura TerceraHoja { get; set; }
        public OrientacionFormato OrientacionFormato { get; set; }
        public int AltoLinea { get; set; }
        public int LineasDespuesDeDetalle { get; set; }
        public string NombreFinal { get; set; }
        public Configuracion()
        {
            Acrofields = new List<Acrofield>();
            HojasCaratula = new List<HojaCaratula>();
            Detalles = new List<Detalle>();
            Numerador = new Numerador();
            CodigoQr = new Imagen();
            HojasCaratula = new List<HojaCaratula>();
            PdfAdicionales = new List<Configuracion.PdfAdicional>();
            OrientacionFormato = new OrientacionFormato();
        }

        public class PdfAdicional
        {
            public int Orden { get; set; }

            public Configuracion Configuracion { get; set; }
        }
    }
}