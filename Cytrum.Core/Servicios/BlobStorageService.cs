using Cytrum.Core.Enumerations;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using Cytrum.Core.Exceptions;

namespace Cytrum.Core.Servicios
{
    public class BlobStorageService
    {
        private readonly CloudStorageAccount _cloudStorageAccount;
        public BlobStorageService()
        {
            _cloudStorageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString);
        }
        private CloudBlobContainer ObtenerContenedor(string nombreContenedor)
        {
            try
            {
                nombreContenedor = nombreContenedor.PadLeft(3, '0');

                var cliente = _cloudStorageAccount.CreateCloudBlobClient();
                var contenedor = cliente.GetContainerReference(nombreContenedor);

                if (contenedor.CreateIfNotExists())
                {
                    var permisos = contenedor.GetPermissions();
                    permisos.PublicAccess = BlobContainerPublicAccessType.Off;
                    contenedor.SetPermissions(permisos);
                }

                return contenedor;
            }
            catch (Exception ex)
            {
                var mensaje = string.Format(ExceptionMensaje.BlobStorageContenedor.DisplayName, nombreContenedor, Environment.NewLine, ex.ToString());

                throw new BlobStorageException(mensaje);
            }
        }
        public CloudBlockBlob GetBlockBlobReference(string containerName, string blobName)
        {
            var container = ObtenerContenedor(containerName);
            return container.GetBlockBlobReference(blobName);
        }

        public async Task<string> UploadPdfAsync(string containerName, string blobName, Stream stream)
        {
            try
            {
                var blob = GetBlockBlobReference(containerName, blobName);
                blob.Properties.ContentType = "application/pdf";

                await blob.UploadFromStreamAsync(stream);
                await blob.SetPropertiesAsync();

                return blob.Uri.ToString();
            }
            catch (Exception ex)
            {
                var mensaje = string.Format(ExceptionMensaje.BlobStorageSubirArchivo.DisplayName, blobName, containerName, Environment.NewLine, ex.ToString());

                throw new BlobStorageException(mensaje);
            }
        }
    }
}