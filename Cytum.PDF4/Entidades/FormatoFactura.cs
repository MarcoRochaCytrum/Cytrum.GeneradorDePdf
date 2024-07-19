using System.Collections.Generic;

namespace Cytrum.PDF4.Entidades
{
    public class FormatoFactura
    {
        public List<Imagen> Imagenes { get; set; }
        public List<Acrofield> Acrofields { get; set; }
        public string RutaPlantilla { get; set; }
        public AreaDetalle AreaDetalle { get; set; }
        public FormatoFactura()
        {
            Imagenes = new List<Imagen>();
            Acrofields = new List<Acrofield>();
            AreaDetalle = new AreaDetalle();
        }
    }
}