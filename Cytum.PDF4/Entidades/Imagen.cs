namespace Cytrum.PDF4.Entidades
{
    public class Imagen
    {
        public string Ruta { get; set; }
        public Coordenadas Coordenadas { get; set; }
        public float Ancho { get; set; }
        public float Alto { get; set; }
        public Imagen()
        {
            Coordenadas = new Coordenadas();
        }
    }
}