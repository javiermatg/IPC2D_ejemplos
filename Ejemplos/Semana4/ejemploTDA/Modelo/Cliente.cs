namespace ChapinCine.Modelo
{
    /// <summary>
    /// Clase base abstracta de la jerarquía de clientes.
    /// Define el contrato común que toda subclase debe implementar, de modo
    /// que el resto del sistema pueda operar sobre cualquier tipo de cliente
    /// sin conocer su tipo concreto.
    /// </summary>
    public abstract class Cliente
    {
        /// <summary>Nombre que identifica al cliente de forma única.</summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Constructor protegido. Se restringe el acceso a las subclases
        /// porque la clase es abstracta y no debe instanciarse directamente.
        /// </summary>
        protected Cliente(string nombre)
        {
            Nombre = nombre;
        }

        /// <summary>Devuelve el nombre del tipo concreto de cliente.</summary>
        public abstract string ObtenerTipo();

        /// <summary>
        /// Devuelve una descripción del cliente incluyendo los datos
        /// particulares de su tipo.
        /// </summary>
        public abstract string ObtenerDescripcion();

        /// <summary>
        /// Determina si el cliente puede cubrir el recargo indicado.
        /// Se utiliza para validar el acceso a asientos con costo adicional.
        /// </summary>
        public abstract bool PuedeCubrirRecargo(int recargo);

        /// <summary>
        /// Descuenta el recargo del presupuesto disponible del cliente.
        /// Modifica el estado del objeto de forma permanente.
        /// </summary>
        public abstract void AplicarCobro(int recargo);
    }
}
