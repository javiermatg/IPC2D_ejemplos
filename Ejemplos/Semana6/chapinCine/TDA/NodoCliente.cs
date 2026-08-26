using ChapinCine.Modelo;

namespace ChapinCine.TDA
{
    /// <summary>
    /// Nodo de la lista enlazada de clientes.
    /// Almacena un objeto Cliente y la referencia al siguiente nodo.
    /// </summary>
    public class NodoCliente
    {
        /// <summary>Objeto almacenado en este nodo.</summary>
        public Cliente Dato { get; set; }

        /// <summary>
        /// Referencia al siguiente nodo de la lista.
        /// Vale null cuando el nodo es el último de la estructura.
        /// </summary>
        public NodoCliente Siguiente { get; set; }

        public NodoCliente(Cliente dato)
        {
            Dato = dato;
            Siguiente = null;
        }
    }
}
