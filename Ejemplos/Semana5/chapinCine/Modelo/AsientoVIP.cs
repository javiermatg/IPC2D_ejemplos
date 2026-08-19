namespace ChapinCine.Modelo
{
    /// <summary>
    /// Asiento con costo adicional. No proviene de un carácter de la
    /// malla, sino de una etiqueta asientoVIP del archivo, que indica su
    /// posición y el monto del recargo.
    /// Es el único tipo de asiento cuya disponibilidad depende del
    /// cliente que la consulta.
    /// </summary>
    public class AsientoVIP : Asiento
    {
        /// <summary>Costo adicional para ocupar este asiento.</summary>
        public int Recargo { get; private set; }

        public AsientoVIP(int recargo)
        {
            Recargo = recargo;
        }

        public override char Simbolo
        {
            get { return 'V'; }
        }

        public override string ObtenerTipo()
        {
            return "VIP";
        }

        /// <summary>
        /// Delega la decisión al cliente, que es quien conoce su
        /// presupuesto disponible. Cuando no se recibe un cliente
        /// concreto, se informa que el asiento es reservable.
        /// </summary>
        public override bool EsReservablePor(Cliente cliente)
        {
            if (cliente == null)
            {
                return true;
            }

            return cliente.PuedeCubrirRecargo(Recargo);
        }

        public override int ObtenerRecargo()
        {
            return Recargo;
        }
    }
}
