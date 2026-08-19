using ChapinCine.Modelo;

namespace ChapinCine.TDA
{
    /// <summary>
    /// Nodo de la pila de reservaciones.
    /// </summary>
    public class NodoReservacion
    {
        public Reservacion Dato { get; set; }
        public NodoReservacion Siguiente { get; set; }

        public NodoReservacion(Reservacion dato)
        {
            Dato = dato;
            Siguiente = null;
        }
    }

    /// <summary>
    /// Pila de reservaciones con política LIFO.
    /// Se utiliza para deshacer operaciones: la última reservación
    /// realizada es la primera que puede revertirse, y la estructura hace
    /// cumplir ese orden sin necesidad de búsquedas.
    /// </summary>
    public class PilaReservaciones
    {
        private NodoReservacion cima;

        public int Cantidad { get; private set; }

        public PilaReservaciones()
        {
            cima = null;
            Cantidad = 0;
        }

        public bool EstaVacia()
        {
            return cima == null;
        }

        /// <summary>Coloca una reservación en la cima de la pila.</summary>
        public void Apilar(Reservacion reservacion)
        {
            NodoReservacion nuevo = new NodoReservacion(reservacion);

            nuevo.Siguiente = cima;
            cima = nuevo;

            Cantidad = Cantidad + 1;
        }

        /// <summary>
        /// Extrae y devuelve la última reservación realizada.
        /// Devuelve null si la pila está vacía.
        /// </summary>
        public Reservacion Desapilar()
        {
            if (cima == null)
            {
                return null;
            }

            Reservacion dato = cima.Dato;
            cima = cima.Siguiente;
            Cantidad = Cantidad - 1;

            return dato;
        }

        /// <summary>
        /// Devuelve el nodo de la cima para recorrer las reservaciones
        /// sin extraerlas.
        /// </summary>
        public NodoReservacion VerCima()
        {
            return cima;
        }
    }
}
