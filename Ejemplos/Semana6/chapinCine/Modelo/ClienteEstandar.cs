namespace ChapinCine.Modelo
{
    /// <summary>
    /// Cliente sin presupuesto para recargos.
    /// No puede ocupar asientos que tengan costo adicional.
    /// </summary>
    public class ClienteEstandar : Cliente
    {
        /// <summary>
        /// Invoca el constructor de la clase base para asignar el nombre.
        /// </summary>
        public ClienteEstandar(string nombre) : base(nombre)
        {
        }

        public override string ObtenerTipo()
        {
            return "ClienteEstandar";
        }

        public override string ObtenerDescripcion()
        {
            return Nombre + " (ClienteEstandar - sin presupuesto para recargos)";
        }

        /// <summary>
        /// Devuelve siempre false: este tipo de cliente no cubre recargos.
        /// </summary>
        public override bool PuedeCubrirRecargo(int recargo)
        {
            return false;
        }

        /// <summary>
        /// Sin implementación. Este tipo de cliente no maneja presupuesto,
        /// por lo que no hay estado que modificar.
        /// </summary>
        public override void AplicarCobro(int recargo)
        {
        }

        /// <summary>
        /// Sin implementación: este cliente nunca recibe cobros, por lo
        /// que tampoco hay nada que revertir.
        /// </summary>
        public override void RevertirCobro(int monto)
        {
        }
    }
}
