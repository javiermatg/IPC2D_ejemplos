using ChapinCine.TDA;

namespace ChapinCine.Modelo
{
    /// <summary>
    /// Representa una reservación realizada en el sistema.
    /// Almacena toda la información necesaria para describir la operación
    /// y para revertirla: cliente, sala, ubicación, monto cobrado y el
    /// estado que tenían las celdas antes de ser ocupadas.
    /// </summary>
    public class Reservacion
    {
        /// <summary>Sala donde se realizó la reservación.</summary>
        public Sala Sala { get; private set; }

        /// <summary>Cliente que realizó la reservación.</summary>
        public Cliente Cliente { get; private set; }

        /// <summary>Fila donde quedó el bloque de asientos.</summary>
        public int Fila { get; private set; }

        /// <summary>Columna donde inicia el bloque.</summary>
        public int ColumnaInicial { get; private set; }

        /// <summary>Cantidad de asientos reservados.</summary>
        public int Cantidad { get; private set; }

        /// <summary>Monto total cobrado por los recargos del bloque.</summary>
        public int MontoTotal { get; private set; }

        /// <summary>
        /// Celdas afectadas, con el asiento que ocupaba cada posición
        /// antes de la reservación. Permite restaurar el estado anterior.
        /// </summary>
        public ListaCeldas Celdas { get; private set; }

        public Reservacion(Sala sala, Cliente cliente, int fila, int columnaInicial,
                           int cantidad, int montoTotal, ListaCeldas celdas)
        {
            Sala = sala;
            Cliente = cliente;
            Fila = fila;
            ColumnaInicial = columnaInicial;
            Cantidad = cantidad;
            MontoTotal = montoTotal;
            Celdas = celdas;
        }

        /// <summary>
        /// Descripción legible de la reservación, para mostrarla en los
        /// listados y en el historial.
        /// </summary>
        public string ObtenerDescripcion()
        {
            string texto = Cliente.Nombre + " - " + Cantidad + " asiento(s) en '"
                           + Sala.Nombre + "', fila " + Fila
                           + ", columnas " + ColumnaInicial
                           + " a " + (ColumnaInicial + Cantidad - 1);

            if (MontoTotal > 0)
            {
                texto = texto + " - recargo Q" + MontoTotal;
            }

            return texto;
        }
    }
}
