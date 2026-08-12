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
