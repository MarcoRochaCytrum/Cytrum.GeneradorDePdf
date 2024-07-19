namespace Cytrum.PDF4.Entidades
{
    public class Numerador
    {
        public Coordenadas Coordenadas { get; set; }
        public Fuente Fuente { get; set; }
        public string Separador { get; set; }
        public bool EsNumeracionLarga { get; set; }
        public Numerador()
        {
            EsNumeracionLarga = true;
            Coordenadas = new Coordenadas();
            Fuente = new Fuente();
        }
    }
}