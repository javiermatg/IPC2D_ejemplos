namespace ChapinCine.TDA
{
    /// <summary>
    /// Cabecera de una fila o de una columna de la matriz ortogonal.
    ///
    /// Mantiene dos referencias a nodos con propósitos distintos: Acceso
    /// marca el inicio de la línea y permite recorrerla, mientras que
    /// Ultimo marca el punto de enlace para la siguiente inserción y
    /// evita tener que recorrer la línea en cada alta.
    ///
    /// La cabecera es a su vez un nodo de lista, porque las cabeceras se
    /// almacenan en una ListaCabeceras.
    /// </summary>
    public class NodoCabecera
    {
        /// <summary>Número de fila o de columna que representa, desde 1.</summary>
        public int Indice { get; set; }

        /// <summary>
        /// Primer nodo de la fila o columna. Vale null si la línea aún no
        /// tiene nodos.
        /// </summary>
        public NodoAsiento Acceso { get; set; }

        /// <summary>
        /// Último nodo insertado en la fila o columna. Es el nodo con el
        /// que se enlazará la siguiente inserción.
        /// </summary>
        public NodoAsiento Ultimo { get; set; }

        /// <summary>Siguiente cabecera dentro de la lista de cabeceras.</summary>
        public NodoCabecera Siguiente { get; set; }

        public NodoCabecera(int indice)
        {
            Indice = indice;
            Acceso = null;
            Ultimo = null;
            Siguiente = null;
        }
    }
}
