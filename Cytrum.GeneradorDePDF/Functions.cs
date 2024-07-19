using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Cytrum.PDFGenericoCFDI;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace Cytrum.GeneradorDePDF
{
    public class Functions
    {
        private readonly BlobServiceClient _blobServiceClient;

        public Functions(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task ProcessBlob([BlobTrigger("input-container/{name}", Connection = "AzureWebJobsStorage")] BlobClient inputBlob, string name, ILogger log)
        {
            log.LogInformation($"Blob trigger function processed blob\n Name:{name} \n Size: {inputBlob.GetProperties().Value.ContentLength} Bytes");

            var outputContainer = _blobServiceClient.GetBlobContainerClient("output-container");
            var outputBlob = outputContainer.GetBlobClient(name);

            await outputContainer.CreateIfNotExistsAsync(PublicAccessType.Blob);

            using (var stream = new MemoryStream())
            {
                await inputBlob.DownloadToAsync(stream);
                stream.Position = 0; 
                
                var cfdiIngresoContenido = new XmlDocument();
                cfdiIngresoContenido.Load(stream);

                var generadorFacturaPdf = new GeneradorPdfGenerico();
                var resultado = generadorFacturaPdf.Generar(cfdiIngresoContenido, null, "", null);
                var nombrePdf = $"{Path.GetFileNameWithoutExtension(name)}.pdf";

                using(var msPdf = new MemoryStream(resultado.Pdf))
                {
                    await outputBlob.UploadAsync(msPdf, overwrite: true);
                }
            }

            log.LogInformation($"Blob {name} copied to output-container.");
        }
    }
}