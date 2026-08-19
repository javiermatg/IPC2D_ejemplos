namespace ChapinCine.TDA
{
    /// <summary>
    /// Lista simplemente enlazada de cabeceras.
    /// La utiliza MallaAsientos para almacenar las cabeceras de fila y de
    /// columna de la matriz ortogonal.
    /// </summary>
    public class ListaCabeceras
    {
        /// <summary>Primera cabecera de la lista.</summary>
        public NodoCabecera Primero { get; private set; }

        /// <summary>Cantidad de cabeceras almacenadas.</summary>
        public int Cantidad { get; private set; }

        /// <summary>
        /// Última cabecera insertada. Se conserva para que la inserción no
        /// requiera recorrer la lista completa.
        /// </summary>
        private NodoCabecera ultimo;

        public ListaCabeceras()
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
        /// Crea e inserta una cabecera con el índice indicado y la
        /// devuelve, para que pueda utilizarse de inmediato.
        /// </summary>
        public NodoCabecera Insertar(int indice)
        {
            NodoCabecera nueva = new NodoCabecera(indice);

            if (Primero == null)
            {
                Primero = nueva;
            }
            else
            {
                ultimo.Siguiente = nueva;
            }

            ultimo = nueva;
            Cantidad = Cantidad + 1;

            return nueva;
        }

        /// <summary>
        /// Busca la cabecera correspondiente al índice indicado.
        /// Devuelve null si no existe.
        /// </summary>
        public NodoCabecera BuscarPorIndice(int indice)
        {
            NodoCabecera actual = Primero;

            while (actual != null)
            {
                if (actual.Indice == indice)
                {
                    return actual;
                }

                actual = actual.Siguiente;
            }

            return null;
        }
    }
}
