using System;
using System.IO;

namespace Cytrum.Core.Helper
{
    public class GetTempFileHelper
    {
        public string CarpetaTemp { get; set; }

        public GetTempFileHelper()
        {
            lock (this)
            {
                CarpetaTemp = Path.Combine(Path.GetTempPath(), "CytrumTmp");
                if (!Directory.Exists(CarpetaTemp))
                    Directory.CreateDirectory(CarpetaTemp);
            }
        }

        public string Obtener()
        {
            lock (this)
            {
                var ruta = string.Empty;
                do
                {
                    try
                    {
                        var nombreCarpeta = DateTime.Now.Year + "_" + DateTime.Now.Month + "_";
                        var carpetaHoy = Path.Combine(CarpetaTemp, nombreCarpeta + DateTime.Now.Day);


                        var carpetas = new DirectoryInfo(CarpetaTemp).GetDirectories();
                        foreach (var carpeta in carpetas)
                        {
                            var año = Convert.ToInt32(carpeta.Name.Split('_')[0]);
                            var mes = Convert.ToInt32(carpeta.Name.Split('_')[1]);
                            var dia = Convert.ToInt32(carpeta.Name.Split('_')[2]);
                            var fechaCarpeta = new DateTime(año, mes, dia);
                            if (fechaCarpeta <= DateTime.Today.AddDays(-2) && Directory.Exists(carpeta.FullName))
                            {
                                Directory.Delete(carpeta.FullName, true);
                            }
                        }


                        if (!Directory.Exists(carpetaHoy))
                            Directory.CreateDirectory(carpetaHoy);

                        ruta = Path.Combine(carpetaHoy, Guid.NewGuid() + "tmp");
                    }
                    catch (Exception)
                    {
                    }
                } while (string.IsNullOrEmpty(ruta));

                return ruta;
            }
        }
    }
}