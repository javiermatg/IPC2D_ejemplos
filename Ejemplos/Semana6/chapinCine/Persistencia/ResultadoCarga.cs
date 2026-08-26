namespace ChapinCine.Persistencia
{
    /// <summary>
    /// Objeto de transferencia que reporta el resultado de una operación
    /// de carga de archivo. Permite que la capa de persistencia devuelva
    /// el detalle de lo ocurrido sin depender de una interfaz de salida
    /// concreta.
    /// </summary>
    public class ResultadoCarga
    {
        /// <summary>
        /// Indica si el archivo pudo procesarse. Vale false cuando ocurrió
        /// un error que impidió completar la lectura.
        /// </summary>
        public bool Exito { get; set; }

        /// <summary>Descripción del error cuando Exito es false.</summary>
        public string MensajeError { get; set; }

        /// <summary>Cantidad de salas insertadas por primera vez.</summary>
        public int SalasNuevas { get; set; }

        /// <summary>Cantidad de salas que ya existían y fueron actualizadas.</summary>
        public int SalasActualizadas { get; set; }

        /// <summary>Cantidad de clientes insertados por primera vez.</summary>
        public int ClientesNuevos { get; set; }

        /// <summary>Cantidad de clientes que ya existían y fueron reemplazados.</summary>
        public int ClientesActualizados { get; set; }

        /// <summary>
        /// Texto acumulado con los registros que se omitieron por tener
        /// datos inválidos. No impiden que el resto del archivo se procese.
        /// </summary>
        public string Advertencias { get; set; }

        public ResultadoCarga()
        {
            Exito = true;
            MensajeError = "";
            Advertencias = "";
            SalasNuevas = 0;
            SalasActualizadas = 0;
            ClientesNuevos = 0;
            ClientesActualizados = 0;
        }

        /// <summary>
        /// Agrega una advertencia al reporte, separándola de las
        /// anteriores con un salto de línea.
        /// </summary>
        public void AgregarAdvertencia(string texto)
        {
            if (Advertencias.Length > 0)
            {
                Advertencias = Advertencias + "\n";
            }

            Advertencias = Advertencias + "  - " + texto;
        }

        /// <summary>Indica si se acumuló al menos una advertencia.</summary>
        public bool TieneAdvertencias()
        {
            return Advertencias.Length > 0;
        }
    }
}
