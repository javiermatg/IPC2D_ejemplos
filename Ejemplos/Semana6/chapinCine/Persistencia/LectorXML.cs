using System;
using System.Xml;
using ChapinCine.Modelo;
using ChapinCine.TDA;

namespace ChapinCine.Persistencia
{
    /// <summary>
    /// Capa de persistencia. Lee el archivo XML de configuración y
    /// traslada su contenido a las estructuras propias del sistema.
    /// Los objetos de la librería de XML se utilizan únicamente dentro de
    /// esta clase; hacia el resto del sistema solo se exponen los TDA.
    /// </summary>
    public class LectorXML
    {
        /// <summary>
        /// Procesa un archivo de configuración y vuelca su contenido en
        /// las listas recibidas por parámetro. Las estructuras no se crean
        /// aquí, se reciben ya instanciadas, lo que permite acumular el
        /// contenido de varios archivos sobre las mismas listas.
        /// </summary>
        public ResultadoCarga CargarConfiguracion(string ruta,
                                                  ListaSalas listaSalas,
                                                  ListaClientes listaClientes)
        {
            ResultadoCarga resultado = new ResultadoCarga();

            if (!System.IO.File.Exists(ruta))
            {
                resultado.Exito = false;
                resultado.MensajeError = "No se encontró el archivo: " + ruta;
                return resultado;
            }

            try
            {
                XmlDocument documento = new XmlDocument();

                // Conserva los espacios en blanco del contenido, necesarios
                // porque el espacio es un carácter significativo dentro de
                // las filas de la malla.
                documento.PreserveWhitespace = true;

                documento.Load(ruta);

                CargarSalas(documento, listaSalas, resultado);
                CargarClientes(documento, listaClientes, resultado);
            }
            catch (XmlException ex)
            {
                // Se produce cuando el documento no está bien formado.
                resultado.Exito = false;
                resultado.MensajeError = "El XML está mal formado: " + ex.Message;
            }
            catch (Exception ex)
            {
                resultado.Exito = false;
                resultado.MensajeError = "Error inesperado al leer: " + ex.Message;
            }

            return resultado;
        }

        /// <summary>
        /// Recorre las etiquetas sala del documento, construye los objetos
        /// Sala correspondientes y los inserta en la lista.
        /// Los registros con datos inválidos se omiten y se reportan como
        /// advertencia sin interrumpir el proceso.
        /// </summary>
        private void CargarSalas(XmlDocument documento,
                                 ListaSalas listaSalas,
                                 ResultadoCarga resultado)
        {
            XmlNodeList nodosSala = documento.SelectNodes("/configuracion/listaSalas/sala");

            if (nodosSala == null)
            {
                return;
            }

            foreach (XmlNode nodoSala in nodosSala)
            {
                // La etiqueta nombre contiene los atributos con las
                // dimensiones de la sala.
                XmlNode nodoNombre = nodoSala.SelectSingleNode("nombre");

                if (nodoNombre == null)
                {
                    resultado.AgregarAdvertencia("Se encontró una <sala> sin etiqueta <nombre>. Se omitió.");
                    continue;
                }

                string nombre = nodoNombre.InnerText.Trim();

                if (nombre.Length == 0)
                {
                    resultado.AgregarAdvertencia("Se encontró una sala con nombre vacío. Se omitió.");
                    continue;
                }

                int filas = LeerAtributoEntero(nodoNombre, "filas", -1);
                int columnas = LeerAtributoEntero(nodoNombre, "columnas", -1);

                if (filas <= 0 || columnas <= 0)
                {
                    resultado.AgregarAdvertencia("La sala '" + nombre +
                        "' tiene filas/columnas inválidas. Se omitió.");
                    continue;
                }

                Sala sala = new Sala(nombre, filas, columnas);

                // La malla se construye primero a partir de los caracteres
                // de las etiquetas fila, y solo después se aplican las
                // etiquetas asientoVIP sobre las celdas ya creadas.
                sala.Asientos = ConstruirMalla(nodoSala, nombre, filas, columnas, resultado);

                if (sala.Asientos != null)
                {
                    AplicarAsientosVIP(nodoSala, nombre, sala.Asientos, resultado);
                }

                bool esNueva = listaSalas.InsertarOActualizar(sala);

                if (esNueva)
                {
                    resultado.SalasNuevas = resultado.SalasNuevas + 1;
                }
                else
                {
                    resultado.SalasActualizadas = resultado.SalasActualizadas + 1;
                }
            }
        }

