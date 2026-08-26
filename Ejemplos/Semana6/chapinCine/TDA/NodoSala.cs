using ChapinCine.Modelo;

namespace ChapinCine.TDA
{
    /// <summary>
    /// Nodo de la lista enlazada de salas.
    /// Almacena un objeto Sala y la referencia al siguiente nodo de la
    /// estructura. Es la unidad básica de memoria dinámica utilizada por
    /// ListaSalas.
    /// </summary>
    public class NodoSala
    {
        /// <summary>Objeto almacenado en este nodo.</summary>
        public Sala Dato { get; set; }

        /// <summary>
        /// Referencia al siguiente nodo de la lista.
        /// Vale null cuando el nodo es el último de la estructura.
        /// </summary>
        public NodoSala Siguiente { get; set; }

        public NodoSala(Sala dato)
        {
            Dato = dato;
            Siguiente = null;
        }
    }
}
