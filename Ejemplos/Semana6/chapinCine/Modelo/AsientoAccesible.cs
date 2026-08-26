namespace ChapinCine.Modelo
{
    /// <summary>
    /// Espacio reservado para silla de ruedas, correspondiente al
    /// carácter 'E' del archivo de configuración. Es reservable.
    /// </summary>
    public class AsientoAccesible : Asiento
    {
        public override char Simbolo
        {
            get { return 'E'; }
        }

        public override string ObtenerTipo()
        {
            return "Accesible";
        }

        public override bool EsReservablePor(Cliente cliente)
        {
            return true;
        }
    }
}
