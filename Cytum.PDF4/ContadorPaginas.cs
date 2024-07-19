using Cytrum.PDF4.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cytrum.PDF4
{
    public static class ContadorPaginas
    {
        public static Dictionary<int, int> ObtenerRelacionDetalle(Configuracion configuracion)
        {
            var totalLineas = 0;
            var detalleLineas = new Dictionary<int, int>();
            var detalleKey = 1;
            
            foreach (Detalle detalle in configuracion.Detalles)
            {
                int maxLineasColumna = 0;
                foreach (Columna columna in detalle.Columnas)
                {
                    int lineasRenglonColumna = 0;
                    foreach (RenglonColumna renglon in columna.RenglonesColumna)
                        lineasRenglonColumna += CalcularNumeroRenglonesPorRenglonColumna(renglon);
                    maxLineasColumna = lineasRenglonColumna > maxLineasColumna ? lineasRenglonColumna : maxLineasColumna;
                }
                detalleLineas.Add(detalleKey, maxLineasColumna + configuracion.LineasDespuesDeDetalle);
                ++detalleKey;
                totalLineas += maxLineasColumna + configuracion.LineasDespuesDeDetalle;
            }

            var lineasPorHojaUnica = CalcularNumeroRenglonesPorHoja(configuracion.UnicaHoja, configuracion);
            var lineasPrimeraHoja = CalcularNumeroRenglonesPorHoja(configuracion.PrimeraHoja, configuracion);
            var lineasSegundaHoja = CalcularNumeroRenglonesPorHoja(configuracion.SegundaHoja, configuracion);
            var lineasTerceraHoja = CalcularNumeroRenglonesPorHoja(configuracion.TerceraHoja, configuracion);
            var resultado = new Dictionary<int, int>();

            if (totalLineas <= lineasPorHojaUnica)
            {
                resultado.Add(1, configuracion.Detalles.Count());
                return resultado;
            }

            var lineasOcupadas = 0;
            var esSegundaHoja = false;
            var hojaKey = 1;

            foreach (var detalle in detalleLineas)
            {
                if (esSegundaHoja)
                {
                    var espacioNecesario = detalle.Value;
                    var lineasParaDetalle = ObtenerNumeroRenglonesParaDetalle(lineasSegundaHoja, lineasOcupadas, espacioNecesario, configuracion.LineasDespuesDeDetalle);
                    
                    if (lineasParaDetalle > 0)
                    {
                        lineasOcupadas += lineasParaDetalle;
                    }
                    else
                    {
                        lineasOcupadas = 0;
                        resultado.Add(hojaKey, detalle.Key);
                        ++hojaKey;
                    }
                }

                if (!esSegundaHoja)
                {
                    var espacioNecesario = detalle.Value;
                    var lineasParaDetalle = ObtenerNumeroRenglonesParaDetalle(lineasPrimeraHoja, lineasOcupadas, espacioNecesario, configuracion.LineasDespuesDeDetalle);
                    
                    if (lineasParaDetalle > 0)
                    {
                        lineasOcupadas += lineasParaDetalle;
                    }
                    else
                    {
                        lineasOcupadas = 0;
                        esSegundaHoja = true;
                        resultado.Add(hojaKey, detalle.Key);
                        ++hojaKey;
                    }
                }
            }

            if (resultado.Count == 0)
            {
                resultado.Add(1, detalleLineas.Last().Key);
            }
            else
            {
                if (resultado.Last().Value != detalleLineas.Count)
                {
                    resultado.Add(resultado.Last().Key + 1, detalleLineas.Last().Key);
                }
            }

            if (resultado.Count == 1 && totalLineas > lineasPorHojaUnica)
                resultado.Add(2, 0);

            if (resultado.Count > 1)
            {
                var desde = resultado[resultado.Count - 1] + 1;
                var hasta = resultado[resultado.Count];
                var lineasOcupadasPorHoja = 0;

                for (var index = desde; index <= hasta; ++index)
                    lineasOcupadasPorHoja += detalleLineas[index];

                if (lineasOcupadasPorHoja > lineasTerceraHoja)
                {
                    resultado.Add(resultado.Last().Key + 1, 0);
                }
            }
            return resultado;
        }

        private static int ObtenerNumeroRenglonesParaDetalle(int renglonesPrimeraHoja, int numeroRenglonesOcupados, int espacioNecesitado, int lineasDespuesDetalle)
        {
            if (renglonesPrimeraHoja - numeroRenglonesOcupados >= espacioNecesitado)
                return espacioNecesitado;
            if (renglonesPrimeraHoja - numeroRenglonesOcupados >= espacioNecesitado - lineasDespuesDetalle)
                return espacioNecesitado - lineasDespuesDetalle;

            return 0;
        }

        private static int CalcularNumeroRenglonesPorHoja(FormatoFactura formatoFactura, Configuracion configuracion)
        {
            return (int)Math.Floor(((double)formatoFactura.AreaDetalle.CoordenadaYFin - (double)formatoFactura.AreaDetalle.CoordenadaYInicio) / configuracion.AltoLinea);
        }

        private static int CalcularNumeroRenglonesPorRenglonColumna(RenglonColumna renglon)
        {
            if (renglon.Texto.Length > renglon.MaximoNumeroDeCaracteres && renglon.MaximoNumeroDeCaracteres > 0)
                return (int)Math.Floor(renglon.Texto.Length / renglon.MaximoNumeroDeCaracteres + decimal.One);
            return 1;
        }
    }
}
