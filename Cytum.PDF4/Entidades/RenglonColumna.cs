namespace Cytrum.PDF4.Entidades
{
    public class RenglonColumna
    {
        private string _variable;
        public string Texto
        {
            get
            {
                if (!EsMoneda || string.IsNullOrEmpty(_variable))
                    return _variable;

                return string.Format("{0:C}", (object)double.Parse(_variable));
            }
            set
            {
                _variable = value;
            }
        }

        public bool CortarPalabrasCompletas { get; set; }
        public float EspacioX { get; set; }
        public Fuente Fuente { get; set; }
        public bool EsMoneda { get; set; }
        public int MaximoNumeroDeCaracteres { get; set; }
        public Rectangulo ColorFila { get; set; }
        public class Rectangulo
        {
            public int PosicionX { get; set; }
            public int PosicionY { get; set; }
            public int Alto { get; set; }
            public int Ancho { get; set; }
            public string ColorHex { get; set; }
        }

        public RenglonColumna()
        {
            Fuente = new Fuente();
        }
    }
}