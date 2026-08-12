using ChapinCine.Modelo;

namespace ChapinCine.TDA
{
    /// <summary>
    /// Tipo de dato abstracto: lista simplemente enlazada de clientes.
    /// Se utiliza para almacenar en memoria dinámica todos los clientes
    /// registrados en el sistema.
    /// </summary>
    public class ListaClientes
    {
        /// <summary>
        /// Referencia al primer nodo de la lista. Vale null cuando la
        /// estructura está vacía.
        /// </summary>
        public NodoCliente Primero { get; private set; }

        /// <summary>Cantidad de elementos almacenados.</summary>
        public int Cantidad { get; private set; }

        public ListaClientes()
        {
            Primero = null;
            Cantidad = 0;
        }

        /// <summary>Indica si la lista no contiene elementos.</summary>
        public bool EstaVacia()
        {
            return Primero == null;
        }

        /// <summary>
        /// Inserta un cliente al final de la lista.
        /// </summary>
        public void Insertar(Cliente cliente)
        {
            NodoCliente nuevo = new NodoCliente(cliente);

            if (Primero == null)
            {
                Primero = nuevo;
            }
            else
            {
                NodoCliente actual = Primero;

                while (actual.Siguiente != null)
                {
                    actual = actual.Siguiente;
                }

                actual.Siguiente = nuevo;
            }

            Cantidad = Cantidad + 1;
        }

        /// <summary>
        /// Busca un cliente por su nombre. La comparación ignora
        /// mayúsculas y minúsculas. Devuelve null si no existe.
        /// </summary>
        public Cliente BuscarPorNombre(string nombre)
        {
            NodoCliente actual = Primero;

            while (actual != null)
            {
                if (string.Equals(actual.Dato.Nombre, nombre,
                                  System.StringComparison.OrdinalIgnoreCase))
                {
                    return actual.Dato;
                }

                actual = actual.Siguiente;
            }

            return null;
        }

        /// <summary>
        /// Inserta el cliente si no existe, o reemplaza el objeto
        /// almacenado cuando el nombre coincide con uno ya registrado.
        /// Se reemplaza el objeto completo en lugar de copiar sus datos
        /// porque el cliente puede cambiar de tipo concreto entre una
        /// carga y otra.
        /// Devuelve true si se realizó una inserción y false si se
        /// realizó un reemplazo.
        /// </summary>
        public bool InsertarOActualizar(Cliente cliente)
        {
            NodoCliente actual = Primero;

            while (actual != null)
            {
                if (string.Equals(actual.Dato.Nombre, cliente.Nombre,
                                  System.StringComparison.OrdinalIgnoreCase))
                {
                    actual.Dato = cliente;
                    return false;
                }

                actual = actual.Siguiente;
            }

            Insertar(cliente);
            return true;
        }

        /// <summary>
        /// Elimina de la lista el cliente cuyo nombre coincide con el
        /// parámetro recibido. Devuelve true si se eliminó un elemento.
        /// </summary>
        public bool EliminarPorNombre(string nombre)
        {
            if (Primero == null)
            {
                return false;
            }

            NodoCliente actual = Primero;
            NodoCliente anterior = null;

            while (actual != null)
            {
                if (string.Equals(actual.Dato.Nombre, nombre,
                                  System.StringComparison.OrdinalIgnoreCase))
                {
                    if (anterior == null)
                    {
                        Primero = actual.Siguiente;
                    }
                    else
                    {
                        anterior.Siguiente = actual.Siguiente;
                    }

                    Cantidad = Cantidad - 1;
                    return true;
                }

                anterior = actual;
                actual = actual.Siguiente;
            }

            return false;
        }

        /// <summary>
        /// Cuenta cuántos clientes almacenados corresponden al tipo
        /// indicado. Se utiliza para validar la disponibilidad de un tipo
        /// de cliente antes de ejecutar una operación que lo requiera.
        /// </summary>
        public int ContarPorTipo(string tipo)
        {
            int contador = 0;
            NodoCliente actual = Primero;

            while (actual != null)
            {
                if (string.Equals(actual.Dato.ObtenerTipo(), tipo,
                                  System.StringComparison.OrdinalIgnoreCase))
                {
                    contador = contador + 1;
                }

                actual = actual.Siguiente;
            }

            return contador;
        }

        /// <summary>
        /// Devuelve el cliente ubicado en la posición indicada,
        /// comenzando en cero. Devuelve null si el índice está fuera
        /// de rango.
        /// </summary>
        public Cliente ObtenerPorIndice(int indice)
        {
            if (indice < 0 || indice >= Cantidad)
            {
                return null;
            }

            NodoCliente actual = Primero;
            int contador = 0;

            while (contador < indice)
            {
                actual = actual.Siguiente;
                contador = contador + 1;
            }

            return actual.Dato;
        }
    }
}
