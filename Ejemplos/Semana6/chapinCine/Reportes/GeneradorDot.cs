using System.Text;
using ChapinCine.Modelo;
using ChapinCine.TDA;

namespace ChapinCine.Reportes
{
    /// <summary>
    /// Construye el código fuente en lenguaje DOT que Graphviz utiliza para
    /// dibujar los reportes.
    ///
    /// Esta clase pertenece a la capa de presentación: convierte las
    /// estructuras del sistema en texto. No modifica la malla ni el modelo,
    /// únicamente los recorre, y lo hace con los mismos métodos que emplea
    /// el dibujo en consola.
    ///
    /// TÉCNICA UTILIZADA PARA LA MALLA
    /// Graphviz es un motor de grafos: si la cuadrícula se armara con un
    /// nodo por celda, el algoritmo de posicionamiento decidiría dónde
    /// ubicar cada uno y el resultado no quedaría alineado.
    ///
    /// Para evitarlo, toda la malla se define como UN SOLO nodo cuya
    /// etiqueta es una tabla en sintaxis HTML-like. Graphviz no calcula
    /// posiciones: se limita a dibujar la tabla, con lo que la cuadrícula
    /// queda exacta.
    ///
    /// Las etiquetas HTML-like se escriben entre los signos menor que y
    /// mayor que, no entre comillas: label=&lt;...&gt;
    /// </summary>
    public class GeneradorDot
    {
        /// <summary>
        /// Devuelve el color de fondo que corresponde a cada tipo de asiento.
        ///
        /// La correspondencia entre tipo y color vive aquí, en la capa de
        /// presentación, y no en las clases Asiento. El color es una
        /// decisión de cómo se muestra el dato, no una propiedad del
        /// dominio: la misma malla podría dibujarse con otra paleta sin
        /// tocar el modelo.
        /// </summary>
        private string ColorDe(Asiento asiento)
        {
            string tipo = asiento.ObtenerTipo();

            if (tipo == "Pasillo") return "black";
            if (tipo == "Estandar") return "white";
            if (tipo == "Ocupado") return "gray70";
            if (tipo == "Accesible") return "green3";
            if (tipo == "VIP") return "red2";

            return "magenta";   // tipo no contemplado: se hace evidente
        }

        /// <summary>Color con el que se resaltan las celdas destacadas.</summary>
        private string ColorResaltado()
        {
            return "khaki1";
        }

        /// <summary>
        /// Indica si la posición indicada pertenece al conjunto de celdas a
        /// resaltar. Recibe null cuando no hay nada que resaltar.
        /// </summary>
        private bool EstaResaltada(ListaCeldas celdas, int fila, int columna)
        {
            if (celdas == null)
            {
                return false;
            }

            NodoCelda actual = celdas.Primero;

            while (actual != null)
            {
                if (actual.Fila == fila && actual.Columna == columna)
                {
                    return true;
                }

                actual = actual.Siguiente;
            }

            return false;
        }

        /// <summary>
        /// Construye la tabla HTML-like de la malla, con una fila de
        /// encabezado para los números de columna y una celda de encabezado
        /// por fila.
        ///
        /// Cada celda usa FIXEDSIZE con WIDTH y HEIGHT para que todas midan
        /// lo mismo; sin ese atributo, Graphviz ajusta el tamaño al
        /// contenido y la cuadrícula se deforma.
        /// </summary>
        private string TablaMalla(MallaAsientos malla, ListaCeldas resaltadas)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<TABLE BORDER=\"0\" CELLBORDER=\"1\" CELLSPACING=\"0\" CELLPADDING=\"0\">");

            // Encabezado con los números de columna.
            sb.Append("<TR>");
            sb.Append("<TD BORDER=\"0\" WIDTH=\"22\" HEIGHT=\"18\" FIXEDSIZE=\"TRUE\"></TD>");