        /// <summary>
        /// Recorre las etiquetas cliente del documento y construye la
        /// instancia concreta que corresponde según el atributo tipo.
        /// Es el único punto del sistema donde se evalúa el tipo para
        /// crear el objeto; el resto de las capas operan por polimorfismo.
        /// </summary>
        private void CargarClientes(XmlDocument documento,
                                    ListaClientes listaClientes,
                                    ResultadoCarga resultado)
        {
            XmlNodeList nodosCliente = documento.SelectNodes("/configuracion/clientes/cliente");

            if (nodosCliente == null)
            {
                return;
            }

            foreach (XmlNode nodoCliente in nodosCliente)
            {
                XmlNode nodoNombre = nodoCliente.SelectSingleNode("nombre");

                if (nodoNombre == null)
                {
                    resultado.AgregarAdvertencia("Se encontró un <cliente> sin <nombre>. Se omitió.");
                    continue;
                }

                string nombre = nodoNombre.InnerText.Trim();
                string tipo = LeerAtributoTexto(nodoNombre, "tipo", "");

                if (nombre.Length == 0)
                {
                    resultado.AgregarAdvertencia("Se encontró un cliente sin nombre. Se omitió.");
                    continue;
                }

                Cliente cliente = null;

                if (string.Equals(tipo, "ClientePremium", StringComparison.OrdinalIgnoreCase))
                {
                    // El atributo presupuesto solo está presente cuando el
                    // tipo declarado es ClientePremium.
                    int presupuesto = LeerAtributoEntero(nodoNombre, "presupuesto", -1);

                    if (presupuesto < 0)
                    {
                        resultado.AgregarAdvertencia("El cliente '" + nombre +
                            "' es Premium pero no tiene presupuesto válido. Se omitió.");
                        continue;
                    }

                    cliente = new ClientePremium(nombre, presupuesto);
                }
                else if (string.Equals(tipo, "ClienteEstandar", StringComparison.OrdinalIgnoreCase))
                {
                    cliente = new ClienteEstandar(nombre);
                }
                else
                {
                    resultado.AgregarAdvertencia("El cliente '" + nombre +
                        "' tiene un tipo desconocido ('" + tipo + "'). Se omitió.");
                    continue;
                }

                bool esNuevo = listaClientes.InsertarOActualizar(cliente);

                if (esNuevo)
                {
                    resultado.ClientesNuevos = resultado.ClientesNuevos + 1;
                }
                else
                {
                    resultado.ClientesActualizados = resultado.ClientesActualizados + 1;
                }
            }
        }

        /// <summary>
        /// Construye la malla de asientos de una sala a partir de sus
        /// etiquetas fila.
        ///
        /// Crea la malla vacía y recorre las filas en orden ascendente,
        /// insertando de izquierda a derecha. Ese orden es el que exige
        /// MallaAsientos.Insertar para poder enlazar cada nodo con su
        /// vecino izquierdo y su vecino superior.
        ///
        /// Devuelve null si la sala tiene filas inválidas o faltantes.
        /// </summary>
        private MallaAsientos ConstruirMalla(XmlNode nodoSala, string nombreSala,
                                             int filas, int columnas,
                                             ResultadoCarga resultado)
        {
            MallaAsientos malla = new MallaAsientos(filas, columnas);

            int numeroFila = 1;

            while (numeroFila <= filas)
            {
                // Se busca la etiqueta fila cuyo atributo numero coincide,
                // en lugar de asumir que vienen en orden dentro del archivo.
                XmlNode nodoFila = BuscarFilaPorNumero(nodoSala, numeroFila);

                if (nodoFila == null)
                {
                    resultado.AgregarAdvertencia("La sala '" + nombreSala +
                        "' no tiene la fila " + numeroFila + ". No se construyó su malla.");
                    return null;
                }

                string contenido = ExtraerEntreComillas(nodoFila.InnerText);

                if (contenido == null)
                {
                    resultado.AgregarAdvertencia("La fila " + numeroFila + " de la sala '" +
                        nombreSala + "' no está delimitada por comillas. No se construyó su malla.");
                    return null;
                }

                if (contenido.Length != columnas)
                {
                    resultado.AgregarAdvertencia("La fila " + numeroFila + " de la sala '" +
                        nombreSala + "' tiene " + contenido.Length + " caracteres y se esperaban " +
                        columnas + ". No se construyó su malla.");
                    return null;
                }

                int numeroColumna = 1;

                while (numeroColumna <= columnas)
                {
                    // Los índices del texto inician en 0 y los de la malla
                    // en 1, de ahí la resta.
                    char simbolo = contenido[numeroColumna - 1];

                    Asiento asiento = CrearAsiento(simbolo);

                    if (asiento == null)
                    {
                        resultado.AgregarAdvertencia("Carácter no reconocido ('" + simbolo +
                            "') en la fila " + numeroFila + ", columna " + numeroColumna +
                            " de la sala '" + nombreSala + "'. Se tomó como pasillo.");

                        asiento = new AsientoPasillo();
                    }

                    malla.Insertar(asiento, numeroFila, numeroColumna);

                    numeroColumna = numeroColumna + 1;
                }

                numeroFila = numeroFila + 1;
            }

            return malla;
        }

