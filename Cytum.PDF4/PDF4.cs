using System;
using System.IO;
using System.Linq;
using System.Threading;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.pdf;
using Cytrum.PDF4.Entidades;
using Cytrum.PDF4.Enumeraciones;
using System.Drawing;

namespace Cytrum.PDF4
{
    public class Pdf4
    {
        private string _rutaTemporalDocumentoIText = "";
        private Document _documentoIText;
        private PdfWriter _pdfWriter;

        public Pdf4()
        {
            _rutaTemporalDocumentoIText = Path.GetTempFileName();
        }

        public void Dibujar(Configuracion configuracion, bool numerarPagina = true)
        {

            _documentoIText = configuracion.OrientacionFormato == null || !configuracion.OrientacionFormato.Horizontal || configuracion.OrientacionFormato.Vertical ? new Document(PageSize.LETTER) : new Document(new iTextSharp.text.Rectangle(792f, 612f, 360));
            _pdfWriter = PdfWriter.GetInstance(_documentoIText, new FileStream(_rutaTemporalDocumentoIText, FileMode.Create));

            var relacionDetalles = ContadorPaginas.ObtenerRelacionDetalle(configuracion);
            var formatoAUtilizar = relacionDetalles.Count == 1 ? TipoFormato.UnaHoja : (relacionDetalles.Count == 2 ? TipoFormato.DosHojas : TipoFormato.MultiHoja);

            _documentoIText.Open();

            foreach (var hojaCaratula in configuracion.HojasCaratula.Where(a => a.LugarInsercion == LugarInsercion.Inicio))
            {
                AgregarHojaCaratula(hojaCaratula.RutaPlantilla);
            }
            var coordenadaYDetalle = CrearFormato(configuracion, relacionDetalles.Count == 1 ? 1 : 2);
            var maxCoordenadaYDetalle = 0.0f;
            var contadorPaginas = 1;
            var contadorDetalles = 1;
            foreach (var detalle in configuracion.Detalles)
            {
                foreach (var columna in detalle.Columnas)
                {
                    var renglonColorFila = columna.RenglonesColumna.FirstOrDefault(x => x.ColorFila != null);
                    if(renglonColorFila != null)
                    {
                        var cbu = _pdfWriter.DirectContentUnder;
                        cbu.SaveState();
                        cbu.SetColorFill(WebColors.GetRGBColor(renglonColorFila.ColorFila.ColorHex));
                        cbu.SetColorStroke(WebColors.GetRGBColor(renglonColorFila.ColorFila.ColorHex));
                        cbu.Rectangle(renglonColorFila.ColorFila.PosicionX, CambioY(coordenadaYDetalle) - renglonColorFila.ColorFila.PosicionY, renglonColorFila.ColorFila.Ancho, renglonColorFila.ColorFila.Alto);
                        cbu.Fill();
                        cbu.RestoreState();
                    }

                    var inicioRenglon = coordenadaYDetalle;
                    foreach (var renglon in columna.RenglonesColumna)
                    {
                        var nombreFuente = "Courier";
                        if (renglon.Fuente.Nombre != null)
                            nombreFuente = renglon.Fuente.Nombre;

                        var tamanoFuente = 10;
                        if (renglon.Fuente.Tamano != 0)
                            tamanoFuente = renglon.Fuente.Tamano;

                        var color = Color.Black;
                        if (renglon.Fuente.Color != null)
                            color = Color.FromName(renglon.Fuente.Color);

                        if (renglon.MaximoNumeroDeCaracteres > 0 && renglon.Texto.Length <= renglon.MaximoNumeroDeCaracteres)
                        {
                            if (renglon.ColorFila != null)
                            {
                                var cbu = _pdfWriter.DirectContentUnder;
                                cbu.SaveState();
                                cbu.SetColorFill(WebColors.GetRGBColor(renglon.ColorFila.ColorHex));
                                cbu.SetColorStroke(WebColors.GetRGBColor(renglon.ColorFila.ColorHex));
                                cbu.Rectangle(renglon.ColorFila.PosicionX, CambioY(coordenadaYDetalle) - configuracion.AltoLinea / 2, renglon.ColorFila.Ancho, renglon.ColorFila.Alto);
                                cbu.Fill();
                                cbu.RestoreState();
                            }

                            var cb = _pdfWriter.DirectContent;
                            cb.SetColorFill(new BaseColor(color));

                            BaseFont bf;
                            if (nombreFuente == "Courier")
                            {
                                bf = BaseFont.CreateFont(nombreFuente, "Cp1252", false);
                                cb.SetFontAndSize(bf, tamanoFuente);
                            }
                            else
                            {
                                bf = BaseFont.CreateFont(nombreFuente, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                                cb.SetFontAndSize(bf, tamanoFuente);
                            }

                            if (renglon.Fuente.TipoLetra == TipoLetra.Normal)
                            {
                                cb.SetLineWidth(0.2);
                                cb.SetTextRenderingMode(PdfContentByte.TEXT_RENDER_MODE_FILL);
                            }
                            if (renglon.Fuente.TipoLetra == TipoLetra.Bold)
                            {
                                cb.SetLineWidth(0.4);
                                cb.SetTextRenderingMode(PdfContentByte.TEXT_RENDER_MODE_FILL_STROKE);
                            }

                            cb.BeginText();
                            cb.ShowTextAligned((int)renglon.Fuente.Alineacion, renglon.Texto, columna.EspacioX + renglon.EspacioX, CambioY(coordenadaYDetalle), 0);
                            cb.EndText();

                            coordenadaYDetalle += configuracion.AltoLinea;
                        }
                        else
                        {
                            if (renglon.CortarPalabrasCompletas)
                            {
                                var indiceInicial = 0;
                                var indiceFinal = renglon.MaximoNumeroDeCaracteres;

                                while (indiceInicial < renglon.Texto.Length - 1)
                                {
                                    var cb = _pdfWriter.DirectContent;
                                    cb.SetColorFill(new BaseColor(color));

                                    BaseFont bf;
                                    if (nombreFuente == "Courier")
                                    {
                                        bf = BaseFont.CreateFont(nombreFuente, "Cp1252", false);
                                        cb.SetFontAndSize(bf, tamanoFuente);
                                    }
                                    else
                                    {
                                        bf = BaseFont.CreateFont(nombreFuente, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                                        cb.SetFontAndSize(bf, tamanoFuente);
                                    }

                                    cb.SetFontAndSize(bf, tamanoFuente);

                                    if (renglon.Fuente.TipoLetra == TipoLetra.Normal)
                                    {
                                        cb.SetLineWidth(0.2);
                                        cb.SetTextRenderingMode(PdfContentByte.TEXT_RENDER_MODE_FILL);
                                    }
                                    if (renglon.Fuente.TipoLetra == TipoLetra.Bold)
                                    {
                                        cb.SetLineWidth(0.4);
                                        cb.SetTextRenderingMode(PdfContentByte.TEXT_RENDER_MODE_FILL_STROKE);
                                    }

                                    cb.BeginText();

                                    if (indiceFinal > renglon.Texto.Length - 1)
                                    {
                                        indiceFinal = renglon.Texto.Length;
                                    }
                                    else
                                    {
                                        while (renglon.Texto[indiceFinal].ToString() != " ")
                                        {
                                            indiceFinal--;
                                        }
                                    }

                                    var textoRenglon = new string(renglon.Texto.Skip(indiceInicial).Take(indiceFinal - indiceInicial).ToArray());

                                    cb.ShowTextAligned((int)renglon.Fuente.Alineacion, textoRenglon, columna.EspacioX + renglon.EspacioX, CambioY(coordenadaYDetalle), 0);
                                    indiceInicial = indiceFinal + 1;
                                    indiceFinal += renglon.MaximoNumeroDeCaracteres + 1;

                                    cb.EndText();

                                    coordenadaYDetalle += configuracion.AltoLinea;
                                }
                            }
                            else
                            {
                                var numeroRenglones = renglon.Texto.Length / (float)renglon.MaximoNumeroDeCaracteres;
                                numeroRenglones = (numeroRenglones / (int)numeroRenglones) == 1 ? numeroRenglones : (float)(Math.Floor(numeroRenglones) + 1);

                                for (var i = 0; i < numeroRenglones; i++)
                                {
                                    var cb = _pdfWriter.DirectContent;
                                    cb.SetColorFill(new BaseColor(color));

                                    BaseFont bf;
                                    if (nombreFuente == "Courier")
                                    {
                                        bf = BaseFont.CreateFont(nombreFuente, "Cp1252", false);
                                        cb.SetFontAndSize(bf, tamanoFuente);
                                    }
                                    else
                                    {
                                        bf = BaseFont.CreateFont(nombreFuente, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                                        cb.SetFontAndSize(bf, tamanoFuente);
                                    }

                                    cb.SetFontAndSize(bf, tamanoFuente);

                                    if (renglon.Fuente.TipoLetra == TipoLetra.Normal)
                                    {
                                        cb.SetLineWidth(0.2);
                                        cb.SetTextRenderingMode(PdfContentByte.TEXT_RENDER_MODE_FILL);
                                    }
                                    if (renglon.Fuente.TipoLetra == TipoLetra.Bold)
                                    {
                                        cb.SetLineWidth(0.4);
                                        cb.SetTextRenderingMode(PdfContentByte.TEXT_RENDER_MODE_FILL_STROKE);
                                    }

                                    cb.BeginText();

                                    cb.ShowTextAligned((int)renglon.Fuente.Alineacion, new string(renglon.Texto.Skip(i * renglon.MaximoNumeroDeCaracteres).Take(renglon.MaximoNumeroDeCaracteres).ToArray()), columna.EspacioX + renglon.EspacioX, CambioY(coordenadaYDetalle), 0);

                                    cb.EndText();
                                    coordenadaYDetalle += configuracion.AltoLinea;
                                }
                            }
                        }
                    }
                    if (coordenadaYDetalle > (double)maxCoordenadaYDetalle)
                        maxCoordenadaYDetalle = coordenadaYDetalle;

                    coordenadaYDetalle = inicioRenglon;
                }

                coordenadaYDetalle = maxCoordenadaYDetalle + configuracion.LineasDespuesDeDetalle * configuracion.AltoLinea;

                if (relacionDetalles[contadorPaginas] == contadorDetalles)
                {
                    #region TextoRelleno
                    if (contadorDetalles == configuracion.Detalles.Count)
                    {
                        FormatoFactura hojaUtilizada = null;
                        if (formatoAUtilizar == TipoFormato.DosHojas)
                        {
                            hojaUtilizada = contadorPaginas == 1
                                ? configuracion.PrimeraHoja
                                : configuracion.TerceraHoja;
                        }
                        else
                        {
                            hojaUtilizada = configuracion.TerceraHoja;
                        }
                        var numeroRenglonesLibres = (hojaUtilizada.AreaDetalle.CoordenadaYFin - coordenadaYDetalle) / configuracion.AltoLinea;
                        var cb = _pdfWriter.DirectContent;

                        var bf = BaseFont.CreateFont("Courier", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        cb.SetFontAndSize(bf, configuracion.AltoLinea / 2);
                        cb.BeginText();
                        for (var i = 0; i < numeroRenglonesLibres; i++)
                        {
                            cb.ShowTextAligned(1, configuracion.TextoRelleno, _documentoIText.PageSize.Width / 2,
                                CambioY(coordenadaYDetalle + (i * configuracion.AltoLinea)), 0);

                        }
                        cb.EndText();


                    }
                    #endregion

                    if (relacionDetalles.Count != contadorPaginas)
                    {
                        #region TextoRelleno si es caso especial

                        if (contadorDetalles == configuracion.Detalles.Count)
                        {
                            FormatoFactura hojaUtilizada = null;
                            if (formatoAUtilizar == TipoFormato.DosHojas)
                            {
                                hojaUtilizada = contadorPaginas == 1
                                    ? configuracion.PrimeraHoja
                                    : configuracion.TerceraHoja;
                            }
                            else
                            {
                                hojaUtilizada = configuracion.SegundaHoja;
                            }
                            var numeroRenglonesLibres = (hojaUtilizada.AreaDetalle.CoordenadaYFin -
                                                         coordenadaYDetalle) / configuracion.AltoLinea;
                            var cb = _pdfWriter.DirectContent;

                            var bf = BaseFont.CreateFont("Courier", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                            cb.SetFontAndSize(bf, configuracion.AltoLinea / 2);
                            cb.BeginText();

                            for (var i = 0; i < numeroRenglonesLibres; i++)
                            {
                                cb.ShowTextAligned(1, configuracion.TextoRelleno, _documentoIText.PageSize.Width / 2,
                                    CambioY(coordenadaYDetalle + (i * configuracion.AltoLinea)), 0);

                            }
                            cb.EndText();
                        }
                        #endregion

                        coordenadaYDetalle = CrearFormato(configuracion, contadorPaginas == relacionDetalles.Count() - 1 ? 4 : 3);

                        #region TextoRlleno hoja en blanco

                        if (contadorDetalles == configuracion.Detalles.Count && relacionDetalles.Last().Value == 0)
                        {
                            FormatoFactura hojaUtilizada;
                            hojaUtilizada = configuracion.TerceraHoja;

                            var numeroRenglonesLibres = (hojaUtilizada.AreaDetalle.CoordenadaYFin - hojaUtilizada.AreaDetalle.CoordenadaYInicio) / configuracion.AltoLinea;
                            var cb = _pdfWriter.DirectContent;
                            var bf = BaseFont.CreateFont("Courier", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

                            cb.SetFontAndSize(bf, configuracion.AltoLinea / 2);
                            cb.BeginText();

                            for (var i = 0; i < numeroRenglonesLibres; i++)
                            {
                                cb.ShowTextAligned(1, configuracion.TextoRelleno, _documentoIText.PageSize.Width / 2, CambioY(coordenadaYDetalle + (i * configuracion.AltoLinea)), 0);
                            }
                            cb.EndText();
                        }
                        #endregion
                        maxCoordenadaYDetalle = 0.0f;
                        ++contadorPaginas;
                    }
                }
                ++contadorDetalles;
            }
            foreach (var hojaCaratula in configuracion.HojasCaratula.Where(a => a.LugarInsercion == LugarInsercion.Final))
                AgregarHojaCaratula(hojaCaratula.RutaPlantilla);

            _documentoIText.Close();
            InsertarNumeroPagina(configuracion, string.Empty, numerarPagina);
        }

        public void InsertarNumeroPagina(Configuracion configuracion, string rutaFinalArchivo = "", bool numerarPagina = true)
        {
            try
            {
                if (!string.IsNullOrEmpty(rutaFinalArchivo))
                    _rutaTemporalDocumentoIText = rutaFinalArchivo;

                var rotation = 0.0f;
                if (configuracion.OrientacionFormato != null && configuracion.OrientacionFormato.Horizontal && !configuracion.OrientacionFormato.Vertical)
                    rotation = 360f;

                var reader = new PdfReader(_rutaTemporalDocumentoIText);
                var numberOfPages = reader.NumberOfPages;
                var numberOfPagesCaratulaFin = configuracion.HojasCaratula.Count(x => x.LugarInsercion == LugarInsercion.Final && x.Numerador != null);
                var document = new Document(reader.GetPageSize(1), 50f, 50f, 50f, 50f);
                var instance = PdfWriter.GetInstance(document, new FileStream(configuracion.NombreFinal, FileMode.Create));
                document.Open();

                var directContent = instance.DirectContent;
                var location = 1;
                while (location <= numberOfPages - numberOfPagesCaratulaFin)
                {
                    document.NewPage();
                    var importedPage = instance.GetImportedPage(reader, location);
                    directContent.AddTemplate(importedPage, 0.0f, 0.0f);
                    directContent = instance.DirectContent;
                    directContent.BeginText();

                    if (configuracion.Numerador != null & numerarPagina)
                    {
                        var str = "Courier";
                        if (!string.IsNullOrEmpty(configuracion.Numerador.Fuente.Nombre))
                        {
                            if (!FontFactory.IsRegistered(configuracion.Numerador.Fuente.Nombre))
                                FontFactory.Register(Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\" + configuracion.Numerador.Fuente.Nombre + ".ttf");
                            str = configuracion.Numerador.Fuente.Nombre;
                        }
                        var bf = !(str == "Courier") ? FontFactory.GetFont(str, "Identity-H", true).BaseFont : BaseFont.CreateFont(str, "Cp1252", false);
                        if (configuracion.Numerador.Fuente.Tamano == 0)
                            configuracion.Numerador.Fuente.Tamano = 10;

                        directContent.SetFontAndSize(bf, configuracion.Numerador.Fuente.Tamano);

                        if (!configuracion.Numerador.EsNumeracionLarga)
                            directContent.ShowTextAligned(0, location.ToString(), configuracion.Numerador.Coordenadas.X, CambioY(configuracion.Numerador.Coordenadas.Y), rotation);
                        else
                            directContent.ShowTextAligned(0, location + configuracion.Numerador.Separador + numberOfPages, configuracion.Numerador.Coordenadas.X, CambioY(configuracion.Numerador.Coordenadas.Y), rotation);
                    }

                    Math.Min(Interlocked.Increment(ref location), location - 1);
                    directContent.EndText();
                }

                if (configuracion.Numerador != null && numberOfPagesCaratulaFin > 0)
                {
                    var locationCaratula = 0;
                    while (location <= numberOfPages)
                    {
                        var caratulaCooredanas = configuracion.HojasCaratula[locationCaratula].Numerador.Coordenadas;

                        location = NumerarPagina(configuracion, numerarPagina, document, instance, reader, location, caratulaCooredanas.X, caratulaCooredanas.Y, rotation, numberOfPages, ref directContent);

                        locationCaratula++;
                    }
                }

                document.Close();
                reader.Close();

                File.Delete(_rutaTemporalDocumentoIText);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        private int NumerarPagina(Configuracion configuracion, bool numerarPagina, Document document, PdfWriter instance, PdfReader reader, int location, float coordenadaPadreX, float coordenadaPadreY, float rotation, int numberOfPages, ref PdfContentByte directContent)
        {
            document.NewPage();
            var importedPage = instance.GetImportedPage(reader, location);
            directContent.AddTemplate(importedPage, 0.0f, 0.0f);
            directContent = instance.DirectContent;
            directContent.BeginText();

            if (configuracion.Numerador != null & numerarPagina)
            {
                var str = "Courier";
                if (!string.IsNullOrEmpty(configuracion.Numerador.Fuente.Nombre))
                {
                    if (!FontFactory.IsRegistered(configuracion.Numerador.Fuente.Nombre))
                        FontFactory.Register(Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\" + configuracion.Numerador.Fuente.Nombre + ".ttf");
                    str = configuracion.Numerador.Fuente.Nombre;
                }
                var bf = !(str == "Courier") ? FontFactory.GetFont(str, "Identity-H", true).BaseFont : BaseFont.CreateFont(str, "Cp1252", false);
                if (configuracion.Numerador.Fuente.Tamano == 0)
                    configuracion.Numerador.Fuente.Tamano = 10;

                directContent.SetFontAndSize(bf, configuracion.Numerador.Fuente.Tamano);

                if (!configuracion.Numerador.EsNumeracionLarga)
                    directContent.ShowTextAligned(0, location.ToString(), coordenadaPadreX, CambioY(coordenadaPadreY), rotation);
                else
                    directContent.ShowTextAligned(0, location + configuracion.Numerador.Separador + numberOfPages, coordenadaPadreX, CambioY(coordenadaPadreY), rotation);
            }

            Math.Min(Interlocked.Increment(ref location), location - 1);
            directContent.EndText();
            return location;
        }

        private void AgregarHojaCaratula(string plantillaFormato)
        {
            var str = Path.GetTempFileName();
            File.Copy(plantillaFormato, str, true);
            var reader1 = new PdfReader(new FileInfo(str).FullName);
            new PdfStamper(reader1, new FileStream(str + "tmp", FileMode.Create))
            {
                FormFlattening = true
            }.Close();
            var reader2 = new PdfReader(str + "tmp");
            var directContent = _pdfWriter.DirectContent;

            var numeroDePaginas1 = reader1.NumberOfPages;
            var numeroDePaginas2 = reader2.NumberOfPages;

            for (int pagina = 1; pagina <= numeroDePaginas2; pagina++)
            {
                _documentoIText.NewPage();
                var importedPage = _pdfWriter.GetImportedPage(reader2, pagina);
                switch (reader2.GetPageRotation(1))
                {
                    case 90:
                    case 270:
                        directContent.AddTemplate(importedPage, 0.0f, -1f, 1f, 0.0f, 0.0f, reader2.GetPageSizeWithRotation(1).Height);
                        break;
                    default:
                        directContent.AddTemplate(importedPage, 1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
                        break;
                }
            }

            reader1.Close();
        }

        private float CrearFormato(Configuracion configuracion, int numeroPlantillaAUtilizar)
        {
            var str = Path.GetTempFileName();
            var formatoFactura = numeroPlantillaAUtilizar == 1 ? configuracion.UnicaHoja : (numeroPlantillaAUtilizar == 2 ? configuracion.PrimeraHoja : (numeroPlantillaAUtilizar == 3 ? configuracion.SegundaHoja : (numeroPlantillaAUtilizar == 4 ? configuracion.TerceraHoja : null)));
            
            if (formatoFactura == null)
                throw new Exception("Numero de Plantilla a Utilizar incorrecta");

            File.Copy(formatoFactura.RutaPlantilla, str, true);
            var reader1 = new PdfReader(str);
            var pdfStamper = new PdfStamper(reader1, new FileStream(str + "tmp", FileMode.Create));
            var acroFields = pdfStamper.AcroFields;
            foreach (var acrofield in configuracion.Acrofields)
            {
                var acroField = acrofield;
                if (acroFields.Fields.Count(a => a.Key == acroField.Nombre) == 1)
                    acroFields.SetField(acroField.Nombre, acroField.Texto); 
            }
            pdfStamper.FormFlattening = true;
            pdfStamper.Close();
            var reader2 = new PdfReader(str + "tmp");
            var directContent = _pdfWriter.DirectContent;
            _documentoIText.NewPage();
            var importedPage = _pdfWriter.GetImportedPage(reader2, 1);
            switch (reader2.GetPageRotation(1))
            {
                case 90:
                case 270:
                    directContent.AddTemplate(importedPage, 0.0f, -1f, 1f, 0.0f, 0.0f, reader2.GetPageSizeWithRotation(1).Height);
                    break;
                default:
                    directContent.AddTemplate(importedPage, 1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
                    break;
            }
            if (configuracion.CodigoQr != null && !string.IsNullOrEmpty(configuracion.CodigoQr.Ruta) && (numeroPlantillaAUtilizar == 1 || numeroPlantillaAUtilizar == 4))
                formatoFactura.Imagenes.Add(configuracion.CodigoQr);

            foreach (var imagene in formatoFactura.Imagenes)
            {
                if (imagene != null)
                {
                    var imageBytes = File.ReadAllBytes(imagene.Ruta);
                    var instance = iTextSharp.text.Image.GetInstance(imageBytes);
                    instance.ScaleAbsolute(imagene.Ancho, imagene.Alto);
                    instance.SetAbsolutePosition(imagene.Coordenadas.X, CambioY(imagene.Coordenadas.Y + imagene.Alto));
                    _documentoIText.Add(instance);
                }
            }
            reader1.Close();
            if (File.Exists(str))
                File.Delete(str);
            return formatoFactura.AreaDetalle.CoordenadaYInicio;
        }

        private float CambioY(float y)
        {
            return _documentoIText.PageSize.Height - y;
        }
    }
}