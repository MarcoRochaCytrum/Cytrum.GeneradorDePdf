using Cytrum.Core.Servicios;
using Cytrum.PDFGenericoCFDI;
using Microsoft.Azure.WebJobs;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace Cytrum.GeneradorDePDF
{
    public class Functions
    {
        private const string Contenedor = "cfdi";
        private const string Carpeta = "timbrados";
        public async Task ProcessBlob([BlobTrigger(Contenedor + "/" + Carpeta + "/" + "{name}.xml")] Stream myBlob, string name)
        {
            using (myBlob)
            {
                myBlob.Position = 0;

                var cfdiIngresoContenido = new XmlDocument();
                cfdiIngresoContenido.Load(myBlob);

                var generadorFacturaPdf = new GeneradorPdfGenerico();
                var resultado = generadorFacturaPdf.Generar(cfdiIngresoContenido, null, "", null);
                var nombrePdf = $"{Carpeta}/{Path.GetFileNameWithoutExtension(name)}.pdf";

                using (var msPdf = new MemoryStream(resultado.Pdf))
                {
                    var blobStorageService = new BlobStorageService();
                    await blobStorageService.UploadPdfAsync(Contenedor, nombrePdf, msPdf);
                }
            }
        }
    }
}