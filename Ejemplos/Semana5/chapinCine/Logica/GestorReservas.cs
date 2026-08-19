using ChapinCine.Modelo;
using ChapinCine.TDA;

namespace ChapinCine.Logica
{
    /// <summary>
    /// Capa de lógica del sistema. Contiene las operaciones de negocio
    /// sobre la malla: búsqueda de asientos disponibles, reservación y
    /// reversión de reservaciones.
    ///
    /// No interactúa con el usuario ni con archivos: recibe datos,
    /// resuelve y devuelve un resultado.
    /// </summary>
    public class GestorReservas
    {
        /// <summary>
        /// Busca un bloque de asientos contiguos en una misma fila que el
        /// cliente pueda reservar, y lo reserva.
        ///
        /// Recorre la malla fila por fila y prueba a iniciar el bloque en
        /// cada posición, avanzando hacia la derecha. Devuelve el primer
        /// bloque que se logre completar, sin buscar el mejor.
        ///
        /// Devuelve la Reservacion creada, o null si no fue posible.
        /// </summary>
        public Reservacion Reservar(Sala sala, Cliente cliente, int cantidad)
        {
            if (sala == null || cliente == null || cantidad < 1)
            {
                return null;
            }

            if (!sala.TieneMalla())
            {
                return null;
            }

            MallaAsientos malla = sala.Asientos;

            int fila = 1;

            while (fila <= malla.TotalFilas)
            {
                NodoAsiento inicio = malla.ObtenerPrimeroDeFila(fila);

                while (inicio != null)
                {
                    Reservacion reservacion =
                        IntentarBloque(sala, cliente, inicio, cantidad);

                    if (reservacion != null)
                    {
                        OcuparCeldas(malla, reservacion);
                        return reservacion;
                    }

                    inicio = inicio.Derecha;
                }

                fila = fila + 1;
            }

            return null;
        }

        /// <summary>
        /// Intenta armar un bloque de asientos contiguos a partir del nodo
        /// indicado.
        ///
        /// El cobro se aplica sobre el cliente a medida que se avanza, de
        /// modo que cada asiento del bloque se evalúa con el presupuesto
        /// ya reducido por los anteriores. Evaluar los asientos de forma
        /// independiente daría un resultado incorrecto cuando el bloque
        /// contiene varios asientos con recargo.
        ///
        /// Si el bloque no se completa, se revierte todo lo cobrado
        /// durante el intento y el cliente queda como estaba. La malla no
        /// se modifica durante el intento.
        ///
        /// Devuelve la Reservacion si el bloque se completó, o null.
        /// </summary>
        private Reservacion IntentarBloque(Sala sala, Cliente cliente,
                                           NodoAsiento inicio, int cantidad)
        {
            ListaCeldas celdas = new ListaCeldas();

            NodoAsiento actual = inicio;
            int tomados = 0;
            int cobrado = 0;

            while (actual != null && tomados < cantidad)
            {
                if (!actual.Dato.EsReservablePor(cliente))
                {
                    break;
                }

                int recargo = actual.Dato.ObtenerRecargo();

                cliente.AplicarCobro(recargo);
                cobrado = cobrado + recargo;

                celdas.Insertar(actual.Fila, actual.Columna, actual.Dato);

                tomados = tomados + 1;
                actual = actual.Derecha;
            }

            if (tomados == cantidad)
            {
                return new Reservacion(sala, cliente, inicio.Fila, inicio.Columna,
                                       cantidad, cobrado, celdas);
            }

            cliente.RevertirCobro(cobrado);

            return null;
        }

        /// <summary>
        /// Marca como ocupadas todas las celdas del bloque reservado.
        /// Se invoca únicamente cuando el bloque ya quedó confirmado.
        /// </summary>
        private void OcuparCeldas(MallaAsientos malla, Reservacion reservacion)
        {
            NodoCelda celda = reservacion.Celdas.Primero;

            while (celda != null)
            {
                malla.ReemplazarAsiento(celda.Fila, celda.Columna, new AsientoOcupado());
                celda = celda.Siguiente;
            }
        }

        /// <summary>
        /// Deshace una reservación: restaura en cada celda el asiento que
        /// tenía antes de la operación y devuelve al cliente el monto
        /// cobrado.
        /// </summary>
        public bool Deshacer(Reservacion reservacion)
        {
            if (reservacion == null)
            {
                return false;
            }

            if (reservacion.Sala == null || !reservacion.Sala.TieneMalla())
            {
                return false;
            }

            MallaAsientos malla = reservacion.Sala.Asientos;

            NodoCelda celda = reservacion.Celdas.Primero;

            while (celda != null)
            {
                malla.ReemplazarAsiento(celda.Fila, celda.Columna, celda.AsientoOriginal);
                celda = celda.Siguiente;
            }

            reservacion.Cliente.RevertirCobro(reservacion.MontoTotal);

            return true;
        }

        /// <summary>
        /// Calcula la corrida más larga de asientos contiguos que el
        /// cliente podría tomar en la sala.
        ///
        /// Evalúa cada asiento de forma independiente, con el presupuesto
        /// completo del cliente y sin aplicar cobros, por lo que el valor
        /// devuelto es un límite superior y no una garantía: un bloque con
        /// varios asientos con recargo puede resultar impagable aunque
        /// cada asiento por separado sí fuera alcanzable.
        ///
        /// Se utiliza únicamente para dar un mensaje orientativo cuando
        /// una reservación no puede completarse.
        /// </summary>
        public int CalcularBloqueMaximo(Sala sala, Cliente cliente)
        {
            if (sala == null || !sala.TieneMalla())
            {
                return 0;
            }

            MallaAsientos malla = sala.Asientos;

            int mejor = 0;
            int fila = 1;

            while (fila <= malla.TotalFilas)
            {
                NodoAsiento actual = malla.ObtenerPrimeroDeFila(fila);
                int corrida = 0;

                while (actual != null)
                {
                    if (actual.Dato.EsReservablePor(cliente))
                    {
                        corrida = corrida + 1;

                        if (corrida > mejor)
                        {
                            mejor = corrida;
                        }
                    }
                    else
                    {
                        corrida = 0;
                    }

                    actual = actual.Derecha;
                }

                fila = fila + 1;
            }

            return mejor;
        }
    }
}
