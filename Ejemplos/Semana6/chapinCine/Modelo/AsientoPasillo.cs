namespace ChapinCine.Modelo
{
    /// <summary>
    /// Estructura o pasillo, correspondiente al carácter '*' del archivo
    /// de configuración. Nunca es reservable.
    /// </summary>
    public class AsientoPasillo : Asiento
    {
        public override char Simbolo
        {
            get { return '*'; }
        }

        public override string ObtenerTipo()
        {
            return "Pasillo";
        }

        /// <summary>
        /// Devuelve siempre false, independientemente del cliente.
        /// </summary>
        public override bool EsReservablePor(Cliente cliente)
        {
            return false;
        }
    }
}
