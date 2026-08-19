namespace ChapinCine.TDA
{
    /// <summary>
    /// Nodo de la pila de operaciones.
    /// Almacena la descripción de una operación y la referencia al
    /// siguiente nodo.
    /// </summary>
    public class NodoPila
    {
        public string Dato { get; set; }
        public NodoPila Siguiente { get; set; }

        public NodoPila(string dato)
        {
            Dato = dato;
            Siguiente = null;
        }
    }

    /// <summary>
    /// Tipo de dato abstracto: pila con política LIFO (el último elemento
    /// en entrar es el primero en salir).
    /// Se utiliza para registrar el historial de operaciones realizadas
    /// en el sistema, quedando la más reciente en la cima.
    /// La inserción y extracción se realizan siempre en la cima, por lo
    /// que ninguna operación requiere recorrer la estructura.
    /// </summary>
    public class PilaOperaciones
    {
        /// <summary>Nodo superior de la pila.</summary>
        private NodoPila cima;

        /// <summary>Cantidad de elementos almacenados.</summary>
        public int Cantidad { get; private set; }

        public PilaOperaciones()
        {
            cima = null;
            Cantidad = 0;
        }

        /// <summary>Indica si la pila no contiene elementos.</summary>
        public bool EstaVacia()
        {
            return cima == null;
        }

        /// <summary>
        /// Inserta una descripción de operación en la cima de la pila.
        /// El nuevo nodo apunta a la cima anterior y luego pasa a ocupar
        /// su lugar.
        /// </summary>
        public void Apilar(string descripcion)
        {
            NodoPila nuevo = new NodoPila(descripcion);

            nuevo.Siguiente = cima;
            cima = nuevo;

            Cantidad = Cantidad + 1;
        }

        /// <summary>
        /// Extrae y devuelve la descripción ubicada en la cima.
        /// Devuelve null si la pila está vacía.
        /// </summary>
        public string Desapilar()
        {
            if (cima == null)
            {
                return null;
            }

            string dato = cima.Dato;
            cima = cima.Siguiente;
            Cantidad = Cantidad - 1;

            return dato;
        }

        /// <summary>
        /// Devuelve el nodo de la cima para permitir recorrer el
        /// historial sin extraer sus elementos.
        /// </summary>
        public NodoPila VerCima()
        {
            return cima;
        }
    }
}
