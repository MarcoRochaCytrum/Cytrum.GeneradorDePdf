namespace Cytrum.Core.Enumerations
{
    public class ExceptionMensaje : Enumeration<ExceptionMensaje>
    {
        public static readonly ExceptionMensaje ConexionBaseDatos = new ExceptionMensaje(1, "Ocurrio un error al tratar de conectarse a la Base de Datos: {0}.{1}{2}");
        public static readonly ExceptionMensaje CodificacionUtf8 = new ExceptionMensaje(2, "Ocurrio un error al tratar de leer el archivo en codificación UTF8: {0}.{1}{2}");
        public static readonly ExceptionMensaje ObtenerCredenciales = new ExceptionMensaje(3, "Ocurrio un error al conectarse al sitio: [{0}] con el ApiKey: [{1}].{2}{3}");
        public static readonly ExceptionMensaje ConfiguracionBaseDatos = new ExceptionMensaje(4, "No se encontro la Configuración en la Base de Datos: {0}.");
        public static readonly ExceptionMensaje EjecucionBaseDatos = new ExceptionMensaje(5, "Ocurrio un error al {0} en la Base de Datos: {1}.{2}{3}");
        public static readonly ExceptionMensaje EjecucionCfdiHelper = new ExceptionMensaje(6, "Ocurrio un error al obtener el valor {0}.{1}{2}");
        public static readonly ExceptionMensaje BlobStorageContenedor = new ExceptionMensaje(7, "Ocurrio un error al obtener el contenedor {0}.{1}{2}");
        public static readonly ExceptionMensaje BlobStorageObtenerArchivoBytes = new ExceptionMensaje(8, "Ocurrio un error al obtener el archivo en bytes: {0}, en el contenedor {1}.{2}{3}");
        public static readonly ExceptionMensaje BlobStorageObtenerArchivoTexto = new ExceptionMensaje(9, "Ocurrio un error al obtener el archivo en texto: {0}, en el contenedor {1}.{2}{3}");
        public static readonly ExceptionMensaje BlobStorageObtenerArchivoStream = new ExceptionMensaje(10, "Ocurrio un error al obtener el archivo en stream: {0}, en el contenedor {1}.{2}{3}");
        public static readonly ExceptionMensaje BlobStorageObtenerArchivoXml = new ExceptionMensaje(11, "Ocurrio un error al obtener el archivo en Xml: {0}, en el contenedor {1}.{2}{3}");
        public static readonly ExceptionMensaje BlobStorageSubirArchivo = new ExceptionMensaje(12, "Ocurrio un error al subir el archivo: {0}, en el contenedor {1}.{2}{3}");
        public static readonly ExceptionMensaje BlobStorageEliminarArchivo = new ExceptionMensaje(13, "Ocurrio un error al eliminar el archivo: {0}, en el contenedor {1}.{2}{3}");
        public static readonly ExceptionMensaje TableStorageObtenerReferencia = new ExceptionMensaje(14, "Ocurrio un error al obtener la referencia a la tabla: {0}.{1}{2}");
        public static readonly ExceptionMensaje TableStorageConsultar = new ExceptionMensaje(15, "Ocurrio un error al obtener la consulta: {0}.{1}{2}");
        public static readonly ExceptionMensaje TableStorageInsertar = new ExceptionMensaje(16, "Ocurrio un error al guardar en la tabla: {0}.{1}{2}");
        public static readonly ExceptionMensaje TableStorageEliminar = new ExceptionMensaje(17, "Ocurrio un error al eliminar en la tabla: {0}.{1}{2}");
        public static readonly ExceptionMensaje QueueStorageObtenerReferencia = new ExceptionMensaje(18, "Ocurrio un error al obtener la referencia a la cola: {0}.{1}{2}");
        public static readonly ExceptionMensaje QueueStorageAgregarMensaje = new ExceptionMensaje(19, "Ocurrio un error al agregar mensaje a la cola: {0}.{1}{2}");
        public static readonly ExceptionMensaje CadenaOriginalTimbreMensaje = new ExceptionMensaje(20, "Ocurrio un error al generar la cadena original del timbre.{0}{1}");
        public static readonly ExceptionMensaje CadenaOriginalMensaje = new ExceptionMensaje(21, "Ocurrio un error al generar la cadena original.{0}{1}");
        public static readonly ExceptionMensaje SelladoMensaje = new ExceptionMensaje(22, "Ocurrio un error al generar el sello para el comprobante.{0}{1}");
        public static readonly ExceptionMensaje SelladoCargaXsltMensaje = new ExceptionMensaje(23, "Ocurrio un error al cargar los archivos para generación de cadena original[XSLT].{0}{1}");
        public static readonly ExceptionMensaje ValidarXmlEstructuraDetalle = new ExceptionMensaje(24, "La estructura del XML no es correcta, {0}.");
        private ExceptionMensaje(short value, string displayName) : base(value, displayName)
        {
        }
    }
}