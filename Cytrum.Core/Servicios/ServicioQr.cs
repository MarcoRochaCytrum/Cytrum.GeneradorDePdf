using Cytrum.Core.Helper;
using System.IO;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System.Drawing.Imaging;
using System.Drawing;

namespace Cytrum.Core.Servicios
{
    public class ServicioQr
    {
        public static string RutaQrGenerado(decimal total, string uuid, string rfcEmisor, string rfcReceptor, string sello)
        {
            var rutaValidarCfdi = "https://verificacfdi.facturaelectronica.sat.gob.mx/default.aspx";
            var pathQr = new GetTempFileHelper().Obtener();
            var totalStr = total.ToString("##################.######");
            if (string.IsNullOrEmpty(totalStr))
                totalStr = "0.000000";

            sello = sello.Substring(sello.Length - 8);
            var contenidoQr = string.Format("{0}?id={1}&re={2}&rr={3}&tt={4}&fe={5}", rutaValidarCfdi, uuid.ToUpper(), rfcEmisor, rfcReceptor, totalStr, sello);
            CrearQr(contenidoQr, pathQr);

            return pathQr;
        }

        private static void CrearQr(string texto, string path)
        {
            var qrEncoder = new QrEncoder(ErrorCorrectionLevel.M);
            var qrCode = qrEncoder.Encode(texto);
            var renderer = new GraphicsRenderer(new FixedModuleSize(5, QuietZoneModules.Zero), Brushes.Black, Brushes.White);

            using (var stream = new FileStream(path, FileMode.Append))
            {
                renderer.WriteToStream(qrCode.Matrix, ImageFormat.Gif, stream);
            }
        }
    }
}