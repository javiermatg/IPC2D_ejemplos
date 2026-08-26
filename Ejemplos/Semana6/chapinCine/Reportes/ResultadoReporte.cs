namespace ChapinCine.Reportes
{
    /// <summary>
    /// Reporta el resultado de generar un reporte gráfico.
    ///
    /// La generación se divide en dos etapas que pueden fallar de forma
    /// independiente: escribir el archivo .dot y ejecutar Graphviz para
    /// convertirlo en imagen. Este objeto informa hasta dónde se llegó,
    /// de modo que la capa de presentación pueda dar un mensaje preciso.
    /// </summary>
    public class ResultadoReporte
    {
        /// <summary>True si se escribió el archivo .dot.</summary>
        public bool DotGenerado { get; set; }

        /// <summary>True si Graphviz produjo la imagen.</summary>
        public bool ImagenGenerada { get; set; }

        /// <summary>Ruta absoluta del archivo .dot escrito.</summary>
        public string RutaDot { get; set; }

        /// <summary>Ruta absoluta de la imagen, si se generó.</summary>
        public string RutaImagen { get; set; }

        /// <summary>Detalle del problema cuando alguna etapa falló.</summary>
        public string MensajeError { get; set; }

        public ResultadoReporte()
        {
            DotGenerado = false;
            ImagenGenerada = false;
            RutaDot = "";
            RutaImagen = "";
            MensajeError = "";
        }
    }
}
