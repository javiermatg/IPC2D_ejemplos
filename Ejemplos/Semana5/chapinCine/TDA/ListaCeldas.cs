using ChapinCine.Modelo;

namespace ChapinCine.TDA
{
    /// <summary>
    /// Lista simplemente enlazada de celdas afectadas por una reservación.
    /// Conserva el orden en que las celdas fueron ocupadas.
    /// </summary>
    public class ListaCeldas
    {
        public NodoCelda Primero { get; private set; }
        public int Cantidad { get; private set; }

        /// <summary>
        /// Último nodo insertado. Se conserva para que la inserción no
        /// requiera recorrer la lista completa.
        /// </summary>
        private NodoCelda ultimo;

        public ListaCeldas()
        {
            Primero = null;
            ultimo = null;
            Cantidad = 0;
        }

        public bool EstaVacia()
        {
            return Primero == null;
        }

        /// <summary>
        /// Agrega al final una celda con su posición y el asiento que la
        /// ocupaba antes de la reservación.
        /// </summary>
        public void Insertar(int fila, int columna, Asiento asientoOriginal)
        {
            NodoCelda nuevo = new NodoCelda(fila, columna, asientoOriginal);

            if (Primero == null)
            {
                Primero = nuevo;
            }
            else
            {
                ultimo.Siguiente = nuevo;
            }

            ultimo = nuevo;
            Cantidad = Cantidad + 1;
        }
    }
}
