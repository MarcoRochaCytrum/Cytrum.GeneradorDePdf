using System.Collections.Generic;

namespace Cytrum.Core.Model
{
    public class InformacionAdicionalPdfModel
    {
        public InformacionAdicionalPdfModel()
        {
            Conceptos = new List<ModelConcepto>();
        }

        public ModelEmisor Emisor { get; set; }
        public ModelReceptor Receptor { get; set; }
        public List<ModelConcepto> Conceptos { get; set; }
        public string Comentarios { get; set; }
        public class ModelEmisor
        {
            public string Calle { get; set; }
            public string NoExterior { get; set; }
            public string NoInterior { get; set; }
            public string Colonia { get; set; }
            public string Municipio { get; set; }
            public string Estado { get; set; }
            public string CodigoPostal { get; set; }
            public string Pais { get; set; }
        }
        public class ModelReceptor
        {
            public string Calle { get; set; }
            public string NoExterior { get; set; }
            public string NoInterior { get; set; }
            public string Colonia { get; set; }
            public string Municipio { get; set; }
            public string Estado { get; set; }
            public string CodigoPostal { get; set; }
            public string Pais { get; set; }
        }
        public class ModelConcepto
        {
            public ModelConcepto()
            {
                Descripciones = new List<Descripcion>();
            }
            public string DescripcionOriginal { get; set; }
            public List<Descripcion> Descripciones { get; set; }
            public class Descripcion
            {
                public string Linea { get; set; }
            }
            public string ClaveProdServ { get; set; }
            public string NoIdentificacion { get; set; }
            public string Cantidad { get; set; }
            public string Comentario { get; set; }
        }
        public string OrdenVenta { get; set; }
        public string OrdenCompra { get; set; }
        public string Proveedor { get; set; }
        public string RefCliente { get; set; }
        public string Formato { get; set; }
        public string DatosBancarios { get; set; }
        public string CondicinoesDePago { get; set; }
    }
}