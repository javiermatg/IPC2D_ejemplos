using ChapinCine.Modelo;

namespace ChapinCine.TDA
{
    /// <summary>
    /// Nodo de la lista de celdas afectadas por una reservación.
    /// Guarda la posición de la celda y el asiento que la ocupaba antes
    /// de la operación, dato necesario para poder restaurarla.
    /// </summary>
    public class NodoCelda
    {
        public int Fila { get; set; }
        public int Columna { get; set; }

        /// <summary>Asiento que ocupaba la posición antes de la reservación.</summary>
        public Asiento AsientoOriginal { get; set; }

        public NodoCelda Siguiente { get; set; }

        public NodoCelda(int fila, int columna, Asiento asientoOriginal)
        {
            Fila = fila;
            Columna = columna;
            AsientoOriginal = asientoOriginal;
            Siguiente = null;
        }
    }
}