            int c = 1;
            while (c <= malla.TotalColumnas)
            {
                sb.Append("<TD BORDER=\"0\" WIDTH=\"18\" HEIGHT=\"18\" FIXEDSIZE=\"TRUE\">");
                sb.Append("<FONT POINT-SIZE=\"9\">").Append(c).Append("</FONT></TD>");
                c = c + 1;
            }

            sb.Append("</TR>");

            int f = 1;
            while (f <= malla.TotalFilas)
            {
                sb.Append("<TR>");
                sb.Append("<TD BORDER=\"0\" WIDTH=\"22\" HEIGHT=\"18\" FIXEDSIZE=\"TRUE\" ALIGN=\"RIGHT\">");
                sb.Append("<FONT POINT-SIZE=\"9\">").Append(f).Append(" </FONT></TD>");

                // Recorrido horizontal siguiendo los enlaces de la malla,
                // el mismo que utiliza el dibujo en consola.
                NodoAsiento actual = malla.ObtenerPrimeroDeFila(f);

                while (actual != null)
                {
                    string color;

                    if (EstaResaltada(resaltadas, actual.Fila, actual.Columna))
                    {
                        color = ColorResaltado();
                    }
                    else
                    {
                        color = ColorDe(actual.Dato);
                    }

                    sb.Append("<TD WIDTH=\"18\" HEIGHT=\"18\" FIXEDSIZE=\"TRUE\" BGCOLOR=\"");
                    sb.Append(color).Append("\"></TD>");

                    actual = actual.Derecha;
                }

                sb.Append("</TR>");
                f = f + 1;
            }

