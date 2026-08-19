using ChapinCine.Modelo;

namespace ChapinCine.TDA
{
    /// <summary>
    /// Nodo de la cola de clientes.
    /// Almacena un objeto Cliente y la referencia al siguiente nodo.
    /// </summary>
    public class NodoCola
    {
        public Cliente Dato { get; set; }
        public NodoCola Siguiente { get; set; }

        public NodoCola(Cliente dato)
        {
            Dato = dato;
            Siguiente = null;
        }
    }

    /// <summary>
    /// Tipo de dato abstracto: cola de clientes con política FIFO
    /// (el primer elemento en entrar es el primero en salir).
    /// Se utiliza para representar la fila de atención en taquilla.
    /// Mantiene referencias al frente y al final para que encolar y
    /// desencolar no requieran recorrer la estructura.
    /// </summary>
    public class ColaClientes
    {
        /// <summary>Primer nodo de la cola. Es el próximo en salir.</summary>
        private NodoCola frente;

        /// <summary>Último nodo de la cola. Punto de inserción.</summary>
        private NodoCola final;

        /// <summary>Cantidad de elementos almacenados.</summary>
        public int Cantidad { get; private set; }

        public ColaClientes()
        {
            frente = null;
            final = null;
            Cantidad = 0;
        }

        /// <summary>Indica si la cola no contiene elementos.</summary>
        public bool EstaVacia()
        {
            return frente == null;
        }

        /// <summary>
        /// Inserta un cliente al final de la cola.
        /// Cuando la cola está vacía el nuevo nodo pasa a ser
        /// simultáneamente el frente y el final.
        /// </summary>
        public void Encolar(Cliente cliente)
        {
            NodoCola nuevo = new NodoCola(cliente);

            if (final == null)
            {
                frente = nuevo;
                final = nuevo;
            }
            else
            {
                final.Siguiente = nuevo;
                final = nuevo;
            }

            Cantidad = Cantidad + 1;
        }

        /// <summary>
        /// Extrae y devuelve el cliente ubicado al frente de la cola.
        /// Cuando se extrae el último elemento se limpia también la
        /// referencia al final para dejar la estructura consistente.
        /// Devuelve null si la cola está vacía.
        /// </summary>
        public Cliente Desencolar()
        {
            if (frente == null)
            {
                return null;
            }

            Cliente dato = frente.Dato;
            frente = frente.Siguiente;

            if (frente == null)
            {
                final = null;
            }

            Cantidad = Cantidad - 1;
            return dato;
        }

        /// <summary>
        /// Devuelve el cliente ubicado al frente sin extraerlo de la cola.
        /// Devuelve null si la cola está vacía.
        /// </summary>
        public Cliente VerFrente()
        {
            if (frente == null)
            {
                return null;
            }

            return frente.Dato;
        }
    }
}
