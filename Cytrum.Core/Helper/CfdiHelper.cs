using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static Cytrum.Core.Helper.CfdiHelper.ConceptoModel;
using System.Xml.Linq;
using System.Xml;
using Cytrum.Core.Enumerations;
using Cytrum.Core.Exceptions;
using Cytrum.Core.Model;

namespace Cytrum.Core.Helper
{
    public class CfdiHelper
    {
        public static decimal ObtenerVersion(XmlDocument xmlEntrada)
        {
            try
            {
                return decimal.Parse(xmlEntrada.DocumentElement.Attributes["Version"].Value);
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "Version", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }

        public static string ObtenerNamespaceCfdi(XmlDocument xmlEntrada)
        {
            var version = ObtenerVersion(xmlEntrada);
            var namespaceCfdi = "http://www.sat.gob.mx/cfd/3";

            if (version == 4.0m)
                namespaceCfdi = "http://www.sat.gob.mx/cfd/4";

            return namespaceCfdi;
        }
        public static string ObtenerRfcEmisor(XmlDocument xmlEntrada)
        {
            try
            {
                if (xmlEntrada.DocumentElement != null)
                {
                    var version = xmlEntrada.DocumentElement.HasAttribute("Version");
                    var xEmisor = xmlEntrada.DocumentElement["cfdi:Emisor"];
                    if (xEmisor == null) return string.Empty;

                    return version ? xEmisor.Attributes["Rfc"].Value : xEmisor.Attributes["rfc"].Value;
                }
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "RFC Emisor", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }

            return string.Empty;
        }
        public static string ObtenerRfcEmisorShrt(string rfc)
        {
            try
            {
                if (rfc != null && rfc.Length > 2)
                    return rfc.Substring(0, 3);
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "RFC Emisor", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }

            return string.Empty;
        }

        public static string ObtenerRfcReceptor(XmlDocument xmlEntrada)
        {
            try
            {
                if (xmlEntrada.DocumentElement != null)
                {
                    var version = xmlEntrada.DocumentElement.HasAttribute("Version");
                    var xReceptor = xmlEntrada.DocumentElement["cfdi:Receptor"];
                    if (xReceptor == null) return string.Empty;

                    return version ? xReceptor.Attributes["Rfc"].Value : xReceptor.Attributes["rfc"].Value;
                }
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "RFC Receptor", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }

            return string.Empty;
        }

        public static string ObtenerSerie(XmlDocument xmlEntrada)
        {
            try
            {
                return xmlEntrada.DocumentElement.HasAttribute("Serie") ? xmlEntrada.DocumentElement.Attributes["Serie"].Value : "";
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "Serie", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }

        public static string ObtenerFolio(XmlDocument xmlEntrada)
        {
            try
            {
                return xmlEntrada.DocumentElement.HasAttribute("Folio") ? xmlEntrada.DocumentElement.Attributes["Folio"].Value : "";
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "Folio", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }

        public static int? ObtenerFolioVisor(XmlDocument xmlEntrada)
        {
            try
            {
                int? folioVisor = null;
                var folio = xmlEntrada.DocumentElement.HasAttribute("Folio") ? xmlEntrada.DocumentElement.Attributes["Folio"].Value : "";
                var numeros = Regex.Split(folio, @"\D+");
                foreach (string valor in numeros)
                {
                    if (!string.IsNullOrEmpty(valor) && int.TryParse(valor, out int folioVisorInt))
                    {
                        folioVisor = folioVisorInt;
                    }
                }
                return folioVisor;
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "Folio", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }
        public static int? ObtenerFolioVisorString(string folio)
        {
            try
            {
                int? folioVisor = null;
                var numeros = Regex.Split(folio, @"\D+");
                foreach (string valor in numeros)
                {
                    if (!string.IsNullOrEmpty(valor) && int.TryParse(valor, out int folioVisorInt))
                    {
                        folioVisor = folioVisorInt;
                    }
                }
                return folioVisor;
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "Folio", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }

        public static decimal ObtenerTotal(XmlDocument xmlEntrada)
        {
            try
            {
                return Convert.ToDecimal(xmlEntrada.DocumentElement.Attributes["Total"].Value);
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "Total", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }

        public static decimal ObtenerTotalRetencionesIVAComplementoPago(XmlDocument xmlEntrada)
        {
            try
            {
                return Convert.ToDecimal(xmlEntrada.DocumentElement["cfdi:Complemento"]["pago20:Pagos"]["pago20:Totales"].Attributes["TotalRetencionesIVA"]?.Value);
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "Total", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }

        public static decimal ObtenerTotalRetencionesISRComplementoPago(XmlDocument xmlEntrada)
        {
            try
            {
                return Convert.ToDecimal(xmlEntrada.DocumentElement["cfdi:Complemento"]["pago20:Pagos"]["pago20:Totales"].Attributes["TotalRetencionesISR"]?.Value);
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "Total", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }

        public static decimal ObtenerTotalRetencionesIEPSComplementoPago(XmlDocument xmlEntrada)
        {
            try
            {
                return Convert.ToDecimal(xmlEntrada.DocumentElement["cfdi:Complemento"]["pago20:Pagos"]["pago20:Totales"].Attributes["TotalRetencionesIEPS"]?.Value);
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "Total", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }

        public static string ObtenerComplementoVersionCartaPorte(XmlDocument xmlEntrada)
        {
            try
            {
                return xmlEntrada.DocumentElement["cfdi:Complemento"]["cartaporte31:CartaPorte"].Attributes["Version"]?.Value;
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "cporte:Version", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }

        public static decimal ObtenerTotalTrasladosBase16ComplementoPago(XmlDocument xmlEntrada)
        {
            try
            {
                return Convert.ToDecimal(xmlEntrada.DocumentElement["cfdi:Complemento"]["pago20:Pagos"]["pago20:Totales"].Attributes["TotalTrasladosBaseIVA16"]?.Value);
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "Total", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }

        public static decimal ObtenerTotalTrasladosImpuestosIVA16ComplementoPago(XmlDocument xmlEntrada)
        {
            try
            {
                return Convert.ToDecimal(xmlEntrada.DocumentElement["cfdi:Complemento"]["pago20:Pagos"]["pago20:Totales"].Attributes["TotalTrasladosImpuestoIVA16"]?.Value);
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "Total", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }
        public static decimal ObtenerTotalTrasladosBaseIVA8ComplementoPago(XmlDocument xmlEntrada)
        {
            try
            {
                return Convert.ToDecimal(xmlEntrada.DocumentElement["cfdi:Complemento"]["pago20:Pagos"]["pago20:Totales"].Attributes["TotalTrasladosBaseIVA8"]?.Value);
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "Total", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }
        public static decimal ObtenerTotalTrasladosImpuestoIVA8ComplementoPago(XmlDocument xmlEntrada)
        {
            try
            {
                return Convert.ToDecimal(xmlEntrada.DocumentElement["cfdi:Complemento"]["pago20:Pagos"]["pago20:Totales"].Attributes["TotalTrasladosImpuestoIVA8"]?.Value);
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "Total", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }
        public static decimal ObtenerTotalTrasladosBaseIVA0ComplementoPago(XmlDocument xmlEntrada)
        {
            try
            {
                return Convert.ToDecimal(xmlEntrada.DocumentElement["cfdi:Complemento"]["pago20:Pagos"]["pago20:Totales"].Attributes["TotalTrasladosBaseIVA0"]?.Value);
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "Total", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }
        public static decimal ObtenerTotalTrasladosImpuestoIVA0ComplementoPago(XmlDocument xmlEntrada)
        {
            try
            {
                return Convert.ToDecimal(xmlEntrada.DocumentElement["cfdi:Complemento"]["pago20:Pagos"]["pago20:Totales"].Attributes["TotalTrasladosImpuestoIVA0"]?.Value);
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "Total", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }
        public static decimal ObtenerTotalTrasladosBaseIVAExentoComplementoPago(XmlDocument xmlEntrada)
        {
            try
            {
                return Convert.ToDecimal(xmlEntrada.DocumentElement["cfdi:Complemento"]["pago20:Pagos"]["pago20:Totales"].Attributes["TotalTrasladosBaseIVAExento"]?.Value);
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "Total", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }
        public static decimal ObtenerMontoTotalPagosComplementoPago(XmlDocument xmlEntrada)
        {
            try
            {
                return Convert.ToDecimal(xmlEntrada.DocumentElement["cfdi:Complemento"]["pago20:Pagos"]["pago20:Totales"].Attributes["MontoTotalPagos"].Value);
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "Total", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }

        public static InformacionAdicionalPdfModel ObtenerInformacionAdicional(XmlDocument xmlEntrada)
        {
            try
            {
                var modelo = new InformacionAdicionalPdfModel { Conceptos = new List<InformacionAdicionalPdfModel.ModelConcepto>() };
                if (xmlEntrada.DocumentElement["cfdi:Addenda"] != null && xmlEntrada.DocumentElement["cfdi:Addenda"]["InformacionAdicional"] != null)
                {
                    var nodoAddendaInformacionAdicional = xmlEntrada.DocumentElement["cfdi:Addenda"]["InformacionAdicional"];

                    var nodoEmisor = nodoAddendaInformacionAdicional["Emisor"];
                    if (nodoEmisor != null)
                    {
                        modelo.Emisor = new InformacionAdicionalPdfModel.ModelEmisor();
                        var nodoEmisorDomicilio = nodoEmisor["Domicilio"];
                        if (nodoEmisorDomicilio.Attributes["Calle"] != null) modelo.Emisor.Calle = nodoEmisorDomicilio.Attributes["Calle"].Value;
                        if (nodoEmisorDomicilio.Attributes["Colonia"] != null) modelo.Emisor.Colonia = nodoEmisorDomicilio.Attributes["Colonia"].Value;
                        if (nodoEmisorDomicilio.Attributes["NumeroExterior"] != null) modelo.Emisor.NoExterior = nodoEmisorDomicilio.Attributes["NumeroExterior"].Value;
                        if (nodoEmisorDomicilio.Attributes["NumeroInterior"] != null) modelo.Emisor.NoInterior = nodoEmisorDomicilio.Attributes["NumeroInterior"].Value;
                        if (nodoEmisorDomicilio.Attributes["Municipio"] != null) modelo.Emisor.Municipio = nodoEmisorDomicilio.Attributes["Municipio"].Value;
                        if (nodoEmisorDomicilio.Attributes["Estado"] != null) modelo.Emisor.Estado = nodoEmisorDomicilio.Attributes["Estado"].Value;
                        if (nodoEmisorDomicilio.Attributes["Pais"] != null) modelo.Emisor.Pais = nodoEmisorDomicilio.Attributes["Pais"].Value;
                        if (nodoEmisorDomicilio.Attributes["CodigoPostal"] != null) modelo.Emisor.CodigoPostal = nodoEmisorDomicilio.Attributes["CodigoPostal"].Value;
                    }

                    var nodoReceptor = nodoAddendaInformacionAdicional["Receptor"];
                    if (nodoReceptor != null)
                    {
                        modelo.Receptor = new InformacionAdicionalPdfModel.ModelReceptor();
                        var nodoReceptorDomicilio = nodoReceptor["Domicilio"];
                        if (nodoReceptorDomicilio.Attributes["Calle"] != null) modelo.Receptor.Calle = nodoReceptorDomicilio.Attributes["Calle"].Value;
                        if (nodoReceptorDomicilio.Attributes["Colonia"] != null) modelo.Receptor.Colonia = nodoReceptorDomicilio.Attributes["Colonia"].Value;
                        if (nodoReceptorDomicilio.Attributes["NumeroExterior"] != null) modelo.Receptor.NoExterior = nodoReceptorDomicilio.Attributes["NumeroExterior"].Value;
                        if (nodoReceptorDomicilio.Attributes["NumeroInterior"] != null) modelo.Receptor.NoInterior = nodoReceptorDomicilio.Attributes["NumeroInterior"].Value;
                        if (nodoReceptorDomicilio.Attributes["Municipio"] != null) modelo.Receptor.Municipio = nodoReceptorDomicilio.Attributes["Municipio"].Value;
                        if (nodoReceptorDomicilio.Attributes["Estado"] != null) modelo.Receptor.Estado = nodoReceptorDomicilio.Attributes["Estado"].Value;
                        if (nodoReceptorDomicilio.Attributes["Pais"] != null) modelo.Receptor.Pais = nodoReceptorDomicilio.Attributes["Pais"].Value;
                        if (nodoReceptorDomicilio.Attributes["CodigoPostal"] != null) modelo.Receptor.CodigoPostal = nodoReceptorDomicilio.Attributes["CodigoPostal"].Value;
                    }

                    if (nodoAddendaInformacionAdicional["Conceptos"] != null)
                    {
                        var nodoConceptos = nodoAddendaInformacionAdicional["Conceptos"].GetElementsByTagName("Concepto");
                        foreach (XmlElement concepto in nodoConceptos)
                        {
                            var conceptoModel = new InformacionAdicionalPdfModel.ModelConcepto();
                            if (concepto.Attributes["NoIdentificacion"] != null) conceptoModel.NoIdentificacion = concepto.Attributes["NoIdentificacion"].Value;
                            if (concepto.Attributes["ClaveProdServ"] != null) conceptoModel.ClaveProdServ = concepto.Attributes["ClaveProdServ"].Value;
                            if (concepto.Attributes["Cantidad"] != null) conceptoModel.Cantidad = concepto.Attributes["Cantidad"].Value;
                            if (concepto.Attributes["NoIdeComentariontificacion"] != null) conceptoModel.Comentario = concepto.Attributes["Comentario"].Value;

                            modelo.Conceptos.Add(conceptoModel);
                        }
                    }

                    var nodoComentarios = nodoAddendaInformacionAdicional["Comentarios"];
                    if (nodoComentarios != null) modelo.Comentarios = nodoComentarios.InnerText;

                    var nodoOrdenVenta = nodoAddendaInformacionAdicional["OrdenVenta"];
                    if (nodoOrdenVenta != null) modelo.OrdenVenta = nodoOrdenVenta.InnerText;

                    var nodoOrdenCompra = nodoAddendaInformacionAdicional["OrdenCompra"];
                    if (nodoOrdenCompra != null) modelo.OrdenCompra = nodoOrdenCompra.InnerText;

                    var nodoProveedor = nodoAddendaInformacionAdicional["Proveedor"];
                    if (nodoProveedor != null) modelo.Proveedor = nodoProveedor.InnerText;

                    var nodoRefCliente = nodoAddendaInformacionAdicional["RefCliente"];
                    if (nodoRefCliente != null) modelo.RefCliente = nodoRefCliente.InnerText;

                    var nodoFormato = nodoAddendaInformacionAdicional["Formato"];
                    if (nodoFormato != null) modelo.Formato = nodoFormato.InnerText;

                    var nodoDatosBancarios = nodoAddendaInformacionAdicional["DatosBancarios"];
                    if (nodoDatosBancarios != null) modelo.DatosBancarios = nodoDatosBancarios.InnerText;

                    return modelo;
                }
                else return modelo;
            }
            catch (Exception ex)
            {

                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "Addenda", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }

        public static string ObtenerFolioSAT(XmlDocument xmlEntrada)
        {
            try
            {
                return xmlEntrada.DocumentElement["cfdi:Complemento"]["tfd:TimbreFiscalDigital"].Attributes["UUID"].Value;
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "TimbreFiscalDigital: UUID", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }

        public static DateTime ObtenerFechaComprobante(XmlDocument xmlEntrada)
        {
            try
            {
                return DateTime.Parse(xmlEntrada.DocumentElement.Attributes["Fecha"].Value);
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "Fecha", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }

        public static string ObtenerFechaComprobanteMesShrt(XmlDocument xmlEntrada)
        {
            try
            {
                var fecha = DateTime.Parse(xmlEntrada.DocumentElement.Attributes["Fecha"].Value);
                var mes = fecha.Month;
                switch (mes)
                {
                    case 1:
                        return "ENE";
                    case 2:
                        return "FEB";
                    case 3:
                        return "MAR";
                    case 4:
                        return "ABR";
                    case 5:
                        return "MAY";
                    case 6:
                        return "JUN";
                    case 7:
                        return "JUL";
                    case 8:
                        return "AGO";
                    case 9:
                        return "SEP";
                    case 10:
                        return "OCT";
                    case 11:
                        return "NOV";
                    case 12:
                        return "DIC";
                    default:
                        return "";
                }
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "MesShrt", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }

        public static string ObtenerFechaComprobanteMes2Digitos(XmlDocument xmlEntrada)
        {
            try
            {
                var fecha = DateTime.Parse(xmlEntrada.DocumentElement.Attributes["Fecha"].Value);
                var mes = fecha.Month.ToString("d2");
                return mes;
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "Mes2", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }

        public static DateTime ObtenerFechaTimbre(XmlDocument xmlEntrada)
        {
            try
            {
                return DateTime.Parse(xmlEntrada.DocumentElement["cfdi:Complemento"]["tfd:TimbreFiscalDigital"].Attributes["FechaTimbrado"].Value);
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "TimbreFiscalDigital: Fecha", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }

        public static string ObtenerSello(XmlDocument xmlEntrada)
        {
            try
            {
                return xmlEntrada.DocumentElement.Attributes["Sello"].Value;
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "Sello", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }

        public static decimal ObtenerTotalImpuestosRetenidos(XmlDocument xmlEntrada)
        {
            try
            {
                if (xmlEntrada.DocumentElement["cfdi:Impuestos"] != null)
                {
                    return decimal.Parse(xmlEntrada.DocumentElement["cfdi:Impuestos"].HasAttribute("TotalImpuestosRetenidos") ? xmlEntrada.DocumentElement["cfdi:Impuestos"].Attributes["TotalImpuestosRetenidos"].Value : "0.00");
                }
                return decimal.Parse("0.00");
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "TotalImpuestosRetenidos", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }

        public static decimal ObtenerTotalImpuestosTrasladados(XmlDocument xmlEntrada)
        {
            try
            {
                if (xmlEntrada.DocumentElement["cfdi:Impuestos"] != null)
                {
                    return decimal.Parse(xmlEntrada.DocumentElement["cfdi:Impuestos"].HasAttribute("TotalImpuestosTrasladados") ? xmlEntrada.DocumentElement["cfdi:Impuestos"].Attributes["TotalImpuestosTrasladados"].Value : "0.00");
                }
                return decimal.Parse("0.00");
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "TotalImpuestosTrasladados", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }

        public static string ObtenerNoCertificado(XmlDocument xmlEntrada)
        {
            try
            {
                return xmlEntrada.DocumentElement.Attributes["NoCertificado"].Value;
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "NoCertificado", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }

        public static string ObtenerNoCertificadoSAT(XmlDocument xmlEntrada)
        {
            try
            {
                return xmlEntrada.DocumentElement["cfdi:Complemento"]["tfd:TimbreFiscalDigital"].Attributes["NoCertificadoSAT"].Value;
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "TimbreFiscalDigital: NoCertificadoSAT", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }

        public static string ObtenerTipoDeComprobante(XmlDocument xmlEntrada)
        {
            try
            {
                return xmlEntrada.DocumentElement.Attributes["TipoDeComprobante"].Value;
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "TipoDeComprobante", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }

        public static string ObtenerMoneda(XmlDocument xmlEntrada)
        {
            try
            {
                return xmlEntrada.DocumentElement.Attributes["Moneda"].Value;
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "Moneda", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }

        public static decimal ObtenerTipoCambio(XmlDocument xmlEntrada)
        {
            try
            {
                return decimal.Parse(xmlEntrada.DocumentElement.HasAttribute("TipoCambio") ? xmlEntrada.DocumentElement.Attributes["TipoCambio"].Value : "0.00");
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "TipoCambio", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }

        public static string ObtenerFormaPago(XmlDocument xmlEntrada)
        {
            try
            {
                return xmlEntrada.DocumentElement.HasAttribute("FormaPago") ? xmlEntrada.DocumentElement.Attributes["FormaPago"].Value : "";
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "FormaPago", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }

        public static string ObtenerCertificado(XmlDocument xmlEntrada)
        {
            try
            {
                return xmlEntrada.DocumentElement.Attributes["Certificado"].Value;
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "Certificado", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }

        public static string ObtenerCondicionesDePago(XmlDocument xmlEntrada)
        {
            try
            {
                return xmlEntrada.DocumentElement.HasAttribute("CondicionesDePago") ? xmlEntrada.DocumentElement.Attributes["CondicionesDePago"].Value : "";
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "CondicionesDePago", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }

        public static decimal ObtenerSubtotal(XmlDocument xmlEntrada)
        {
            try
            {
                return Convert.ToDecimal(xmlEntrada.DocumentElement.Attributes["SubTotal"].Value);
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "Subtotal", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }

        public static decimal ObtenerDescuento(XmlDocument xmlEntrada)
        {
            try
            {
                return Convert.ToDecimal(xmlEntrada.DocumentElement.HasAttribute("Descuento") ? xmlEntrada.DocumentElement.Attributes["Descuento"].Value : "0.00");
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "Descuento", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }

        public static string ObtenerMetodoPago(XmlDocument xmlEntrada)
        {
            try
            {
                return xmlEntrada.DocumentElement.HasAttribute("MetodoPago") ? xmlEntrada.DocumentElement.Attributes["MetodoPago"].Value : "";
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "MetodoPago", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }

        public static string ObtenerLugarExpedicion(XmlDocument xmlEntrada)
        {
            try
            {
                return xmlEntrada.DocumentElement.Attributes["LugarExpedicion"].Value;
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "LugarExpedicion", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }

        public static string ObtenerEmisorNombre(XmlDocument xmlEntrada)
        {
            try
            {
                return xmlEntrada.DocumentElement["cfdi:Emisor"].HasAttribute("Nombre") ? xmlEntrada.DocumentElement["cfdi:Emisor"].Attributes["Nombre"].Value : "";
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "EmisorNombre", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }

        public static string ObtenerRegimenFiscal(XmlDocument xmlEntrada)
        {
            try
            {
                return xmlEntrada.DocumentElement["cfdi:Emisor"].Attributes["RegimenFiscal"].Value;
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "RegimenFiscal", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }

        public static string ObtenerReceptorNombre(XmlDocument xmlEntrada)
        {
            try
            {
                return xmlEntrada.DocumentElement["cfdi:Receptor"].HasAttribute("Nombre") ? xmlEntrada.DocumentElement["cfdi:Receptor"].Attributes["Nombre"].Value : "";
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "ReceptorNombre", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }

        public static string ObtenerConfirmacion(XmlDocument xmlEntrada)
        {
            try
            {
                return xmlEntrada.DocumentElement.HasAttribute("Confirmacion") ? xmlEntrada.DocumentElement.Attributes["Confirmacion"].Value : "";
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "Confirmacion", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }

        public static string ObtenerExportacion(XmlDocument xmlEntrada)
        {
            try
            {
                return xmlEntrada.DocumentElement.HasAttribute("Exportacion") ? xmlEntrada.DocumentElement.Attributes["Exportacion"].Value : "";
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "Exportación", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }

        public static string ObtenerTipoRelacion(XmlDocument xmlEntrada)
        {
            if (xmlEntrada.DocumentElement["cfdi:CfdiRelacionados"] != null)
            {
                return xmlEntrada.DocumentElement["cfdi:CfdiRelacionados"].HasAttribute("TipoRelacion") ? xmlEntrada.DocumentElement["cfdi:CfdiRelacionados"].Attributes["TipoRelacion"].Value : null;
            }
            return null;
        }

        public static List<Guid> ObtenerCfdisRelacionados(XmlDocument xmlEntrada)
        {
            var listaUUIDs = new List<Guid>();
            if (xmlEntrada.DocumentElement["cfdi:CfdiRelacionados"] != null)
            {
                var cfdisRelacionados = xmlEntrada.DocumentElement["cfdi:CfdiRelacionados"].GetElementsByTagName("cfdi:CfdiRelacionado");
                foreach (XmlElement nodo in cfdisRelacionados)
                {
                    listaUUIDs.Add(Guid.Parse(nodo.GetAttribute("UUID")));
                }
                return listaUUIDs;
            }
            return null;
        }

        public static string ObtenerReceptorResidenciaFiscal(XmlDocument xmlEntrada)
        {
            try
            {
                return xmlEntrada.DocumentElement["cfdi:Receptor"].HasAttribute("ResidenciaFiscal") ? xmlEntrada.DocumentElement["cfdi:Receptor"].Attributes["ResidenciaFiscal"].Value : "";
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "ReceptorResidenciaFiscal", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }

        public static string ObtenerReceptorDomicilioFiscalReceptor(XmlDocument xmlEntrada)
        {
            try
            {
                return xmlEntrada.DocumentElement["cfdi:Receptor"].HasAttribute("DomicilioFiscalReceptor") ? xmlEntrada.DocumentElement["cfdi:Receptor"].Attributes["DomicilioFiscalReceptor"].Value : "";
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "DomicilioFiscalReceptor", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }

        public static string ObtenerReceptorRegimenFiscalReceptor(XmlDocument xmlEntrada)
        {
            try
            {
                return xmlEntrada.DocumentElement["cfdi:Receptor"].HasAttribute("RegimenFiscalReceptor") ? xmlEntrada.DocumentElement["cfdi:Receptor"].Attributes["RegimenFiscalReceptor"].Value : "";
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "RegimenFiscalReceptor", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }

        public static string ObtenerReceptorNumRegIdTrib(XmlDocument xmlEntrada)
        {
            try
            {
                return xmlEntrada.DocumentElement["cfdi:Receptor"].HasAttribute("NumRegIdTrib") ? xmlEntrada.DocumentElement["cfdi:Receptor"].Attributes["NumRegIdTrib"].Value : "";
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "ReceptorNumRegIdTrib", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }

        public static string ObtenerReceptorUsoCFDI(XmlDocument xmlEntrada)
        {
            try
            {
                return xmlEntrada.DocumentElement["cfdi:Receptor"].HasAttribute("UsoCFDI") ? xmlEntrada.DocumentElement["cfdi:Receptor"].Attributes["UsoCFDI"].Value : "";
            }
            catch (Exception ex)
            {
                var mensajeError = string.Format(ExceptionMensaje.EjecucionCfdiHelper.DisplayName, "ReceptorUsoCFDI", Environment.NewLine, ex);

                throw new CfdiHelperException(mensajeError);
            }
        }

        public static List<Concepto> ObtenerConceptos(XmlDocument xmlEntrada)
        {
            List<Concepto> conceptos = new List<Concepto>();
            if (xmlEntrada.DocumentElement["cfdi:Conceptos"] != null)
            {
                var concepto = xmlEntrada.DocumentElement["cfdi:Conceptos"].GetElementsByTagName("cfdi:Concepto");
                foreach (XmlElement nodo in concepto)
                {
                    var conceptoModel = new Concepto
                    {
                        ClaveProdServ = nodo.GetAttribute("ClaveProdServ"),
                        NoIdentificacion = nodo.GetAttribute("NoIdentificacion"),
                        Cantidad = Convert.ToDecimal(nodo.GetAttribute("Cantidad")),
                        ClaveUnidad = nodo.GetAttribute("ClaveUnidad"),
                        Unidad = nodo.GetAttribute("Unidad"),
                        Descripcion = nodo.GetAttribute("Descripcion"),
                        ValorUnitario = nodo.GetAttribute("ValorUnitario"),
                        Importe = nodo.GetAttribute("Importe"),
                        Descuento = nodo.HasAttribute("Descuento") ? nodo.GetAttribute("Descuento") : "0.00",
                        ObjetoImp = nodo.GetAttribute("ObjetoImp")
                    };

                    if (nodo.GetElementsByTagName("cfdi:Impuestos").Count > 0)
                    {
                        XmlElement impuestos = (XmlElement)nodo.GetElementsByTagName("cfdi:Impuestos")[0];

                        if (impuestos.GetElementsByTagName("cfdi:Traslados").Count > 0)
                        {
                            var impuestosTraslados = (XmlElement)impuestos.GetElementsByTagName("cfdi:Traslados")[0];
                            var traslados = impuestosTraslados.GetElementsByTagName("cfdi:Traslado");

                            foreach (XmlElement nodotraslado in traslados)
                            {
                                if (conceptoModel.Traslados == null)
                                    conceptoModel.Traslados = new List<Impuesto>();

                                var impuestoConcepto = new Impuesto();
                                var bExento = false;

                                if (nodotraslado.GetAttribute("Base") != null)
                                {
                                    impuestoConcepto.Base = nodotraslado.GetAttribute("Base");
                                }

                                if (nodotraslado.GetAttribute("Impuesto") != null)
                                {
                                    impuestoConcepto.Tipo = nodotraslado.GetAttribute("Impuesto");
                                }

                                if (nodotraslado.GetAttribute("TipoFactor") != null)
                                {
                                    impuestoConcepto.Factor = nodotraslado.GetAttribute("TipoFactor");
                                    if (impuestoConcepto.Factor == "Exento")
                                    {
                                        bExento = true;
                                    }
                                }

                                if (!bExento)
                                {
                                    if (nodotraslado.GetAttribute("TasaOCuota") != null)
                                    {
                                        impuestoConcepto.Tasa = nodotraslado.GetAttribute("TasaOCuota");
                                    }

                                    if (nodotraslado.GetAttribute("Importe") != null)
                                    {
                                        impuestoConcepto.Importe = nodotraslado.GetAttribute("Importe");
                                    }
                                }

                                conceptoModel.Traslados.Add(impuestoConcepto);
                            }
                        }

                        if (impuestos.GetElementsByTagName("cfdi:Retenciones").Count > 0)
                        {
                            var impuestosRetenciones = (XmlElement)impuestos.GetElementsByTagName("cfdi:Retenciones")[0];
                            var retenciones = impuestosRetenciones.GetElementsByTagName("cfdi:Retencion");

                            foreach (XmlElement nodoretencion in retenciones)
                            {
                                if (conceptoModel.Retenciones == null)
                                    conceptoModel.Retenciones = new List<Impuesto>();

                                var impuestoConcepto = new Impuesto();

                                if (nodoretencion.GetAttribute("Base") != null)
                                {
                                    impuestoConcepto.Base = nodoretencion.GetAttribute("Base");
                                }

                                if (nodoretencion.GetAttribute("Impuesto") != null)
                                {
                                    impuestoConcepto.Tipo = nodoretencion.GetAttribute("Impuesto");
                                }

                                if (nodoretencion.GetAttribute("TipoFactor") != null)
                                {
                                    impuestoConcepto.Factor = nodoretencion.GetAttribute("TipoFactor");
                                }

                                if (nodoretencion.GetAttribute("TasaOCuota") != null)
                                {
                                    impuestoConcepto.Tasa = nodoretencion.GetAttribute("TasaOCuota");
                                }

                                if (nodoretencion.GetAttribute("Importe") != null)
                                {
                                    impuestoConcepto.Importe = nodoretencion.GetAttribute("Importe");
                                }

                                conceptoModel.Retenciones.Add(impuestoConcepto);
                            }
                        }
                    }
                    conceptos.Add(conceptoModel);
                }
            }
            return conceptos;
        }

        public static ImpuestosModel ObtenerImpuestosPorTipo(XmlDocument xmlEntrada)
        {
            var model = new ImpuestosModel();
            var xmlDoc = XDocument.Parse(xmlEntrada.InnerXml);
            var ns = xmlDoc.Root.Name.Namespace;
            var impuestos = xmlDoc.Root.Element(ns + "Impuestos");
            if (impuestos != null)
            {
                var traslados = impuestos.Element(ns + "Traslados");
                var retenciones = impuestos.Element(ns + "Retenciones");

                if (traslados != null)
                {
                    var xTraslados = traslados.Elements(ns + "Traslado");
                    foreach (var xTraslado in xTraslados)
                    {
                        var trasladoModel = new ImpuestosModel.Impuesto
                        {
                            Importe = xTraslado.Attribute("Importe")?.Value,
                            Tipo = xTraslado.Attribute("Impuesto")?.Value,
                            Factor = xTraslado.Attribute("TipoFactor")?.Value,
                            Tasa = xTraslado.Attribute("TasaOCuota")?.Value,
                            Base = xTraslado.Attribute("Base")?.Value
                        };

                        model.Traslados.Add(trasladoModel);
                    }
                }

                if (retenciones != null)
                {
                    var xRetenciones = retenciones.Elements(ns + "Retencion");
                    foreach (var xRetencion in xRetenciones)
                    {
                        var retencionModel = new ImpuestosModel.Impuesto
                        {
                            Importe = xRetencion.Attribute("Importe")?.Value,
                            Tipo = xRetencion.Attribute("Impuesto")?.Value,
                            Factor = xRetencion.Attribute("TipoFactor")?.Value,
                            Tasa = xRetencion.Attribute("TasaOCuota")?.Value,
                            Base = xRetencion.Attribute("Base")?.Value
                        };

                        model.Retenciones.Add(retencionModel);
                    }
                }

            }

            return model;
        }
        public static ConceptoModel ObtenerConceptosGenerico(XmlDocument xmlEntrada)
        {
            var model = new ConceptoModel();
            var xmlDoc = XDocument.Parse(xmlEntrada.InnerXml);
            var ns = xmlDoc.Root.Name.Namespace;
            var xConceptos = xmlDoc.Root.Element(ns + "Conceptos");
            if (xConceptos != null)
            {
                foreach (var xConcepto in xConceptos.Elements(ns + "Concepto"))
                {
                    var concepto = new ConceptoModel.Concepto();
                    var impuestos = xConcepto.Element(ns + "Impuestos");
                    if (impuestos != null)
                    {
                        var traslados = impuestos.Element(ns + "Traslados");
                        var retenciones = impuestos.Element(ns + "Retenciones");

                        if (traslados != null)
                        {
                            var xTraslados = traslados.Elements(ns + "Traslado");
                            foreach (var xTraslado in xTraslados)
                            {
                                var trasladoModel = new ConceptoModel.Impuesto
                                {
                                    Importe = xTraslado.Attribute("Importe")?.Value,
                                    Tipo = xTraslado.Attribute("Impuesto")?.Value,
                                    Factor = xTraslado.Attribute("TipoFactor")?.Value,
                                    Tasa = xTraslado.Attribute("TasaOCuota")?.Value,
                                    Base = xTraslado.Attribute("Base")?.Value
                                };

                                concepto.Traslados.Add(trasladoModel);
                            }
                        }

                        if (retenciones != null)
                        {
                            var xRetenciones = retenciones.Elements(ns + "Retencion");
                            foreach (var xRetencion in xRetenciones)
                            {
                                var retencionModel = new ConceptoModel.Impuesto
                                {
                                    Importe = xRetencion.Attribute("Importe")?.Value,
                                    Tipo = xRetencion.Attribute("Impuesto")?.Value,
                                    Factor = xRetencion.Attribute("TipoFactor")?.Value,
                                    Tasa = xRetencion.Attribute("TasaOCuota")?.Value,
                                    Base = xRetencion.Attribute("Base")?.Value
                                };

                                concepto.Retenciones.Add(retencionModel);
                            }
                        }
                    }

                    concepto.ClaveProdServ = xConcepto.Attribute("ClaveProdServ")?.Value;
                    concepto.NoIdentificacion = xConcepto.Attribute("NoIdentificacion")?.Value;
                    concepto.Cantidad = decimal.Parse(xConcepto.Attribute("Cantidad")?.Value);
                    concepto.ClaveUnidad = xConcepto.Attribute("ClaveUnidad")?.Value;
                    concepto.Unidad = xConcepto.Attribute("Unidad")?.Value;
                    concepto.Descripcion = xConcepto.Attribute("Descripcion")?.Value;
                    concepto.ValorUnitario = xConcepto.Attribute("ValorUnitario")?.Value;
                    concepto.Importe = xConcepto.Attribute("Importe")?.Value;
                    concepto.Descuento = xConcepto.Attribute("Descuento")?.Value;
                    concepto.ObjetoImp = xConcepto.Attribute("ObjetoImp")?.Value;

                    model.Conceptos.Add(concepto);
                }
            }

            return model;
        }

        public class ImpuestosModel
        {
            public ImpuestosModel()
            {
                Traslados = new List<Impuesto>();
                Retenciones = new List<Impuesto>();
            }
            public IList<Impuesto> Traslados { get; set; }
            public IList<Impuesto> Retenciones { get; set; }
            public class Impuesto
            {
                public string Base { get; set; }
                public string Tipo { get; set; }
                public string Factor { get; set; }
                public string Tasa { get; set; }
                public string Importe { get; set; }
            }
        }
        public class ConceptoModel
        {
            public ConceptoModel()
            {
                Conceptos = new List<Concepto>();
            }
            public IList<Concepto> Conceptos { get; set; }
            public class Concepto
            {
                public Concepto()
                {
                    Traslados = new List<Impuesto>();
                    Retenciones = new List<Impuesto>();
                }
                public decimal Cantidad { get; set; }
                public string NoIdentificacion { get; set; }
                public string ClaveUnidad { get; set; }
                public string Unidad { get; set; }
                public string Descripcion { get; set; }
                public string ValorUnitario { get; set; }
                public string Importe { get; set; }
                public string ClaveProdServ { get; set; }
                public string Descuento { get; set; }
                public string ObjetoImp { get; set; }
                public IList<Impuesto> Traslados { get; set; }
                public IList<Impuesto> Retenciones { get; set; }
            }

            public class Impuesto
            {
                public string Base { get; set; }
                public string Tipo { get; set; }
                public string Factor { get; set; }
                public string Tasa { get; set; }
                public string Importe { get; set; }
            }
        }

        public static string ObtenerPrefijoPago(XmlDocument xmlEntrada)
        {
            var pago10 = xmlEntrada.DocumentElement["cfdi:Complemento"]["pago10:Pagos"];
            if (pago10 != null)
                return "pago10";

            var pago20 = xmlEntrada.DocumentElement["cfdi:Complemento"]["pago20:Pagos"];
            if (pago20 != null)
                return "pago20";

            return null;
        }

        //public static List<ComplementoPago> ObtenerComplementoPago(XmlDocument xmlEntrada)
        //{
        //    var prefijo = ObtenerPrefijoPago(xmlEntrada);
        //    var complementosPago = new List<ComplementoPago>();
        //    var complementoPagoPagos = xmlEntrada.DocumentElement["cfdi:Complemento"][$"{prefijo}:Pagos"].GetElementsByTagName($"{prefijo}:Pago");

        //    foreach (XmlElement nodo in complementoPagoPagos)
        //    {
        //        var complementoPagoModel = new ComplementoPago
        //        {
        //            Id = Guid.NewGuid(),
        //            FechaPago = Convert.ToDateTime(nodo.GetAttribute("FechaPago")),
        //            FormaDePagoP = nodo.GetAttribute("FormaDePagoP"),
        //            MonedaP = nodo.GetAttribute("MonedaP"),
        //            TipoCambioP = Convert.ToDecimal(nodo.HasAttribute("TipoCambioP") ? nodo.GetAttribute("TipoCambioP") : "0.00"),
        //            Monto = Convert.ToDecimal(nodo.HasAttribute("Monto") ? nodo.GetAttribute("Monto") : "0.00"),
        //            NumOperacion = nodo.GetAttribute("NumOperacion"),
        //            RfcEmisorCtaOrd = nodo.GetAttribute("RfcEmisorCtaOrd"),
        //            NomBancoOrdExt = nodo.GetAttribute("NomBancoOrdExt"),
        //            CtaOrdenante = nodo.GetAttribute("CtaOrdenante"),
        //            RfcEmisorCtaBen = nodo.GetAttribute("RfcEmisorCtaBen"),
        //            CtaBeneficiario = nodo.GetAttribute("CtaBeneficiario"),
        //            TipoCadPago = nodo.GetAttribute("TipoCadPago"),
        //            CertPago = nodo.GetAttribute("CertPago"),
        //            CadPago = nodo.GetAttribute("CadPago"),
        //            SelloPago = nodo.GetAttribute("SelloPago")
        //        };

        //        List<DoctoRelacionado> DoctoRelacionados = new List<DoctoRelacionado>();
        //        foreach (XmlElement doctorel in nodo)
        //        {
        //            if (doctorel.Name.Contains("DoctoRelacionado"))
        //            {
        //                var id = Guid.NewGuid();
        //                var idDocumento = doctorel.GetAttribute("IdDocumento");
        //                var serie = doctorel.HasAttribute("Serie") ? doctorel.GetAttribute("Serie") : "";
        //                var folio = doctorel.HasAttribute("Folio") ? doctorel.GetAttribute("Folio") : "";
        //                var monedaDR = doctorel.GetAttribute("MonedaDR");
        //                var tipoCambioDR = Convert.ToDecimal(doctorel.HasAttribute("TipoCambioDR") ? doctorel.GetAttribute("TipoCambioDR") : "0.00");
        //                var metodoDePagoDR = doctorel.GetAttribute("MetodoDePagoDR");
        //                var objetoImpDr = doctorel.GetAttribute("ObjetoImpDR");
        //                var numParcialidad = doctorel.HasAttribute("NumParcialidad") ? doctorel.GetAttribute("NumParcialidad") : "";
        //                var impSaldoAnt = Convert.ToDecimal(doctorel.HasAttribute("ImpSaldoAnt") ? doctorel.GetAttribute("ImpSaldoAnt") : "0.00");
        //                var impPagado = Convert.ToDecimal(doctorel.HasAttribute("ImpPagado") ? doctorel.GetAttribute("ImpPagado") : "0.00");
        //                var impSaldoInsoluto = Convert.ToDecimal(doctorel.HasAttribute("ImpSaldoInsoluto") ? doctorel.GetAttribute("ImpSaldoInsoluto") : "0.00");

        //                DoctoRelacionados.Add(new DoctoRelacionado(id, idDocumento, serie, folio, monedaDR, tipoCambioDR, metodoDePagoDR, objetoImpDr, numParcialidad, impSaldoAnt, impPagado, impSaldoInsoluto));
        //            }
        //        }

        //        complementoPagoModel.DoctoRelacionados = DoctoRelacionados;

        //        complementosPago.Add(complementoPagoModel);
        //    }

        //    return complementosPago;
        //}

        public static DateTime ObtenerComplementoPagoFechaPago(XmlDocument xmlEntrada)
        {
            var prefijo = ObtenerPrefijoPago(xmlEntrada);

            return DateTime.Parse(xmlEntrada.DocumentElement["cfdi:Complemento"][$"{prefijo}:Pagos"][$"{prefijo}:Pago"].Attributes["FechaPago"].Value);
        }

        public static string ObtenerComplementoPagoFormaDePagoP(XmlDocument xmlEntrada)
        {
            var prefijo = ObtenerPrefijoPago(xmlEntrada);

            return xmlEntrada.DocumentElement["cfdi:Complemento"][$"{prefijo}:Pagos"][$"{prefijo}:Pago"].Attributes["FormaDePagoP"].Value;
        }

        public static string ObtenerComplementoPagoMonedaP(XmlDocument xmlEntrada)
        {
            var prefijo = ObtenerPrefijoPago(xmlEntrada);

            return xmlEntrada.DocumentElement["cfdi:Complemento"][$"{prefijo}:Pagos"][$"{prefijo}:Pago"].Attributes["MonedaP"].Value;
        }

        public static decimal ObtenerComplementoPagoTipoCambioP(XmlDocument xmlEntrada)
        {
            var prefijo = ObtenerPrefijoPago(xmlEntrada);

            return decimal.Parse(xmlEntrada.DocumentElement["cfdi:Complemento"][$"{prefijo}:Pagos"][$"{prefijo}:Pago"].HasAttribute("TipoCambioP") ? xmlEntrada.DocumentElement["cfdi:Complemento"][$"{prefijo}:Pagos"][$"{prefijo}:Pago"].Attributes["TipoCambioP"].Value : "0.00");
        }

        public static decimal ObtenerComplementoPagoMonto(XmlDocument xmlEntrada)
        {
            var prefijo = ObtenerPrefijoPago(xmlEntrada);

            return decimal.Parse(xmlEntrada.DocumentElement["cfdi:Complemento"][$"{prefijo}:Pagos"][$"{prefijo}:Pago"].Attributes["Monto"].Value);
        }

        public static string ObtenerComplementoPagoNumOperacion(XmlDocument xmlEntrada)
        {
            var prefijo = ObtenerPrefijoPago(xmlEntrada);

            return xmlEntrada.DocumentElement["cfdi:Complemento"][$"{prefijo}:Pagos"][$"{prefijo}:Pago"].HasAttribute("NumOperacion") ? xmlEntrada.DocumentElement["cfdi:Complemento"][$"{prefijo}:Pagos"][$"{prefijo}:Pago"].Attributes["NumOperacion"].Value : null;
        }

        public static string ObtenerComplementoPagoRfcEmisorCtaOrd(XmlDocument xmlEntrada)
        {
            var prefijo = ObtenerPrefijoPago(xmlEntrada);

            return xmlEntrada.DocumentElement["cfdi:Complemento"][$"{prefijo}:Pagos"][$"{prefijo}:Pago"].HasAttribute("RfcEmisorCtaOrd") ? xmlEntrada.DocumentElement["cfdi:Complemento"][$"{prefijo}:Pagos"][$"{prefijo}:Pago"].Attributes["RfcEmisorCtaOrd"].Value : null;
        }

        public static string ObtenerComplementoPagoNomBancoOrdExt(XmlDocument xmlEntrada)
        {
            var prefijo = ObtenerPrefijoPago(xmlEntrada);

            return xmlEntrada.DocumentElement["cfdi:Complemento"][$"{prefijo}:Pagos"][$"{prefijo}:Pago"].HasAttribute("NomBancoOrdExt") ? xmlEntrada.DocumentElement["cfdi:Complemento"][$"{prefijo}:Pagos"][$"{prefijo}:Pago"].Attributes["NomBancoOrdExt"].Value : null;
        }

        public static string ObtenerComplementoPagoCtaOrdenante(XmlDocument xmlEntrada)
        {
            var prefijo = ObtenerPrefijoPago(xmlEntrada);

            return xmlEntrada.DocumentElement["cfdi:Complemento"][$"{prefijo}:Pagos"][$"{prefijo}:Pago"].HasAttribute("CtaOrdenante") ? xmlEntrada.DocumentElement["cfdi:Complemento"][$"{prefijo}:Pagos"][$"{prefijo}:Pago"].Attributes["CtaOrdenante"].Value : null;
        }

        public static string ObtenerComplementoPagoRfcEmisorCtaBen(XmlDocument xmlEntrada)
        {
            var prefijo = ObtenerPrefijoPago(xmlEntrada);

            return xmlEntrada.DocumentElement["cfdi:Complemento"][$"{prefijo}:Pagos"][$"{prefijo}:Pago"].HasAttribute("RfcEmisorCtaBen") ? xmlEntrada.DocumentElement["cfdi:Complemento"][$"{prefijo}:Pagos"][$"{prefijo}:Pago"].Attributes["RfcEmisorCtaBen"].Value : null;
        }

        public static string ObtenerComplementoPagoCtaBeneficiario(XmlDocument xmlEntrada)
        {
            var prefijo = ObtenerPrefijoPago(xmlEntrada);

            return xmlEntrada.DocumentElement["cfdi:Complemento"][$"{prefijo}:Pagos"][$"{prefijo}:Pago"].HasAttribute("CtaBeneficiario") ? xmlEntrada.DocumentElement["cfdi:Complemento"][$"{prefijo}:Pagos"][$"{prefijo}:Pago"].Attributes["CtaBeneficiario"].Value : null;
        }

        public static string ObtenerComplementoPagoTipoCadPago(XmlDocument xmlEntrada)
        {
            var prefijo = ObtenerPrefijoPago(xmlEntrada);

            return xmlEntrada.DocumentElement["cfdi:Complemento"][$"{prefijo}:Pagos"][$"{prefijo}:Pago"].HasAttribute("TipoCadPago") ? xmlEntrada.DocumentElement["cfdi:Complemento"][$"{prefijo}:Pagos"][$"{prefijo}:Pago"].Attributes["TipoCadPago"].Value : null;
        }

        public static string ObtenerComplementoPagoCertPago(XmlDocument xmlEntrada)
        {
            var prefijo = ObtenerPrefijoPago(xmlEntrada);

            return xmlEntrada.DocumentElement["cfdi:Complemento"][$"{prefijo}:Pagos"][$"{prefijo}:Pago"].HasAttribute("CertPago") ? xmlEntrada.DocumentElement["cfdi:Complemento"][$"{prefijo}:Pagos"][$"{prefijo}:Pago"].Attributes["CertPago"].Value : null;
        }

        public static string ObtenerComplementoPagoCadPago(XmlDocument xmlEntrada)
        {
            var prefijo = ObtenerPrefijoPago(xmlEntrada);

            return xmlEntrada.DocumentElement["cfdi:Complemento"][$"{prefijo}:Pagos"][$"{prefijo}:Pago"].HasAttribute("CadPago") ? xmlEntrada.DocumentElement["cfdi:Complemento"][$"{prefijo}:Pagos"][$"{prefijo}:Pago"].Attributes["CadPago"].Value : null;
        }

        public static string ObtenerComplementoPagoSelloPago(XmlDocument xmlEntrada)
        {
            var prefijo = ObtenerPrefijoPago(xmlEntrada);

            return xmlEntrada.DocumentElement["cfdi:Complemento"][$"{prefijo}:Pagos"][$"{prefijo}:Pago"].HasAttribute("SelloPago") ? xmlEntrada.DocumentElement["cfdi:Complemento"][$"{prefijo}:Pagos"][$"{prefijo}:Pago"].Attributes["SelloPago"].Value : null;
        }

        //public static List<DoctoRelacionado> ObtenerDoctosRelacionados(XmlDocument xmlEntrada)
        //{
        //    var prefijo = ObtenerPrefijoPago(xmlEntrada);

        //    List<DoctoRelacionado> DoctoRelacionados = new List<DoctoRelacionado>();
        //    if (xmlEntrada.DocumentElement["cfdi:Complemento"][$"{prefijo}:Pagos"][$"{prefijo}:Pago"][$"{prefijo}:DoctoRelacionado"] != null)
        //    {
        //        var doctosRelacionados = xmlEntrada.DocumentElement["cfdi:Complemento"][$"{prefijo}:Pagos"][$"{prefijo}:Pago"].GetElementsByTagName($"{prefijo}:DoctoRelacionado");
        //        foreach (XmlElement nodo in doctosRelacionados)
        //        {
        //            var id = Guid.NewGuid();
        //            var idDocumento = nodo.GetAttribute("IdDocumento");
        //            var serie = nodo.HasAttribute("Serie") ? nodo.GetAttribute("Serie") : "";
        //            var folio = nodo.HasAttribute("Folio") ? nodo.GetAttribute("Folio") : "";
        //            var monedaDR = nodo.GetAttribute("MonedaDR");
        //            var tipoCambioDR = Convert.ToDecimal(nodo.HasAttribute("TipoCambioDR") ? nodo.GetAttribute("TipoCambioDR") : "0.00");
        //            var metodoDePagoDR = nodo.GetAttribute("MetodoDePagoDR");
        //            var objetoImpDr = nodo.GetAttribute("ObjetoImpDR");
        //            var numParcialidad = nodo.HasAttribute("NumParcialidad") ? nodo.GetAttribute("NumParcialidad") : "";
        //            var impSaldoAnt = Convert.ToDecimal(nodo.HasAttribute("ImpSaldoAnt") ? nodo.GetAttribute("ImpSaldoAnt") : "0.00");
        //            var impPagado = Convert.ToDecimal(nodo.HasAttribute("ImpPagado") ? nodo.GetAttribute("ImpPagado") : "0.00");
        //            var impSaldoInsoluto = Convert.ToDecimal(nodo.HasAttribute("ImpSaldoInsoluto") ? nodo.GetAttribute("ImpSaldoInsoluto") : "0.00");

        //            DoctoRelacionados.Add(new DoctoRelacionado(id, idDocumento, serie, folio, monedaDR, tipoCambioDR, metodoDePagoDR, objetoImpDr, numParcialidad, impSaldoAnt, impPagado, impSaldoInsoluto));
        //        }
        //    }

        //    return DoctoRelacionados;
        //}

        public static DateTime ObtenerComplementoNominaFechaFinalPago(XmlDocument xmlEntrada)
        {
            return DateTime.Parse(xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"].HasAttribute("FechaFinalPago") ? xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"].Attributes["FechaFinalPago"].Value : null);
        }

        public static DateTime ObtenerComplementoNominaFechaInicialPago(XmlDocument xmlEntrada)
        {
            return DateTime.Parse(xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"].HasAttribute("FechaInicialPago") ? xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"].Attributes["FechaInicialPago"].Value : null);
        }

        public static DateTime ObtenerComplementoNominaFechaPago(XmlDocument xmlEntrada)
        {
            return DateTime.Parse(xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"].HasAttribute("FechaPago") ? xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"].Attributes["FechaPago"].Value : null);
        }

        public static string ObtenerComplementoNominaNumDiasPagados(XmlDocument xmlEntrada)
        {
            return xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"].HasAttribute("NumDiasPagados") ? xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"].Attributes["NumDiasPagados"].Value : null;
        }

        public static string ObtenerComplementoNominaTipoNomina(XmlDocument xmlEntrada)
        {
            return xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"].HasAttribute("TipoNomina") ? xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"].Attributes["TipoNomina"].Value : null;
        }

        public static decimal ObtenerComplementoNominaTotalDeducciones(XmlDocument xmlEntrada)
        {
            return decimal.Parse(xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"].HasAttribute("TotalDeducciones") ? xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"].Attributes["TotalDeducciones"].Value : "0.00");
        }

        public static decimal ObtenerComplementoNominaTotalPercepciones(XmlDocument xmlEntrada)
        {
            return decimal.Parse(xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"].HasAttribute("TotalPercepciones") ? xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"].Attributes["TotalPercepciones"].Value : "0.00");
        }

        public static string ObtenerComplementoNominaVersion(XmlDocument xmlEntrada)
        {
            return xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"].HasAttribute("Version") ? xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"].Attributes["Version"].Value : null;
        }

        public static string ObtenerComplementoNominaRegistroPatronal(XmlDocument xmlEntrada)
        {
            return xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"]["nomina12:Emisor"].HasAttribute("RegistroPatronal") ? xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"]["nomina12:Emisor"].Attributes["RegistroPatronal"].Value : null;
        }

        public static string ObtenerComplementoNominaAntigüedad(XmlDocument xmlEntrada)
        {
            return xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"]["nomina12:Receptor"].HasAttribute("Antigüedad") ? xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"]["nomina12:Receptor"].Attributes["Antigüedad"].Value : null;
        }

        public static string ObtenerComplementoNominaBanco(XmlDocument xmlEntrada)
        {
            return xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"]["nomina12:Receptor"].HasAttribute("NominaBanco") ? xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"]["nomina12:Receptor"].Attributes["NominaBanco"].Value : null;
        }

        public static string ObtenerComplementoNominaClaveEntFed(XmlDocument xmlEntrada)
        {
            return xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"]["nomina12:Receptor"].HasAttribute("ClaveEntFed") ? xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"]["nomina12:Receptor"].Attributes["ClaveEntFed"].Value : null;
        }

        public static string ObtenerComplementoNominaCuentaBancaria(XmlDocument xmlEntrada)
        {
            return xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"]["nomina12:Receptor"].HasAttribute("CuentaBancaria") ? xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"]["nomina12:Receptor"].Attributes["CuentaBancaria"].Value : null;
        }

        public static string ObtenerComplementoNominaCurp(XmlDocument xmlEntrada)
        {
            return xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"]["nomina12:Receptor"].HasAttribute("Curp") ? xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"]["nomina12:Receptor"].Attributes["Curp"].Value : null;
        }

        public static string ObtenerComplementoNominaDepartamento(XmlDocument xmlEntrada)
        {
            return xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"]["nomina12:Receptor"].HasAttribute("Departamento") ? xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"]["nomina12:Receptor"].Attributes["Departamento"].Value : null;
        }

        public static DateTime ObtenerComplementoNominaFechaInicioRelLaboral(XmlDocument xmlEntrada)
        {
            return DateTime.Parse(xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"]["nomina12:Receptor"].HasAttribute("FechaInicioRelLaboral") ? xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"]["nomina12:Receptor"].Attributes["FechaInicioRelLaboral"].Value : null);
        }

        public static string ObtenerComplementoNominaNumEmpleado(XmlDocument xmlEntrada)
        {
            return xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"]["nomina12:Receptor"].HasAttribute("NumEmpleado") ? xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"]["nomina12:Receptor"].Attributes["NumEmpleado"].Value : null;
        }

        public static string ObtenerComplementoNominaNumSeguridadSocial(XmlDocument xmlEntrada)
        {
            return xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"]["nomina12:Receptor"].HasAttribute("NumSeguridadSocial") ? xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"]["nomina12:Receptor"].Attributes["NumSeguridadSocial"].Value : null;
        }

        public static string ObtenerComplementoNominaPeriodicidadPago(XmlDocument xmlEntrada)
        {
            return xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"]["nomina12:Receptor"].HasAttribute("PeriodicidadPago") ? xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"]["nomina12:Receptor"].Attributes["PeriodicidadPago"].Value : null;
        }

        public static string ObtenerComplementoNominaRiesgoPuesto(XmlDocument xmlEntrada)
        {
            return xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"]["nomina12:Receptor"].HasAttribute("RiesgoPuesto") ? xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"]["nomina12:Receptor"].Attributes["RiesgoPuesto"].Value : null;
        }

        public static decimal ObtenerComplementoNominaSalarioBaseCotApor(XmlDocument xmlEntrada)
        {
            return decimal.Parse(xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"]["nomina12:Receptor"].HasAttribute("SalarioBaseCotApor") ? xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"]["nomina12:Receptor"].Attributes["SalarioBaseCotApor"].Value : "0.00");
        }

        public static decimal ObtenerComplementoNominaSalarioDiarioIntegrado(XmlDocument xmlEntrada)
        {
            return decimal.Parse(xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"]["nomina12:Receptor"].HasAttribute("SalarioDiarioIntegrado") ? xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"]["nomina12:Receptor"].Attributes["SalarioDiarioIntegrado"].Value : "0.00");
        }

        public static string ObtenerComplementoNominaSindicalizado(XmlDocument xmlEntrada)
        {
            return xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"]["nomina12:Receptor"].HasAttribute("Sindicalizado") ? xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"]["nomina12:Receptor"].Attributes["Sindicalizado"].Value : null;
        }

        public static string ObtenerComplementoNominaTipoContrato(XmlDocument xmlEntrada)
        {
            return xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"]["nomina12:Receptor"].HasAttribute("TipoContrato") ? xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"]["nomina12:Receptor"].Attributes["TipoContrato"].Value : null;
        }

        public static string ObtenerComplementoNominaTipoJornada(XmlDocument xmlEntrada)
        {
            return xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"]["nomina12:Receptor"].HasAttribute("TipoJornada") ? xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"]["nomina12:Receptor"].Attributes["TipoJornada"].Value : null;
        }

        public static string ObtenerComplementoNominaTipoRegimen(XmlDocument xmlEntrada)
        {
            return xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"]["nomina12:Receptor"].HasAttribute("TipoRegimen") ? xmlEntrada.DocumentElement["cfdi:Complemento"]["nomina12:Nomina"]["nomina12:Receptor"].Attributes["TipoRegimen"].Value : null;
        }

    }
}