            sb.Append("</TABLE>");
            return sb.ToString();
        }

        /// <summary>
        /// Construye la tabla de referencia de colores.
        /// </summary>
        private string TablaLeyenda(bool incluirResaltado)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<TABLE BORDER=\"0\" CELLBORDER=\"0\" CELLSPACING=\"2\" CELLPADDING=\"1\">");

            sb.Append(FilaLeyenda("black", "Pasillo o estructura"));
            sb.Append(FilaLeyenda("white", "Asiento disponible"));
            sb.Append(FilaLeyenda("gray70", "Asiento ocupado"));
            sb.Append(FilaLeyenda("green3", "Espacio accesible"));
            sb.Append(FilaLeyenda("red2", "Asiento VIP (con recargo)"));

            if (incluirResaltado)
            {
                sb.Append(FilaLeyenda(ColorResaltado(), "Asientos de esta reservación"));
            }

            sb.Append("</TABLE>");
            return sb.ToString();
        }

        private string FilaLeyenda(string color, string texto)
        {
            return "<TR><TD BORDER=\"1\" WIDTH=\"16\" HEIGHT=\"12\" FIXEDSIZE=\"TRUE\" BGCOLOR=\""
                   + color + "\"></TD>"
                   + "<TD ALIGN=\"LEFT\"><FONT POINT-SIZE=\"10\">" + Escapar(texto)
                   + "</FONT></TD></TR>";
        }

        /// <summary>
        /// Sustituye los caracteres que tienen significado especial dentro
        /// de una etiqueta HTML-like. Sin esta sustitución, un nombre que
        /// contenga el símbolo &amp; o los signos de comparación haría que
        /// Graphviz rechace el archivo.
        /// </summary>
        private string Escapar(string texto)
        {
            if (texto == null)
            {
                return "";
            }

            string r = texto.Replace("&", "&amp;");
            r = r.Replace("<", "&lt;");
            r = r.Replace(">", "&gt;");
            r = r.Replace("\"", "&quot;");
            return r;
        }

        /// <summary>
        /// Arma el documento DOT completo: un único nodo con una tabla
        /// contenedora que agrupa el título, la malla, la leyenda y las
        /// líneas de pie.
        ///
        /// Al quedar todo dentro de un solo nodo, Graphviz no tiene ninguna
        /// decisión de posicionamiento que tomar y el resultado es idéntico
        /// en cualquier equipo.
        /// </summary>
        private string ArmarDocumento(string titulo, MallaAsientos malla,
                                      ListaCeldas resaltadas, string[] pie)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<TABLE BORDER=\"0\" CELLBORDER=\"0\" CELLSPACING=\"10\">");

            sb.Append("<TR><TD><FONT POINT-SIZE=\"16\"><B>");
            sb.Append(Escapar(titulo));
            sb.Append("</B></FONT></TD></TR>");

            sb.Append("<TR><TD>").Append(TablaMalla(malla, resaltadas)).Append("</TD></TR>");
            sb.Append("<TR><TD>").Append(TablaLeyenda(resaltadas != null)).Append("</TD></TR>");

            if (pie != null)
            {
                int i = 0;
                while (i < pie.Length)
                {
                    sb.Append("<TR><TD><FONT POINT-SIZE=\"11\">");
                    sb.Append(Escapar(pie[i]));
                    sb.Append("</FONT></TD></TR>");
                    i = i + 1;
                }
            }

            sb.Append("</TABLE>");

            StringBuilder doc = new StringBuilder();
            doc.AppendLine("digraph Reporte {");
            doc.AppendLine("    graph [bgcolor=\"white\"];");
            doc.AppendLine("    node [shape=plaintext, fontname=\"Arial\"];");
            doc.Append("    reporte [label=<").Append(sb).AppendLine(">];");
            doc.AppendLine("}");

            return doc.ToString();
        }

        /// <summary>
        /// Genera el DOT de una sala sin resaltar ninguna celda.
        /// </summary>
        public string GenerarSala(Sala sala)
        {
            if (sala == null || !sala.TieneMalla())
            {
                return null;
            }

            string[] pie = new string[1];
            pie[0] = "Dimensiones: " + sala.Filas + " filas x " + sala.Columnas + " columnas";

            return ArmarDocumento(sala.Nombre, sala.Asientos, null, pie);
        }

        /// <summary>
        /// Genera el DOT de una reservación, resaltando las celdas que la
        /// componen y agregando al pie los datos de la operación.
        ///
        /// Las celdas resaltadas se toman de la propia reservación, que ya
        /// las conserva para poder deshacerse.
        /// </summary>
        public string GenerarReservacion(Reservacion reservacion)
        {
            if (reservacion == null || reservacion.Sala == null
                || !reservacion.Sala.TieneMalla())
            {
                return null;
            }

            string[] pie = new string[3];

            pie[0] = "Cliente: " + reservacion.Cliente.Nombre
                     + " (" + reservacion.Cliente.ObtenerTipo() + ")";

            pie[1] = "Asientos reservados: fila " + reservacion.Fila
                     + ", columnas " + reservacion.ColumnaInicial
                     + " a " + (reservacion.ColumnaInicial + reservacion.Cantidad - 1);

            ClientePremium premium = reservacion.Cliente as ClientePremium;

            if (premium != null)
            {
                pie[2] = "Presupuesto inicial Q" + premium.PresupuestoInicial
                         + " - Presupuesto final Q" + premium.Presupuesto
                         + " (recargo de esta reservación: Q" + reservacion.MontoTotal + ")";
            }
            else
            {
                pie[2] = "Sin recargo: el cliente no maneja presupuesto.";
            }

            return ArmarDocumento("Reservación en " + reservacion.Sala.Nombre,
                                  reservacion.Sala.Asientos,
                                  reservacion.Celdas, pie);
        }

        /// <summary>
        /// Genera el DOT que representa la estructura interna de la matriz
        /// ortogonal en un fragmento de la malla: un nodo por celda y una
        /// arista por cada enlace entre nodos vecinos.
        ///
        /// A diferencia de los reportes anteriores, aquí sí se dibuja un
        /// grafo real. Para que la cuadrícula quede alineada se fija la
        /// posición de cada nodo con el atributo pos, lo que obliga a
        /// procesar el archivo con el motor neato y la opción -n, que
        /// respeta las posiciones indicadas en lugar de calcularlas.
        ///
        /// El fragmento se limita a unas pocas filas y columnas porque una
        /// malla completa produciría una imagen demasiado densa.
        /// </summary>
        public string GenerarEstructura(Sala sala, int filaIni, int colIni,
                                        int filaFin, int colFin)
        {
            if (sala == null || !sala.TieneMalla())
            {
                return null;
            }

            MallaAsientos malla = sala.Asientos;

            if (filaIni < 1 || colIni < 1
                || filaFin > malla.TotalFilas || colFin > malla.TotalColumnas
                || filaIni > filaFin || colIni > colFin)
            {
                return null;
            }

            // Separación en puntos entre nodos vecinos.
            int dx = 85;
            int dy = 62;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("graph Estructura {");
            sb.AppendLine("    graph [bgcolor=white, splines=false];");
            sb.AppendLine("    node [shape=box, style=filled, fontname=\"Arial\", fontsize=10,");
            sb.AppendLine("          width=0.62, height=0.42, fixedsize=true];");
            sb.AppendLine("    edge [color=gray45, penwidth=1.2];");
            sb.AppendLine();

            // Cabeceras de columna, ubicadas sobre la primera fila.
            int c = colIni;
            while (c <= colFin)
            {
                sb.Append("    cc_").Append(c).Append(" [label=\"col ").Append(c);
                sb.Append("\", pos=\"").Append((c - colIni) * dx).Append(",").Append(dy);
                sb.AppendLine("!\", fillcolor=lightblue];");
                c = c + 1;
            }

            // Cabeceras de fila y nodos.
            int f = filaIni;
            while (f <= filaFin)
            {
                sb.Append("    cf_").Append(f).Append(" [label=\"fila ").Append(f);
                sb.Append("\", pos=\"").Append(-dx).Append(",").Append(-(f - filaIni) * dy);
                sb.AppendLine("!\", fillcolor=lightblue];");

                c = colIni;
                while (c <= colFin)
                {
                    NodoAsiento nodo = malla.ObtenerNodo(f, c);

                    string relleno = ColorDe(nodo.Dato);
                    string letra = "black";

                    if (relleno == "black" || relleno == "red2")
                    {
                        letra = "white";
                    }

                    sb.Append("    n_").Append(f).Append("_").Append(c);
                    sb.Append(" [label=\"").Append(f).Append(",").Append(c);
                    sb.Append("\", pos=\"").Append((c - colIni) * dx).Append(",");
                    sb.Append(-(f - filaIni) * dy).Append("!\", fillcolor=\"").Append(relleno);
                    sb.Append("\", fontcolor=\"").Append(letra).AppendLine("\"];");

                    c = c + 1;
                }

                f = f + 1;
            }

            sb.AppendLine();

            // Aristas horizontales: representan los enlaces Izquierda/Derecha.
            f = filaIni;
            while (f <= filaFin)
            {
                sb.Append("    cf_").Append(f).Append(" -- n_").Append(f).Append("_");
                sb.Append(colIni).AppendLine(" [color=blue, style=dashed];");

                c = colIni;
                while (c < colFin)
                {
                    sb.Append("    n_").Append(f).Append("_").Append(c);
                    sb.Append(" -- n_").Append(f).Append("_").Append(c + 1).AppendLine(";");
                    c = c + 1;
                }

                f = f + 1;
            }

            // Aristas verticales: representan los enlaces Arriba/Abajo.
            c = colIni;
            while (c <= colFin)
            {
                sb.Append("    cc_").Append(c).Append(" -- n_").Append(filaIni);
                sb.Append("_").Append(c).AppendLine(" [color=blue, style=dashed];");

                f = filaIni;
                while (f < filaFin)
                {
                    sb.Append("    n_").Append(f).Append("_").Append(c);
                    sb.Append(" -- n_").Append(f + 1).Append("_").Append(c).AppendLine(";");
                    f = f + 1;
                }

                c = c + 1;
            }

            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}