        /// <summary>
        /// Aplica las etiquetas asientoVIP sobre una malla ya construida,
        /// sustituyendo el contenido de las posiciones indicadas.
        ///
        /// Se valida que la posición esté dentro del rango y que la celda
        /// destino sea un asiento estándar; las etiquetas que no cumplan
        /// se omiten y se reportan como advertencia.
        /// </summary>
        private void AplicarAsientosVIP(XmlNode nodoSala, string nombreSala,
                                        MallaAsientos malla, ResultadoCarga resultado)
        {
            XmlNodeList nodosVIP = nodoSala.SelectNodes("asientoVIP");

            if (nodosVIP == null)
            {
                return;
            }

            foreach (XmlNode nodoVIP in nodosVIP)
            {
                int fila = LeerAtributoEntero(nodoVIP, "fila", -1);
                int columna = LeerAtributoEntero(nodoVIP, "columna", -1);

                if (fila < 1 || fila > malla.TotalFilas ||
                    columna < 1 || columna > malla.TotalColumnas)
                {
                    resultado.AgregarAdvertencia("Un asientoVIP de la sala '" + nombreSala +
                        "' tiene una posición fuera de rango. Se omitió.");
                    continue;
                }

                int recargo;

                if (!int.TryParse(nodoVIP.InnerText.Trim(), out recargo) || recargo < 0)
                {
                    resultado.AgregarAdvertencia("El asientoVIP en " + fila + "," + columna +
                        " de la sala '" + nombreSala + "' no tiene un recargo válido. Se omitió.");
                    continue;
                }

                Asiento actual = malla.ObtenerAsiento(fila, columna);

                if (!(actual is AsientoEstandar))
                {
                    resultado.AgregarAdvertencia("El asientoVIP en " + fila + "," + columna +
                        " de la sala '" + nombreSala + "' cae sobre un asiento de tipo " +
                        actual.ObtenerTipo() + ". Se omitió.");
                    continue;
                }

                malla.ReemplazarAsiento(fila, columna, new AsientoVIP(recargo));
            }
        }

        /// <summary>
        /// Busca dentro de una sala la etiqueta fila cuyo atributo numero
        /// coincide con el valor indicado.
        /// </summary>
        private XmlNode BuscarFilaPorNumero(XmlNode nodoSala, int numero)
        {
            XmlNodeList nodosFila = nodoSala.SelectNodes("fila");

            if (nodosFila == null)
            {
                return null;
            }

            foreach (XmlNode nodoFila in nodosFila)
            {
                if (LeerAtributoEntero(nodoFila, "numero", -1) == numero)
                {
                    return nodoFila;
                }
            }

            return null;
        }

        /// <summary>
        /// Extrae el contenido ubicado entre la primera y la última
        /// comilla del texto recibido.
        ///
        /// No se aplica Trim() al resultado: dentro de las comillas el
        /// carácter espacio representa un asiento disponible, por lo que
        /// recortarlo alteraría la longitud de la fila y deformaría la
        /// malla. Las comillas delimitan el contenido significativo.
        ///
        /// Devuelve null si el texto no contiene dos comillas.
        /// </summary>
        private string ExtraerEntreComillas(string texto)
        {
            if (texto == null)
            {
                return null;
            }

            int inicio = texto.IndexOf('"');
            int fin = texto.LastIndexOf('"');

            if (inicio < 0 || fin <= inicio)
            {
                return null;
            }

            return texto.Substring(inicio + 1, fin - inicio - 1);
        }

        /// <summary>
        /// Crea el objeto Asiento que corresponde a un carácter de la
        /// malla. Es el único punto donde se decide qué subclase de
        /// Asiento instanciar.
        ///
        /// Devuelve null cuando el carácter no está definido.
        /// </summary>
        private Asiento CrearAsiento(char simbolo)
        {
            if (simbolo == '*')
            {
                return new AsientoPasillo();
            }

            if (simbolo == ' ')
            {
                return new AsientoEstandar();
            }

            if (simbolo == 'O' || simbolo == 'o')
            {
                return new AsientoOcupado();
            }

            if (simbolo == 'E' || simbolo == 'e')
            {
                return new AsientoAccesible();
            }

            return null;
        }

        /// <summary>
        /// Obtiene el valor de un atributo como texto.
        /// Valida la existencia de la colección de atributos y del
        /// atributo solicitado para evitar referencias nulas.
        /// Devuelve valorPorDefecto cuando el atributo no está presente.
        /// </summary>
        private string LeerAtributoTexto(XmlNode nodo, string nombreAtributo, string valorPorDefecto)
        {
            if (nodo.Attributes == null)
            {
                return valorPorDefecto;
            }

            XmlAttribute atributo = nodo.Attributes[nombreAtributo];

            if (atributo == null)
            {
                return valorPorDefecto;
            }

            return atributo.Value.Trim();
        }

        /// <summary>
        /// Obtiene el valor de un atributo y lo convierte a entero.
        /// Se utiliza TryParse para que un valor no numérico devuelva el
        /// valor por defecto en lugar de generar una excepción.
        /// </summary>
        private int LeerAtributoEntero(XmlNode nodo, string nombreAtributo, int valorPorDefecto)
        {
            string texto = LeerAtributoTexto(nodo, nombreAtributo, "");

            int valor;

            if (int.TryParse(texto, out valor))
            {
                return valor;
            }

            return valorPorDefecto;
        }
    }
}
