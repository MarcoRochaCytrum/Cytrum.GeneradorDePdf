using System.IO;
using System.Reflection;
using System.Text;

namespace Cytrum.Core.Helper
{
    public class ResursosHelper
    {
        public static FileInfo ObtenerRecursoIncrustado(Assembly executingAssembly, string archivo)
        {
            var rutaArchivo = archivo;
            var fileInfo = new FileInfo(new GetTempFileHelper().Obtener());
            using (var fileStream = new FileStream(fileInfo.FullName, FileMode.Create, FileAccess.Write, FileShare.Write))
            using (var streamWriter = new StreamWriter(fileStream, Encoding.GetEncoding("ISO-8859-1")))
            {
                var streamReader = new StreamReader(executingAssembly.GetManifestResourceStream(rutaArchivo), Encoding.GetEncoding("ISO-8859-1"));
                var value = streamReader.ReadToEnd();
                streamWriter.Write(value);
            }
            return fileInfo;
        }
    }
}