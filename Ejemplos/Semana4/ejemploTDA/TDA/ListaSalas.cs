using ChapinCine.Modelo;

namespace ChapinCine.TDA
{
    /// <summary>
    /// Tipo de dato abstracto: lista simplemente enlazada de salas.
    /// Se utiliza para almacenar en memoria dinámica todas las salas
    /// cargadas en el sistema, permitiendo insertar, buscar, actualizar,
    /// eliminar y recorrer los elementos.
    /// </summary>
    public class ListaSalas
    {
        /// <summary>
        /// Referencia al primer nodo de la lista. Vale null cuando la
        /// estructura está vacía. Se expone en modo lectura para permitir
        /// el recorrido desde otras capas sin exponer la modificación.
        /// </summary>
        public NodoSala Primero { get; private set; }

        /// <summary>
        /// Cantidad de elementos almacenados. Se actualiza en cada
        /// operación de inserción y eliminación.
        /// </summary>
        public int Cantidad { get; private set; }

        public ListaSalas()
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
        /// Inserta una sala al final de la lista.
        /// Si la lista está vacía, el nuevo nodo se convierte en el
        /// primero; en caso contrario se recorre hasta el último nodo
        /// para enlazarlo.
        /// </summary>
        public void Insertar(Sala sala)
        {
            NodoSala nuevo = new NodoSala(sala);

            if (Primero == null)
            {
                Primero = nuevo;
            }
            else
            {
                NodoSala actual = Primero;

                while (actual.Siguiente != null)
                {
                    actual = actual.Siguiente;
                }

                actual.Siguiente = nuevo;
            }

            Cantidad = Cantidad + 1;
        }

        /// <summary>
        /// Busca una sala por su nombre recorriendo la lista.
        /// La comparación ignora mayúsculas y minúsculas.
        /// Devuelve null si no existe ninguna coincidencia.
        /// </summary>
        public Sala BuscarPorNombre(string nombre)
        {
            NodoSala actual = Primero;

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
        /// Inserta la sala si no existe, o actualiza los datos de la sala
        /// existente cuando el nombre coincide con una ya almacenada.
        /// Se utiliza al cargar varios archivos de configuración para
        /// evitar registros duplicados.
        /// Devuelve true si se realizó una inserción y false si se
        /// realizó una actualización.
        /// </summary>
        public bool InsertarOActualizar(Sala sala)
        {
            Sala existente = BuscarPorNombre(sala.Nombre);

            if (existente == null)
            {
                Insertar(sala);
                return true;
            }

            existente.Filas = sala.Filas;
            existente.Columnas = sala.Columnas;

            return false;
        }

        /// <summary>
        /// Elimina de la lista la sala cuyo nombre coincide con el
        /// parámetro recibido. Contempla el caso de eliminar el primer
        /// nodo y el caso de eliminar un nodo intermedio o final, para lo
        /// cual conserva la referencia al nodo anterior.
        /// Devuelve true si se eliminó un elemento.
        /// </summary>
        public bool EliminarPorNombre(string nombre)
        {
            if (Primero == null)
            {
                return false;
            }

            NodoSala actual = Primero;
            NodoSala anterior = null;

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
        /// Devuelve la sala ubicada en la posición indicada, comenzando
        /// en cero. Recorre la lista hasta alcanzar el índice solicitado.
        /// Devuelve null si el índice está fuera de rango.
        /// </summary>
        public Sala ObtenerPorIndice(int indice)
        {
            if (indice < 0 || indice >= Cantidad)
            {
                return null;
            }

            NodoSala actual = Primero;
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
