namespace ChapinCine.Modelo
{
    /// <summary>
    /// Clase base abstracta de la jerarquía de asientos.
    /// Define el contrato que toda subclase debe implementar, de modo que
    /// la malla pueda operar sobre cualquier tipo de asiento sin conocer
    /// su tipo concreto.
    /// </summary>
    public abstract class Asiento
    {
        /// <summary>Carácter utilizado para representar el asiento en pantalla.</summary>
        public abstract char Simbolo { get; }

        /// <summary>Nombre del tipo concreto de asiento.</summary>
        public abstract string ObtenerTipo();

        /// <summary>
        /// Determina si el cliente indicado puede reservar este asiento.
        /// El resultado depende del tipo de asiento y, en el caso de los
        /// asientos con recargo, del presupuesto del cliente.
        /// Admite null cuando se desea evaluar la disponibilidad sin
        /// considerar a un cliente en particular.
        /// </summary>
        public abstract bool EsReservablePor(Cliente cliente);

        /// <summary>
        /// Costo adicional que implica ocupar este asiento.
        /// Es virtual y no abstracto porque la mayoría de las subclases
        /// comparten el valor cero; solo AsientoVIP lo redefine.
        /// </summary>
        public virtual int ObtenerRecargo()
        {
            return 0;
        }
    }
}
