using ChapinCine.TDA;

namespace ChapinCine.Modelo
{
    /// <summary>
    /// Entidad de dominio que representa una sala de cine.
    /// Almacena el nombre identificador y las dimensiones de la sala.
    /// </summary>
    public class Sala
    {
        /// <summary>Nombre que identifica la sala de forma única.</summary>
        public string Nombre { get; set; }

        /// <summary>Cantidad de filas de la sala.</summary>
        public int Filas { get; set; }

        /// <summary>Cantidad de columnas de la sala.</summary>
        public int Columnas { get; set; }

        /// <summary>
        /// Malla de asientos de la sala. Vale null cuando la sala se creó
        /// sin una malla asociada.
        /// </summary>
        public MallaAsientos Asientos { get; set; }

        public Sala(string nombre, int filas, int columnas)
        {
            Nombre = nombre;
            Filas = filas;
            Columnas = columnas;
            Asientos = null;
        }

        /// <summary>Indica si la sala tiene una malla de asientos cargada.</summary>
        public bool TieneMalla()
        {
            return Asientos != null;
        }

        /// <summary>
        /// Calcula el total de celdas de la sala a partir de sus dimensiones.
        /// Se calcula en lugar de almacenarse para evitar inconsistencias
        /// cuando se modifican Filas o Columnas.
        /// </summary>
        public int TotalCeldas()
        {
            return Filas * Columnas;
        }
    }
}
