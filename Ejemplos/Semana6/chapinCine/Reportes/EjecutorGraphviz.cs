using System;
using System.Diagnostics;
using System.IO;

namespace ChapinCine.Reportes
{
    /// <summary>
    /// Escribe el archivo .dot en disco e invoca a Graphviz para
    /// convertirlo en imagen.
    ///
    /// Graphviz es un programa externo: no forma parte de .NET y debe estar
    /// instalado en el equipo. Por eso todas las operaciones que dependen
    /// de él están protegidas: si el programa no se encuentra, el sistema
    /// informa el problema y conserva el archivo .dot generado, en lugar de
    /// interrumpir la ejecución.
    /// </summary>
    public class EjecutorGraphviz
    {
        /// <summary>
        /// Nombre o ruta del ejecutable de Graphviz.
        ///
        /// El valor por defecto es el nombre a secas, lo que hace que el
        /// sistema operativo lo busque en el PATH. Se deja como propiedad
        /// modificable para poder indicar una ruta completa en equipos
        /// donde Graphviz esté instalado fuera del PATH; fijar la ruta
        /// dentro del código haría que el programa solo funcione en la
        /// máquina donde se desarrolló.
        /// </summary>
        public string RutaDot { get; set; }

        /// <summary>
        /// Carpeta donde se escriben los archivos generados.
        /// </summary>
        public string CarpetaSalida { get; set; }

        public EjecutorGraphviz()
        {
            RutaDot = "dot";
            CarpetaSalida = "Reportes";
        }

        /// <summary>
        /// Verifica si Graphviz puede invocarse, ejecutando la opción que
        /// muestra su versión. Devuelve false si el ejecutable no existe.
        /// </summary>
        public bool EstaDisponible()
        {
            try
            {
                ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = RutaDot;
                info.Arguments = "-V";
                info.UseShellExecute = false;
                info.RedirectStandardError = true;
                info.RedirectStandardOutput = true;
                info.CreateNoWindow = true;

                Process proceso = Process.Start(info);
                proceso.WaitForExit(5000);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Escribe el contenido DOT en un archivo y ejecuta Graphviz para
        /// producir el PNG correspondiente.
        ///
        /// El parámetro motor indica qué programa de Graphviz utilizar.
        /// Los reportes de la malla usan "dot"; el reporte de la estructura
        /// usa "neato" con la opción -n, porque define posiciones fijas
        /// para cada nodo y necesita un motor que las respete.
        /// </summary>
        public ResultadoReporte Generar(string contenidoDot, string nombreBase, string motor)
        {
            ResultadoReporte resultado = new ResultadoReporte();

            if (contenidoDot == null)
            {
                resultado.MensajeError = "No se pudo construir el contenido del reporte.";
                return resultado;
            }

            // --- Etapa 1: escribir el archivo .dot ---
            try
            {
                if (!Directory.Exists(CarpetaSalida))
                {
                    Directory.CreateDirectory(CarpetaSalida);
                }

                resultado.RutaDot = Path.GetFullPath(
                    Path.Combine(CarpetaSalida, nombreBase + ".dot"));

                File.WriteAllText(resultado.RutaDot, contenidoDot);
                resultado.DotGenerado = true;
            }
            catch (Exception ex)
            {
                resultado.MensajeError = "No se pudo escribir el archivo .dot: " + ex.Message;
                return resultado;
            }

            // --- Etapa 2: ejecutar Graphviz ---
            resultado.RutaImagen = Path.GetFullPath(
                Path.Combine(CarpetaSalida, nombreBase + ".png"));

            string ejecutable = RutaDot;
            string argumentos;

            if (motor == "neato")
            {
                // -n indica a neato que use las posiciones del atributo pos
                // en lugar de calcular una distribución propia.
                ejecutable = "neato";
                argumentos = "-n -Tpng \"" + resultado.RutaDot
                             + "\" -o \"" + resultado.RutaImagen + "\"";
            }
            else
            {
                argumentos = "-Tpng \"" + resultado.RutaDot
                             + "\" -o \"" + resultado.RutaImagen + "\"";
            }

            try
            {
                ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = ejecutable;
                info.Arguments = argumentos;

                // UseShellExecute en false permite capturar la salida de
                // error de Graphviz, que es donde informa los problemas de
                // sintaxis del archivo .dot.
                info.UseShellExecute = false;
                info.RedirectStandardError = true;
                info.CreateNoWindow = true;

                Process proceso = Process.Start(info);
                string error = proceso.StandardError.ReadToEnd();
                proceso.WaitForExit();

                if (proceso.ExitCode == 0 && File.Exists(resultado.RutaImagen))
                {
                    resultado.ImagenGenerada = true;
                }
                else
                {
                    resultado.MensajeError = "Graphviz no pudo generar la imagen. " + error;
                }
            }
            catch (Exception)
            {
                // Se llega aquí normalmente cuando el ejecutable no existe.
                resultado.MensajeError =
                    "No se encontró Graphviz. Verifique que esté instalado y "
                    + "que la carpeta bin esté agregada al PATH del sistema. "
                    + "El archivo .dot sí se generó y puede procesarse manualmente.";
            }

            return resultado;
        }

        /// <summary>
        /// Abre la imagen con el visor predeterminado del sistema.
        ///
        /// A diferencia de la ejecución de Graphviz, aquí UseShellExecute
        /// debe estar en true: es lo que permite que el sistema operativo
        /// decida con qué programa abrir el archivo.
        /// </summary>
        public bool AbrirImagen(string ruta)
        {
            try
            {
                if (!File.Exists(ruta))
                {
                    return false;
                }

                ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = ruta;
                info.UseShellExecute = true;

                Process.Start(info);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
