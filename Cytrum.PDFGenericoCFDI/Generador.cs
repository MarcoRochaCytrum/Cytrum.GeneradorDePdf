using Cytrum.Core.Helper;
using Cytrum.Core.Model;
using Cytrum.Core.Servicios;
using Cytrum.PDF4;
using Cytrum.PDF4.Entidades;
using Cytrum.PDF4.Enumeraciones;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace Cytrum.PDFGenericoCFDI
{
    public class GeneradorPdfGenerico
    {
        public class PdfServiceResultado
        {
            public string Mensaje { get; set; }
            public byte[] Pdf { get; set; }
        }

        public GeneradorPdfGenerico()
        {
            ArchivosTemporales = new List<string>();
        }
        private IList<string> ArchivosTemporales { get; set; }
        private readonly Assembly _ensamblado = Assembly.GetExecutingAssembly();
        public PdfServiceResultado Generar(XmlDocument comprobanteTimbrado, Imagen logoCliente, string cadenaOriginal, InformacionAdicionalPdfModel informacionAdicional)
        {
            string logoTmp = null;

            if (logoCliente != null)
            {
                logoTmp = logoCliente.Ruta;
                ArchivosTemporales.Add(logoTmp);
            }


            var receptor = informacionAdicional?.Receptor;
            var emisor = informacionAdicional?.Emisor;
            var configuracion = new Configuracion { AltoLinea = 5, LineasDespuesDeDetalle = 1 };
            var lsImagenes = new List<Imagen>();

            if (!string.IsNullOrEmpty(logoTmp))
                lsImagenes.Add(new Imagen { Alto = logoCliente.Alto, Ancho = logoCliente.Ancho, Coordenadas = new Coordenadas { X = logoCliente.Coordenadas.X, Y = logoCliente.Coordenadas.Y }, Ruta = logoTmp });

            var timbre = comprobanteTimbrado.DocumentElement.GetElementsByTagName("tfd:TimbreFiscalDigital")[0];

            #region Configuracion Plantillas PDF

            var plantillaPagina1Temp = ResursosHelper.ObtenerRecursoIncrustado(_ensamblado, "Cytrum.PDFGenericoCFDI.Plantilla.Factura_Pagina1.pdf").FullName;
            var formatoUnicaHoja = new FormatoFactura
            {
                AreaDetalle = new AreaDetalle { CoordenadaYInicio = 327, CoordenadaYFin = 455 },
                RutaPlantilla = plantillaPagina1Temp,
                Imagenes = lsImagenes
            };
            ArchivosTemporales.Add(plantillaPagina1Temp);
            configuracion.UnicaHoja = formatoUnicaHoja;

            var plantillaPagina2Temp = ResursosHelper.ObtenerRecursoIncrustado(_ensamblado, "Cytrum.PDFGenericoCFDI.Plantilla.Factura_Pagina2.pdf").FullName;
            var formatoPrimeraHoja = new FormatoFactura
            {
                AreaDetalle = new AreaDetalle { CoordenadaYInicio = 327, CoordenadaYFin = 728 },
                RutaPlantilla = plantillaPagina2Temp,
                Imagenes = lsImagenes
            };
            ArchivosTemporales.Add(plantillaPagina2Temp);
            configuracion.PrimeraHoja = formatoPrimeraHoja;

            var plantillaPagina3Temp = ResursosHelper.ObtenerRecursoIncrustado(_ensamblado, "Cytrum.PDFGenericoCFDI.Plantilla.Factura_Pagina3.pdf").FullName;
            var formatoSegundaHoja = new FormatoFactura
            {
                AreaDetalle = new AreaDetalle { CoordenadaYInicio = 140, CoordenadaYFin = 728 },
                RutaPlantilla = plantillaPagina3Temp,
            };
            ArchivosTemporales.Add(plantillaPagina3Temp);
            configuracion.SegundaHoja = formatoSegundaHoja;

            var plantillaPagina4Temp = ResursosHelper.ObtenerRecursoIncrustado(_ensamblado, "Cytrum.PDFGenericoCFDI.Plantilla.Factura_Pagina4.pdf").FullName;
            var formatoTerceraHoja = new FormatoFactura
            {
                AreaDetalle = new AreaDetalle { CoordenadaYInicio = 140, CoordenadaYFin = 455 },
                RutaPlantilla = plantillaPagina4Temp,
            };
            ArchivosTemporales.Add(plantillaPagina4Temp);
            configuracion.TerceraHoja = formatoTerceraHoja;

            #endregion

            #region Llenado de campos
            var tipoComprobante = string.Empty;
            var serirFolio = CfdiHelper.ObtenerSerie(comprobanteTimbrado) + CfdiHelper.ObtenerFolio(comprobanteTimbrado);
            tipoComprobante = CfdiHelper.ObtenerTipoDeComprobante(comprobanteTimbrado);

            var acrofield = new Acrofield();
            acrofield.Nombre = "var_SerieyFolio";
            acrofield.Texto = serirFolio;
            configuracion.Acrofields.Add(acrofield);


            switch (tipoComprobante)
            {
                case "I":
                    tipoComprobante = "INGRESO";
                    break;
                case "E":
                    tipoComprobante = "EGRESO";
                    break;
                case "T":
                    tipoComprobante = "TRANSFERENCIA";
                    break;
            }

            acrofield = new Acrofield();
            acrofield.Nombre = "var_TipoComprobante";
            acrofield.Texto = tipoComprobante;
            configuracion.Acrofields.Add(acrofield);

            acrofield = new Acrofield();
            acrofield.Nombre = "var_Fecha";
            acrofield.Texto = CfdiHelper.ObtenerFechaComprobante(comprobanteTimbrado).ToString("yyyy-MM-dd hh:mm:ss");
            configuracion.Acrofields.Add(acrofield);

            acrofield = new Acrofield();
            acrofield.Nombre = "var_UUID";
            acrofield.Texto = CfdiHelper.ObtenerFolioSAT(comprobanteTimbrado);
            configuracion.Acrofields.Add(acrofield);

            acrofield = new Acrofield();
            acrofield.Nombre = "var_LugarExpedicion";
            acrofield.Texto = CfdiHelper.ObtenerLugarExpedicion(comprobanteTimbrado);
            configuracion.Acrofields.Add(acrofield);

            acrofield = new Acrofield();
            acrofield.Nombre = "var_NombreEmisor";
            acrofield.Texto = CfdiHelper.ObtenerEmisorNombre(comprobanteTimbrado);
            configuracion.Acrofields.Add(acrofield);

            acrofield = new Acrofield();
            acrofield.Nombre = "var_RFCEmisor";
            acrofield.Texto = CfdiHelper.ObtenerRfcEmisor(comprobanteTimbrado);
            configuracion.Acrofields.Add(acrofield);

            acrofield = new Acrofield();
            acrofield.Nombre = "var_Exportacion";
            acrofield.Texto = CfdiHelper.ObtenerExportacion(comprobanteTimbrado);
            configuracion.Acrofields.Add(acrofield);

            acrofield = new Acrofield();
            acrofield.Nombre = "var_RegimenFiscalReceptor";
            acrofield.Texto = CfdiHelper.ObtenerReceptorRegimenFiscalReceptor(comprobanteTimbrado);
            configuracion.Acrofields.Add(acrofield);

            var direccionEmisor = string.Empty;

            var direccionRenglon1Emisor = string.Empty;

            if (emisor != null)
            {
                if (!string.IsNullOrEmpty(emisor.Calle)) direccionRenglon1Emisor += emisor.Calle + ", ";
                if (!string.IsNullOrEmpty(emisor.NoExterior)) direccionRenglon1Emisor += emisor.NoExterior + ", ";
                if (!string.IsNullOrEmpty(emisor.NoInterior)) direccionRenglon1Emisor += emisor.NoInterior + ", ";
                if (!string.IsNullOrEmpty(emisor.Colonia)) direccionRenglon1Emisor += emisor.Colonia + ", "; //Environment.NewLine
                if (!string.IsNullOrEmpty(direccionRenglon1Emisor))
                {
                    direccionRenglon1Emisor = direccionRenglon1Emisor.Substring(0, direccionRenglon1Emisor.Length - 2);
                    direccionEmisor += direccionRenglon1Emisor;
                }

                var direccionRenglon2Emisor = string.Empty;
                if (!string.IsNullOrEmpty(emisor.Municipio)) direccionRenglon2Emisor += emisor.Municipio + ", ";
                if (!string.IsNullOrEmpty(emisor.Estado)) direccionRenglon2Emisor += emisor.Estado + ", ";
                if (!string.IsNullOrEmpty(emisor.CodigoPostal)) direccionRenglon2Emisor += emisor.CodigoPostal + ", ";
                if (!string.IsNullOrEmpty(emisor.Pais)) direccionRenglon2Emisor += emisor.Pais + ", ";
                if (!string.IsNullOrEmpty(direccionRenglon2Emisor))
                {
                    direccionRenglon2Emisor = direccionRenglon2Emisor.Substring(0, direccionRenglon2Emisor.Length - 2);
                    if (!string.IsNullOrEmpty(direccionEmisor))
                        direccionEmisor += Environment.NewLine + direccionRenglon2Emisor;
                }
            }

            acrofield = new Acrofield();
            acrofield.Nombre = "var_DomicilioEmisor";
            acrofield.Texto = direccionEmisor;
            configuracion.Acrofields.Add(acrofield);

            acrofield = new Acrofield();
            acrofield.Nombre = "var_NombreReceptor";
            acrofield.Texto = CfdiHelper.ObtenerReceptorNombre(comprobanteTimbrado);
            configuracion.Acrofields.Add(acrofield);

            acrofield = new Acrofield();
            acrofield.Nombre = "var_RFCReceptor";
            acrofield.Texto = CfdiHelper.ObtenerRfcReceptor(comprobanteTimbrado);
            configuracion.Acrofields.Add(acrofield);

            acrofield = new Acrofield();
            acrofield.Nombre = "var_UsoCFDI";
            acrofield.Texto = CfdiHelper.ObtenerReceptorUsoCFDI(comprobanteTimbrado);
            configuracion.Acrofields.Add(acrofield);

            var direccionReceptor = string.Empty;

            var direccionRenglon1 = string.Empty;

            if (receptor != null)
            {
                if (!string.IsNullOrEmpty(receptor.Calle)) direccionRenglon1 += receptor.Calle + ", ";
                if (!string.IsNullOrEmpty(receptor.NoExterior)) direccionRenglon1 += receptor.NoExterior + ", ";
                if (!string.IsNullOrEmpty(receptor.NoInterior)) direccionRenglon1 += receptor.NoInterior + ", ";
                if (!string.IsNullOrEmpty(direccionRenglon1))
                {
                    direccionRenglon1 = direccionRenglon1.Substring(0, direccionRenglon1.Length - 2);
                    direccionReceptor += direccionRenglon1;
                }

                var direccionRenglon2 = string.Empty;
                if (!string.IsNullOrEmpty(receptor.Colonia)) direccionRenglon2 += receptor.Colonia;
                if (!string.IsNullOrEmpty(receptor.Municipio)) direccionRenglon2 += receptor.Municipio + ", ";
                if (!string.IsNullOrEmpty(receptor.Estado)) direccionRenglon2 += receptor.Estado + ", ";
                if (!string.IsNullOrEmpty(receptor.CodigoPostal)) direccionRenglon2 += receptor.CodigoPostal + ", ";
                if (!string.IsNullOrEmpty(receptor.Pais)) direccionRenglon2 += receptor.Pais + ", ";
                if (!string.IsNullOrEmpty(direccionRenglon2))
                {
                    direccionRenglon2 = direccionRenglon2.Substring(0, direccionRenglon2.Length - 2);

                    if (!string.IsNullOrEmpty(direccionReceptor)) direccionReceptor += Environment.NewLine;
                    direccionReceptor += direccionRenglon2;
                }
            }

            acrofield = new Acrofield();
            acrofield.Nombre = "var_DomicilioReceptor";
            acrofield.Texto = direccionReceptor;
            configuracion.Acrofields.Add(acrofield);

            acrofield = new Acrofield();
            acrofield.Nombre = "var_NoSerieCertSAT";
            acrofield.Texto = CfdiHelper.ObtenerNoCertificadoSAT(comprobanteTimbrado);
            configuracion.Acrofields.Add(acrofield);

            acrofield = new Acrofield();
            acrofield.Nombre = "var_NoSerieCertCSD";
            acrofield.Texto = CfdiHelper.ObtenerNoCertificado(comprobanteTimbrado);
            configuracion.Acrofields.Add(acrofield);

            acrofield = new Acrofield();
            acrofield.Nombre = "var_MetodoPago";
            acrofield.Texto = CfdiHelper.ObtenerMetodoPago(comprobanteTimbrado);
            configuracion.Acrofields.Add(acrofield);

            acrofield = new Acrofield();
            acrofield.Nombre = "var_FormaPago";
            acrofield.Texto = CfdiHelper.ObtenerFormaPago(comprobanteTimbrado);
            configuracion.Acrofields.Add(acrofield);

            acrofield = new Acrofield();
            acrofield.Nombre = "var_Moneda";
            acrofield.Texto = CfdiHelper.ObtenerMoneda(comprobanteTimbrado);
            configuracion.Acrofields.Add(acrofield);

            acrofield = new Acrofield();
            acrofield.Nombre = "var_TipoCambio";
            acrofield.Texto = CfdiHelper.ObtenerTipoCambio(comprobanteTimbrado).ToString(CultureInfo.InvariantCulture);
            configuracion.Acrofields.Add(acrofield);

            acrofield = new Acrofield();
            acrofield.Nombre = "var_RegimenFiscalEmisor";
            acrofield.Texto = CfdiHelper.ObtenerRegimenFiscal(comprobanteTimbrado);
            configuracion.Acrofields.Add(acrofield);

            var total = CfdiHelper.ObtenerTotal(comprobanteTimbrado);
            var moneda = CfdiHelper.ObtenerMoneda(comprobanteTimbrado);
            var importeConLetra = string.Empty;
            switch (moneda)
            {
                case "MXN":
                    importeConLetra = TextoAMonedaHelper.enletras(total.ToString(CultureInfo.InvariantCulture), TextoAMonedaHelper.Moneda.Pesos);
                    break;
                case "USD":
                    importeConLetra = TextoAMonedaHelper.enletras(total.ToString(CultureInfo.InvariantCulture), TextoAMonedaHelper.Moneda.Dolares);
                    break;
                case "USN":
                    importeConLetra = TextoAMonedaHelper.enletras(total.ToString(CultureInfo.InvariantCulture), TextoAMonedaHelper.Moneda.Dolares);
                    break;
                case "EUR":
                    importeConLetra = TextoAMonedaHelper.enletras(total.ToString(CultureInfo.InvariantCulture), TextoAMonedaHelper.Moneda.Euros);
                    break;
                case "CHF":
                    importeConLetra = TextoAMonedaHelper.enletras(total.ToString(CultureInfo.InvariantCulture), TextoAMonedaHelper.Moneda.FrancosSuizos);
                    break;
                default:
                    break;
            }

            acrofield = new Acrofield();
            acrofield.Nombre = "var_ImporteLetra";
            acrofield.Texto = importeConLetra;
            configuracion.Acrofields.Add(acrofield);

            acrofield = new Acrofield();
            acrofield.Nombre = "var_SubTotal";
            acrofield.Texto = "$" + $"{CfdiHelper.ObtenerSubtotal(comprobanteTimbrado):#,##0.00##}"; ;
            configuracion.Acrofields.Add(acrofield);

            acrofield = new Acrofield();
            acrofield.Nombre = "var_TotalImpuestosTras";
            acrofield.Texto = "$" + $"{CfdiHelper.ObtenerTotalImpuestosTrasladados(comprobanteTimbrado):#,##0.00##}"; ;
            configuracion.Acrofields.Add(acrofield);

            acrofield = new Acrofield();
            acrofield.Nombre = "var_TotalImpuestosRet";
            acrofield.Texto = "$" + $"{CfdiHelper.ObtenerTotalImpuestosRetenidos(comprobanteTimbrado):#,##0.00##}"; ;
            configuracion.Acrofields.Add(acrofield);

            acrofield = new Acrofield();
            acrofield.Nombre = "var_Descuento";
            acrofield.Texto = "$" + $"{CfdiHelper.ObtenerDescuento(comprobanteTimbrado):#,##0.00##}";
            configuracion.Acrofields.Add(acrofield);

            acrofield = new Acrofield();
            acrofield.Nombre = "var_SelloDigitalCFDI";
            acrofield.Texto = CfdiHelper.ObtenerSello(comprobanteTimbrado);
            configuracion.Acrofields.Add(acrofield);

            acrofield = new Acrofield();
            acrofield.Nombre = "var_SelloSAT";
            acrofield.Texto = timbre.Attributes.GetNamedItem("SelloSAT").Value;
            configuracion.Acrofields.Add(acrofield);

            acrofield = new Acrofield();
            acrofield.Nombre = "var_CadenaOriginal";
            acrofield.Texto = cadenaOriginal;
            configuracion.Acrofields.Add(acrofield);

            acrofield = new Acrofield();
            acrofield.Nombre = "var_Comentario";
            acrofield.Texto = informacionAdicional?.Comentarios;
            configuracion.Acrofields.Add(acrofield);

            acrofield = new Acrofield();
            acrofield.Nombre = "var_Total";
            acrofield.Texto = "$" + $"{CfdiHelper.ObtenerTotal(comprobanteTimbrado):#,##0.00##}";
            configuracion.Acrofields.Add(acrofield);

            //Numerador de pagina
            configuracion.Numerador.Fuente.Nombre = "Courier";
            configuracion.Numerador.Fuente.Alineacion = Alineacion.Centro;
            configuracion.Numerador.Separador = "/";
            configuracion.Numerador.Coordenadas.X = 565;
            configuracion.Numerador.Coordenadas.Y = 16;
            configuracion.Numerador.EsNumeracionLarga = true;
            configuracion.Numerador.Fuente.Tamano = 8;

            #endregion

            #region Llenado de detalle

            var conceptos = CfdiHelper.ObtenerConceptosGenerico(comprobanteTimbrado).Conceptos;
            foreach (var concepto in conceptos)
            {
                var detalle = new Detalle();
                var columna = new Columna { EspacioX = 14 };
                var fuenteCentro = new Fuente { Alineacion = Alineacion.Centro, Tamano = 7 };
                var fuenteDerecha = new Fuente
                {
                    Alineacion = Alineacion.Derecha,
                    Tamano = 7
                };
                var fuenteIzquierda = new Fuente
                {
                    Alineacion = Alineacion.Izquierda,
                    Tamano = 7
                };

                var renglon = new RenglonColumna
                {
                    Texto = concepto.Cantidad.ToString(),
                    EspacioX = 25,
                    MaximoNumeroDeCaracteres = 10,
                    Fuente = fuenteCentro
                };

                columna.RenglonesColumna.Add(renglon);
                detalle.Columnas.Add(columna);

                columna = new Columna { EspacioX = 14 };
                renglon = new RenglonColumna
                {
                    Texto = concepto.ClaveProdServ,
                    EspacioX = 73,
                    MaximoNumeroDeCaracteres = 14,
                    Fuente = fuenteCentro
                };

                columna.RenglonesColumna.Add(renglon);
                detalle.Columnas.Add(columna);

                columna = new Columna { EspacioX = 14 };
                renglon = new RenglonColumna
                {
                    Texto = concepto.ClaveUnidad + " - " + concepto.Unidad,
                    EspacioX = 135,
                    MaximoNumeroDeCaracteres = 16,
                    Fuente = fuenteCentro
                };

                columna.RenglonesColumna.Add(renglon);
                detalle.Columnas.Add(columna);

                columna = new Columna { EspacioX = 14 };
                renglon = new RenglonColumna
                {
                    Texto = concepto.ObjetoImp,
                    EspacioX = 189,
                    MaximoNumeroDeCaracteres = 2,
                    Fuente = fuenteCentro
                };

                columna.RenglonesColumna.Add(renglon);
                detalle.Columnas.Add(columna);

                var descripcion = concepto.Descripcion;
                columna = new Columna { EspacioX = 14 };
                renglon = new RenglonColumna
                {
                    Texto = descripcion,
                    EspacioX = 207,
                    MaximoNumeroDeCaracteres = 56,
                    Fuente = fuenteIzquierda
                };

                columna.RenglonesColumna.Add(renglon);
                detalle.Columnas.Add(columna);

                columna = new Columna { EspacioX = 14 };
                renglon = new RenglonColumna
                {
                    Texto = "$" + $"{Convert.ToDecimal(concepto.ValorUnitario):#,##0.00####}",
                    EspacioX = 454,
                    MaximoNumeroDeCaracteres = 15,
                    Fuente = fuenteDerecha
                };

                columna.RenglonesColumna.Add(renglon);
                detalle.Columnas.Add(columna);

                columna = new Columna { EspacioX = 14 };
                renglon = new RenglonColumna
                {
                    Texto = "$" + $"{Convert.ToDecimal(concepto.Descuento):#,##0.00####}",
                    EspacioX = 510,
                    MaximoNumeroDeCaracteres = 15,
                    Fuente = fuenteDerecha
                };

                columna.RenglonesColumna.Add(renglon);
                detalle.Columnas.Add(columna);

                columna = new Columna { EspacioX = 14 };
                renglon = new RenglonColumna
                {
                    Texto = "$" + $"{Convert.ToDecimal(concepto.Importe):#,##0.00####}",
                    EspacioX = 580,
                    MaximoNumeroDeCaracteres = 15,
                    Fuente = fuenteDerecha
                };

                columna.RenglonesColumna.Add(renglon);

                detalle.Columnas.Add(columna);
                configuracion.Detalles.Add(detalle);

                //Impuestos
                if (concepto.Traslados.Any() || concepto.Retenciones.Any())
                {
                    AgregarEncabezadoImpuestos(fuenteCentro, configuracion);

                    foreach (var conceptoTraslado in concepto.Traslados)
                    {
                        AgregarImpuestoDetalle("Traslado", fuenteCentro, conceptoTraslado, configuracion);
                    }

                    foreach (var conceptoRetencion in concepto.Retenciones)
                    {
                        AgregarImpuestoDetalle("Retención", fuenteCentro, conceptoRetencion, configuracion);
                    }
                }

                detalle = new Detalle();
                columna = new Columna { EspacioX = 14 };
                renglon = new RenglonColumna
                {
                    Texto = "",
                    EspacioX = 22,
                    MaximoNumeroDeCaracteres = 8,
                    Fuente = fuenteCentro
                };

                columna.RenglonesColumna.Add(renglon);
                detalle.Columnas.Add(columna);
                configuracion.Detalles.Add(detalle);
            }

            ConfiguracionCfdiRelacionados(comprobanteTimbrado, configuracion);

            #endregion

            //Configuracion QR
            var rfcReceptor = CfdiHelper.ObtenerRfcReceptor(comprobanteTimbrado);
            var rfcEmisor = CfdiHelper.ObtenerRfcEmisor(comprobanteTimbrado);
            var selloCfd = CfdiHelper.ObtenerSello(comprobanteTimbrado);
            var rutaQr = ServicioQr.RutaQrGenerado(total, CfdiHelper.ObtenerFolioSAT(comprobanteTimbrado), rfcEmisor, rfcReceptor, selloCfd);

            configuracion.CodigoQr.Alto = 85;
            configuracion.CodigoQr.Ancho = 85;
            configuracion.CodigoQr.Coordenadas.X = 500;
            configuracion.CodigoQr.Coordenadas.Y = 545;
            configuracion.CodigoQr.Ruta = rutaQr;

            var pdfTemp = new GetTempFileHelper().Obtener() + ".pdf";
            ArchivosTemporales.Add(pdfTemp);
            ArchivosTemporales.Add(rutaQr);

            configuracion.NombreFinal = pdfTemp;
            configuracion.TextoRelleno = string.Empty;

            try
            {
                var pdf4 = new Pdf4();
                pdf4.Dibujar(configuracion);

                var pdfBytes = File.ReadAllBytes(pdfTemp);

                foreach (var archivo in ArchivosTemporales)
                {
                    File.Delete(archivo);
                }

                return new PdfServiceResultado { Pdf = pdfBytes };
            }
            catch (Exception ex)
            {
                return new PdfServiceResultado { Mensaje = "Ocurrio un error al generar el PDF, " + ex };
            }
        }

        private static void ConfiguracionCfdiRelacionados(XmlDocument comprobanteTimbrado, Configuracion configuracion)
        {
            var tipoRelacion = CfdiHelper.ObtenerTipoRelacion(comprobanteTimbrado);
            if (string.IsNullOrEmpty(tipoRelacion))
            {
                var acrofieldTipoRelacion = new Acrofield
                {
                    Nombre = "var_CFDIRelacionado",
                    Texto = "-"
                };
                configuracion.Acrofields.Add(acrofieldTipoRelacion);
                acrofieldTipoRelacion = new Acrofield
                {
                    Nombre = "var_UUIDRelacionado",
                    Texto = "-"
                };
                configuracion.Acrofields.Add(acrofieldTipoRelacion);
                return;
            };

            var cfdiRelacionados = CfdiHelper.ObtenerCfdisRelacionados(comprobanteTimbrado);
            if (!cfdiRelacionados.Any())
            {
                var acrofieldCfdiRelacionado = new Acrofield
                {
                    Nombre = "var_CFDIRelacionado",
                    Texto = "-"
                };
                configuracion.Acrofields.Add(acrofieldCfdiRelacionado);
                acrofieldCfdiRelacionado = new Acrofield
                {
                    Nombre = "var_UUIDRelacionado",
                    Texto = "-"
                };
                configuracion.Acrofields.Add(acrofieldCfdiRelacionado);
                return;
            };

            var acrofield = new Acrofield
            {
                Nombre = "var_CFDIRelacionado",
                Texto = tipoRelacion
            };
            configuracion.Acrofields.Add(acrofield);

            if (cfdiRelacionados.Count == 1)
            {
                acrofield = new Acrofield
                {
                    Nombre = "var_UUIDRelacionado",
                    Texto = cfdiRelacionados.First().ToString()
                };
                configuracion.Acrofields.Add(acrofield);
                return;
            }

            acrofield = new Acrofield
            {
                Nombre = "var_UUIDRelacionado",
                Texto = "Listado mas abajo."
            };
            configuracion.Acrofields.Add(acrofield);

            var detalle = new Detalle();
            var columna = new Columna { EspacioX = 14 };
            var fuenteCentro = new Fuente { Alineacion = Alineacion.Centro, Tamano = 7 };
            var renglon = new RenglonColumna
            {
                Texto = "CFDI Relacionados",
                EspacioX = 285,
                MaximoNumeroDeCaracteres = 20,
                Fuente = fuenteCentro
            };

            columna.RenglonesColumna.Add(renglon);
            detalle.Columnas.Add(columna);
            configuracion.Detalles.Add(detalle);

            foreach (var cfdiRelacionado in cfdiRelacionados)
            {
                detalle = new Detalle();
                columna = new Columna { EspacioX = 14 };
                renglon = new RenglonColumna
                {
                    Texto = cfdiRelacionado.ToString(),
                    EspacioX = 285,
                    MaximoNumeroDeCaracteres = 36,
                    Fuente = fuenteCentro
                };

                columna.RenglonesColumna.Add(renglon);
                detalle.Columnas.Add(columna);
                configuracion.Detalles.Add(detalle);
            }
        }

        private static void AgregarEncabezadoImpuestos(Fuente fuenteCentro, Configuracion configuracion)
        {
            var detalle = new Detalle();
            var columna = new Columna { EspacioX = 14 };
            var renglon = new RenglonColumna
            {
                Texto = "IMPUESTO",
                EspacioX = 22,
                MaximoNumeroDeCaracteres = 8,
                Fuente = fuenteCentro
            };

            columna.RenglonesColumna.Add(renglon);
            detalle.Columnas.Add(columna);

            columna = new Columna { EspacioX = 14 };
            renglon = new RenglonColumna
            {
                Texto = "BASE",
                EspacioX = 71,
                MaximoNumeroDeCaracteres = 4,
                Fuente = fuenteCentro
            };

            columna.RenglonesColumna.Add(renglon);
            detalle.Columnas.Add(columna);

            columna = new Columna { EspacioX = 14 };
            renglon = new RenglonColumna
            {
                Texto = "CLAVE",
                EspacioX = 133,
                MaximoNumeroDeCaracteres = 8,
                Fuente = fuenteCentro
            };

            columna.RenglonesColumna.Add(renglon);
            detalle.Columnas.Add(columna);

            columna = new Columna { EspacioX = 14 };
            renglon = new RenglonColumna
            {
                Texto = "T. FACTOR",
                EspacioX = 227,
                MaximoNumeroDeCaracteres = 11,
                Fuente = fuenteCentro
            };

            columna.RenglonesColumna.Add(renglon);
            detalle.Columnas.Add(columna);

            columna = new Columna { EspacioX = 14 };
            renglon = new RenglonColumna()
            {
                Texto = "TASA O CUOTA",
                EspacioX = 298,
                MaximoNumeroDeCaracteres = 12,
                Fuente = fuenteCentro
            };

            columna.RenglonesColumna.Add(renglon);
            detalle.Columnas.Add(columna);

            columna = new Columna { EspacioX = 14 };
            renglon = new RenglonColumna
            {
                Texto = "IMPORTE",
                EspacioX = 369,
                MaximoNumeroDeCaracteres = 12,
                Fuente = fuenteCentro
            };

            columna.RenglonesColumna.Add(renglon);
            detalle.Columnas.Add(columna);
            configuracion.Detalles.Add(detalle);
        }

        private static void AgregarImpuestoDetalle(string tipoImpuesto, Fuente fuenteCentro, CfdiHelper.ConceptoModel.Impuesto impuesto, Configuracion configuracion)
        {
            var detalle = new Detalle();
            var columna = new Columna { EspacioX = 14 };
            var renglon = new RenglonColumna
            {
                Texto = tipoImpuesto,
                EspacioX = 22,
                MaximoNumeroDeCaracteres = 10,
                Fuente = fuenteCentro
            };

            columna.RenglonesColumna.Add(renglon);
            detalle.Columnas.Add(columna);

            columna = new Columna { EspacioX = 14 };
            renglon = new RenglonColumna
            {
                Texto = impuesto.Base,
                EspacioX = 71,
                MaximoNumeroDeCaracteres = 15,
                Fuente = fuenteCentro
            };

            columna.RenglonesColumna.Add(renglon);
            detalle.Columnas.Add(columna);

            columna = new Columna { EspacioX = 14 };


            var impuestoTipo = impuesto.Tipo;
            switch (impuesto.Tipo)
            {
                case "001":
                    impuestoTipo += " - ISR";
                    break;
                case "002":
                    impuestoTipo += " - IVA";
                    break;
                case "003":
                    impuestoTipo += " - IEPS";
                    break;
            }

            renglon = new RenglonColumna
            {
                Texto = impuestoTipo,
                EspacioX = 133,
                MaximoNumeroDeCaracteres = 10,
                Fuente = fuenteCentro
            };

            columna.RenglonesColumna.Add(renglon);
            detalle.Columnas.Add(columna);

            columna = new Columna { EspacioX = 14 };
            renglon = new RenglonColumna
            {
                Texto = impuesto.Factor,
                EspacioX = 227,
                MaximoNumeroDeCaracteres = 11,
                Fuente = fuenteCentro
            };

            columna.RenglonesColumna.Add(renglon);
            detalle.Columnas.Add(columna);

            columna = new Columna { EspacioX = 14 };
            renglon = new RenglonColumna()
            {
                Texto = impuesto.Tasa,
                EspacioX = 298,
                MaximoNumeroDeCaracteres = 15,
                Fuente = fuenteCentro
            };

            columna.RenglonesColumna.Add(renglon);
            detalle.Columnas.Add(columna);

            columna = new Columna { EspacioX = 14 };
            renglon = new RenglonColumna
            {
                Texto = impuesto.Importe,
                EspacioX = 369,
                MaximoNumeroDeCaracteres = 15,
                Fuente = fuenteCentro
            };

            columna.RenglonesColumna.Add(renglon);
            detalle.Columnas.Add(columna);
            configuracion.Detalles.Add(detalle);
        }
    }
}