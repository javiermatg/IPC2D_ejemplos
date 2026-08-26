using ChapinCine.Modelo;

namespace ChapinCine.TDA
{
    /// <summary>
    /// Matriz ortogonal enlazada de asientos.
    ///
    /// La estructura se compone de una lista de cabeceras de fila, una
    /// lista de cabeceras de columna y los nodos, enlazados entre sí en
    /// las cuatro direcciones. Los nodos no se almacenan en ninguna
    /// colección adicional: existen mientras las cabeceras y los enlaces
    /// los mantengan referenciados.
    /// </summary>
    public class MallaAsientos
    {
        /// <summary>Cabeceras de fila, una por cada fila de la sala.</summary>
        private ListaCabeceras cabecerasFila;

        /// <summary>Cabeceras de columna, una por cada columna de la sala.</summary>
        private ListaCabeceras cabecerasColumna;

        /// <summary>Cantidad de filas de la malla.</summary>
        public int TotalFilas { get; private set; }

        /// <summary>Cantidad de columnas de la malla.</summary>
        public int TotalColumnas { get; private set; }

        /// <summary>
        /// Crea las cabeceras de todas las filas y columnas. La malla
        /// queda sin nodos: estos se agregan mediante Insertar.
        /// </summary>
        public MallaAsientos(int totalFilas, int totalColumnas)
        {
            TotalFilas = totalFilas;
            TotalColumnas = totalColumnas;

            cabecerasFila = new ListaCabeceras();
            cabecerasColumna = new ListaCabeceras();

            int i = 1;
            while (i <= totalFilas)
            {
                cabecerasFila.Insertar(i);
                i = i + 1;
            }

            int j = 1;
            while (j <= totalColumnas)
            {
                cabecerasColumna.Insertar(j);
                j = j + 1;
            }
        }

        /// <summary>
        /// Inserta un asiento en la posición indicada y lo enlaza con la
        /// estructura existente.
        ///
        /// El método asume que la malla se construye fila por fila y de
        /// izquierda a derecha. Bajo ese orden, al insertar la posición
        /// (i, j) su vecino izquierdo y su vecino superior ya existen y
        /// están referenciados en el campo Ultimo de la cabecera de fila
        /// y de la cabecera de columna respectivamente, por lo que el
        /// enlace no requiere ningún recorrido.
        ///
        /// Los vecinos derecho e inferior quedan en null y serán
        /// establecidos por los nodos que se inserten después.
        /// </summary>
        public bool Insertar(Asiento dato, int fila, int columna)
        {
            NodoCabecera cabeceraFila = cabecerasFila.BuscarPorIndice(fila);
            NodoCabecera cabeceraColumna = cabecerasColumna.BuscarPorIndice(columna);

            if (cabeceraFila == null || cabeceraColumna == null)
            {
                return false;
            }

            NodoAsiento nuevo = new NodoAsiento(dato, fila, columna);

            // Enlace horizontal. Si la fila no tiene nodos, este pasa a
            // ser su punto de acceso; en caso contrario se enlaza en
            // ambos sentidos con el último nodo insertado en la fila.
            if (cabeceraFila.Ultimo == null)
            {
                cabeceraFila.Acceso = nuevo;
            }
            else
            {
                nuevo.Izquierda = cabeceraFila.Ultimo;
                cabeceraFila.Ultimo.Derecha = nuevo;
            }

            cabeceraFila.Ultimo = nuevo;

            // Enlace vertical, con el mismo criterio aplicado a la columna.
            if (cabeceraColumna.Ultimo == null)
            {
                cabeceraColumna.Acceso = nuevo;
            }
            else
            {
                nuevo.Arriba = cabeceraColumna.Ultimo;
                cabeceraColumna.Ultimo.Abajo = nuevo;
            }

            cabeceraColumna.Ultimo = nuevo;

            return true;
        }

        /// <summary>
        /// Devuelve el nodo ubicado en la posición indicada, o null si no
        /// existe. Entra por la cabecera de la fila y avanza hacia la
        /// derecha, por lo que solo recorre una fila.
        /// </summary>
        public NodoAsiento ObtenerNodo(int fila, int columna)
        {
            NodoCabecera cabecera = cabecerasFila.BuscarPorIndice(fila);

            if (cabecera == null)
            {
                return null;
            }

            NodoAsiento actual = cabecera.Acceso;

            while (actual != null)
            {
                if (actual.Columna == columna)
                {
                    return actual;
                }

                actual = actual.Derecha;
            }

            return null;
        }

        /// <summary>
        /// Devuelve el asiento almacenado en la posición indicada, o null
        /// si la posición no existe.
        /// </summary>
        public Asiento ObtenerAsiento(int fila, int columna)
        {
            NodoAsiento nodo = ObtenerNodo(fila, columna);

            if (nodo == null)
            {
                return null;
            }

            return nodo.Dato;
        }

        /// <summary>
        /// Sustituye el contenido de una posición existente.
        ///
        /// No se crea un nodo nuevo ni se modifican los enlaces: solo
        /// cambia el objeto almacenado. Esto permite aplicar cambios
        /// puntuales sobre una malla ya construida, como las etiquetas
        /// asientoVIP del archivo de configuración o el marcado de
        /// asientos ocupados al reservar.
        /// </summary>
        public bool ReemplazarAsiento(int fila, int columna, Asiento nuevo)
        {
            NodoAsiento nodo = ObtenerNodo(fila, columna);

            if (nodo == null)
            {
                return false;
            }

            nodo.Dato = nuevo;
            return true;
        }

        /// <summary>
        /// Devuelve el primer nodo de la fila indicada, para permitir el
        /// recorrido horizontal desde otras capas sin exponer las
        /// cabeceras.
        /// </summary>
        public NodoAsiento ObtenerPrimeroDeFila(int fila)
        {
            NodoCabecera cabecera = cabecerasFila.BuscarPorIndice(fila);

            if (cabecera == null)
            {
                return null;
            }

            return cabecera.Acceso;
        }

        /// <summary>
        /// Devuelve el primer nodo de la columna indicada, para permitir
        /// el recorrido vertical.
        /// </summary>
        public NodoAsiento ObtenerPrimeroDeColumna(int columna)
        {
            NodoCabecera cabecera = cabecerasColumna.BuscarPorIndice(columna);

            if (cabecera == null)
            {
                return null;
            }

            return cabecera.Acceso;
        }

        /// <summary>
        /// Cuenta los asientos de la malla que son reservables por el
        /// cliente indicado. Con null cuenta los que son reservables sin
        /// considerar presupuesto.
        ///
        /// Recorre la malla bajando por las filas y avanzando hacia la
        /// derecha dentro de cada una.
        /// </summary>
        public int ContarReservablesPor(Cliente cliente)
        {
            int total = 0;
            int fila = 1;

            while (fila <= TotalFilas)
            {
                NodoAsiento actual = ObtenerPrimeroDeFila(fila);

                while (actual != null)
                {
                    if (actual.Dato.EsReservablePor(cliente))
                    {
                        total = total + 1;
                    }

                    actual = actual.Derecha;
                }

                fila = fila + 1;
            }

            return total;
        }
    }
}
