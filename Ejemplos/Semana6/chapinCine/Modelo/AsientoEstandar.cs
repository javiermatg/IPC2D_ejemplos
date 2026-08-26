namespace ChapinCine.Modelo
{
    /// <summary>
    /// Asiento disponible sin costo adicional, correspondiente al
    /// carácter espacio del archivo de configuración.
    /// </summary>
    public class AsientoEstandar : Asiento
    {
        public override char Simbolo
        {
            get { return ' '; }
        }

        public override string ObtenerTipo()
        {
            return "Estandar";
        }

        /// <summary>
        /// Devuelve siempre true: no impone ninguna condición al cliente.
        /// </summary>
        public override bool EsReservablePor(Cliente cliente)
        {
            return true;
        }
    }
}
