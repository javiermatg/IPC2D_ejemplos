using ChapinCine.Modelo;

namespace ChapinCine.TDA
{
    /// <summary>
    /// Nodo de la matriz ortogonal enlazada.
    /// Almacena un asiento, su posición dentro de la malla y una
    /// referencia hacia cada uno de sus cuatro vecinos, lo que permite
    /// consultar celdas adyacentes sin recorrer la estructura.
    /// </summary>
    public class NodoAsiento
    {
        /// <summary>Objeto Asiento almacenado en esta posición.</summary>
        public Asiento Dato { get; set; }

        /// <summary>Fila que ocupa el nodo, numerada desde 1.</summary>
        public int Fila { get; set; }

        /// <summary>Columna que ocupa el nodo, numerada desde 1.</summary>
        public int Columna { get; set; }

        /// <summary>Nodo de la fila anterior en la misma columna.</summary>
        public NodoAsiento Arriba { get; set; }

        /// <summary>Nodo de la fila siguiente en la misma columna.</summary>
        public NodoAsiento Abajo { get; set; }

        /// <summary>Nodo de la columna anterior en la misma fila.</summary>
        public NodoAsiento Izquierda { get; set; }

        /// <summary>Nodo de la columna siguiente en la misma fila.</summary>
        public NodoAsiento Derecha { get; set; }

        /// <summary>
        /// Crea el nodo sin enlaces. Los enlaces con los nodos vecinos los
        /// establece MallaAsientos al insertarlo en la estructura.
        /// </summary>
        public NodoAsiento(Asiento dato, int fila, int columna)
        {
            Dato = dato;
            Fila = fila;
            Columna = columna;
            Arriba = null;
            Abajo = null;
            Izquierda = null;
            Derecha = null;
        }
    }
}
