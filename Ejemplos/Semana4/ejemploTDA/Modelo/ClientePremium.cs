namespace ChapinCine.Modelo
{
    /// <summary>
    /// Cliente que dispone de un presupuesto para cubrir recargos de
    /// asientos con costo adicional. El presupuesto se consume conforme
    /// se aplican cobros.
    /// </summary>
    public class ClientePremium : Cliente
    {
        /// <summary>Presupuesto disponible en el momento actual.</summary>
        public int Presupuesto { get; private set; }

        /// <summary>
        /// Presupuesto con el que se creó el cliente. Se conserva para
        /// poder reportar el valor inicial y para restaurar el estado.
        /// </summary>
        public int PresupuestoInicial { get; private set; }

        public ClientePremium(string nombre, int presupuesto) : base(nombre)
        {
            Presupuesto = presupuesto;
            PresupuestoInicial = presupuesto;
        }

        public override string ObtenerTipo()
        {
            return "ClientePremium";
        }

        public override string ObtenerDescripcion()
        {
            return Nombre + " (ClientePremium - presupuesto: Q" + Presupuesto + ")";
        }

        /// <summary>
        /// Devuelve true si el presupuesto disponible es estrictamente
        /// mayor al recargo. Un presupuesto igual al recargo no lo cubre.
        /// </summary>
        public override bool PuedeCubrirRecargo(int recargo)
        {
            return Presupuesto > recargo;
        }

        /// <summary>
        /// Resta el recargo del presupuesto disponible.
        /// </summary>
        public override void AplicarCobro(int recargo)
        {
            Presupuesto = Presupuesto - recargo;
        }

        /// <summary>
        /// Restaura el presupuesto a su valor inicial.
        /// Se utiliza para revertir cobros aplicados durante una operación
        /// que no se completó.
        /// </summary>
        public void ReiniciarPresupuesto()
        {
            Presupuesto = PresupuestoInicial;
        }
    }
}
