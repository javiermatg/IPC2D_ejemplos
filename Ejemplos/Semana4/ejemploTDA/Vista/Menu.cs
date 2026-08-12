using System;
using ChapinCine.Modelo;
using ChapinCine.TDA;
using ChapinCine.Persistencia;
using ChapinCine.Demo;

namespace ChapinCine.Vista
{
    /// <summary>
    /// Capa de presentación. Concentra toda la interacción con el usuario
    /// mediante consola y coordina las llamadas a las demás capas del
    /// sistema.
    /// </summary>
    public class Menu
    {
        /// <summary>Estructura con las salas cargadas en el sistema.</summary>
        private ListaSalas listaSalas;

        /// <summary>Estructura con los clientes registrados.</summary>
        private ListaClientes listaClientes;

        /// <summary>Pila con el historial de operaciones ejecutadas.</summary>
        private PilaOperaciones historial;

        /// <summary>Cola utilizada para simular la fila de taquilla.</summary>
        private ColaClientes taquilla;

        /// <summary>Componente encargado de leer los archivos de configuración.</summary>
        private LectorXML lector;

        /// <summary>
        /// Recibe las estructuras ya instanciadas para que todas las
        /// opciones del menú operen sobre los mismos datos.
        /// </summary>
        public Menu(ListaSalas salas, ListaClientes clientes,
                    PilaOperaciones pila, ColaClientes cola)
        {
            listaSalas = salas;
            listaClientes = clientes;
            historial = pila;
            taquilla = cola;
            lector = new LectorXML();
        }

        /// <summary>
        /// Ciclo principal de la aplicación. Muestra el menú, lee la
        /// opción seleccionada y deriva a la operación correspondiente
        /// hasta que el usuario decide salir.
        /// </summary>
        public void Iniciar()
        {
            try
            {
                Console.Title = "ChapinCine";
            }
            catch (Exception)
            {
                // Algunas terminales no permiten modificar el título.
            }

            bool salir = false;

            while (!salir)
            {
                MostrarOpciones();
                string opcion = Console.ReadLine();

                if (opcion == null)
                {
                    opcion = "";
                }

                switch (opcion.Trim())
                {
                    case "0":
                        DemoArregloEstatico demo = new DemoArregloEstatico(3);
                        demo.Ejecutar();
                        break;

                    case "1":
                        OpcionCargarArchivo();
                        break;

                    case "2":
                        OpcionListarSalas();
                        break;

                    case "3":
                        OpcionListarClientes();
                        break;

                    case "4":
                        OpcionEliminarSala();
                        break;

                    case "5":
                        OpcionVerHistorial();
                        break;

                    case "6":
                        OpcionTaquilla();
                        break;

                    case "7":
                        salir = true;
                        Console.WriteLine();
                        Console.WriteLine("Sistema finalizado.");
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine(">> Opción no válida. Intente de nuevo.");
                        Pausa();
                        break;
                }
            }
        }

        /// <summary>
        /// Limpia la consola. Se controla la excepción que se produce
        /// cuando la salida está redirigida y no admite esta operación.
        /// </summary>
        private void LimpiarPantalla()
        {
            try
            {
                Console.Clear();
            }
            catch (Exception)
            {
                Console.WriteLine();
            }
        }

        /// <summary>Dibuja el menú principal con el estado actual del sistema.</summary>
        private void MostrarOpciones()
        {
            LimpiarPantalla();
            Console.WriteLine("==========================================================");
            Console.WriteLine("        CHAPINCINE - Sistema de Reservación");
            Console.WriteLine("==========================================================");
            Console.WriteLine("  Salas cargadas: " + listaSalas.Cantidad
                              + "   |   Clientes cargados: " + listaClientes.Cantidad);
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("  0. Demostración: limitaciones de los arreglos estáticos");
            Console.WriteLine("  1. Cargar archivo de configuración (XML)");
            Console.WriteLine("  2. Listar salas");
            Console.WriteLine("  3. Listar clientes");
            Console.WriteLine("  4. Eliminar una sala");
            Console.WriteLine("  5. Ver historial de operaciones");
            Console.WriteLine("  6. Simular fila de taquilla");
            Console.WriteLine("  7. Salir");
            Console.WriteLine("----------------------------------------------------------");
            Console.Write("  Seleccione una opción: ");
        }

        /// <summary>
        /// Solicita la ruta del archivo, invoca al lector y muestra el
        /// reporte devuelto.
        /// </summary>
        private void OpcionCargarArchivo()
        {
            Console.WriteLine();
            Console.WriteLine("--- CARGAR ARCHIVO DE CONFIGURACIÓN ---");
            Console.WriteLine("Escriba la ruta del archivo XML.");
            Console.WriteLine("(ENTER para usar: Datos/config_salas.xml)");
            Console.Write("Ruta: ");

            string ruta = Console.ReadLine();

            if (ruta == null || ruta.Trim().Length == 0)
            {
                ruta = "Datos/config_salas.xml";
            }
            else
            {
                ruta = ruta.Trim();
            }

            ResultadoCarga resultado =
                lector.CargarConfiguracion(ruta, listaSalas, listaClientes);

            Console.WriteLine();

            if (!resultado.Exito)
            {
                Console.WriteLine(">> ERROR: " + resultado.MensajeError);
                Pausa();
                return;
            }

            Console.WriteLine(">> Archivo cargado correctamente.");
            Console.WriteLine("   Salas nuevas .......... " + resultado.SalasNuevas);
            Console.WriteLine("   Salas actualizadas .... " + resultado.SalasActualizadas);
            Console.WriteLine("   Clientes nuevos ....... " + resultado.ClientesNuevos);
            Console.WriteLine("   Clientes actualizados . " + resultado.ClientesActualizados);

            if (resultado.TieneAdvertencias())
            {
                Console.WriteLine();
                Console.WriteLine("   ADVERTENCIAS:");
                Console.WriteLine(resultado.Advertencias);
            }

            historial.Apilar("Se cargó el archivo: " + ruta);
            Pausa();
        }

        /// <summary>
        /// Recorre la lista de salas desde el primer nodo hasta el final
        /// y muestra los datos de cada una.
        /// </summary>
        private void OpcionListarSalas()
        {
            Console.WriteLine();
            Console.WriteLine("--- SALAS REGISTRADAS ---");

            if (listaSalas.EstaVacia())
            {
                Console.WriteLine("No hay salas cargadas. Use la opción 1 primero.");
                Pausa();
                return;
            }

            NodoSala actual = listaSalas.Primero;
            int numero = 1;

            while (actual != null)
            {
                Sala sala = actual.Dato;

                Console.WriteLine("  " + numero + ". " + sala.Nombre
                                  + "  [" + sala.Filas + " filas x " + sala.Columnas + " columnas"
                                  + " = " + sala.TotalCeldas() + " celdas]");

                actual = actual.Siguiente;
                numero = numero + 1;
            }

            Console.WriteLine();
            Console.WriteLine("  Total: " + listaSalas.Cantidad + " sala(s).");
            Pausa();
        }

        /// <summary>
        /// Recorre la lista de clientes y muestra la descripción de cada
        /// uno. La descripción y la validación de recargo se resuelven por
        /// polimorfismo, sin evaluar el tipo concreto del objeto.
        /// </summary>
        private void OpcionListarClientes()
        {
            Console.WriteLine();
            Console.WriteLine("--- CLIENTES REGISTRADOS ---");

            if (listaClientes.EstaVacia())
            {
                Console.WriteLine("No hay clientes cargados. Use la opción 1 primero.");
                Pausa();
                return;
            }

            NodoCliente actual = listaClientes.Primero;
            int numero = 1;

            while (actual != null)
            {
                Console.WriteLine("  " + numero + ". " + actual.Dato.ObtenerDescripcion());

                actual = actual.Siguiente;
                numero = numero + 1;
            }

            Console.WriteLine();
            Console.WriteLine("  Total: " + listaClientes.Cantidad + " cliente(s).");
            Console.WriteLine("  Estándar: " + listaClientes.ContarPorTipo("ClienteEstandar")
                              + "   |   Premium: " + listaClientes.ContarPorTipo("ClientePremium"));

            Console.WriteLine();
            Console.WriteLine("  --- Validación de recargo de Q50 por cliente ---");

            actual = listaClientes.Primero;

            while (actual != null)
            {
                string respuesta;

                if (actual.Dato.PuedeCubrirRecargo(50))
                {
                    respuesta = "SÍ";
                }
                else
                {
                    respuesta = "NO";
                }

                Console.WriteLine("     " + actual.Dato.Nombre + " -> " + respuesta);
                actual = actual.Siguiente;
            }

            Pausa();
        }

        /// <summary>
        /// Solicita el nombre de una sala y la elimina de la lista.
        /// </summary>
        private void OpcionEliminarSala()
        {
            Console.WriteLine();
            Console.WriteLine("--- ELIMINAR SALA ---");

            if (listaSalas.EstaVacia())
            {
                Console.WriteLine("No hay salas cargadas. Use la opción 1 primero.");
                Pausa();
                return;
            }

            Console.Write("Nombre de la sala a eliminar: ");
            string nombre = Console.ReadLine();

            if (nombre == null || nombre.Trim().Length == 0)
            {
                Console.WriteLine(">> No se ingresó ningún nombre.");
                Pausa();
                return;
            }

            nombre = nombre.Trim();

            if (listaSalas.EliminarPorNombre(nombre))
            {
                Console.WriteLine(">> Sala eliminada correctamente.");
                historial.Apilar("Se eliminó la sala: " + nombre);
            }
            else
            {
                Console.WriteLine(">> No se encontró ninguna sala con ese nombre.");
            }

            Pausa();
        }

        /// <summary>
        /// Recorre la pila desde la cima siguiendo los enlaces, sin
        /// extraer elementos, para no destruir el historial al mostrarlo.
        /// </summary>
        private void OpcionVerHistorial()
        {
            Console.WriteLine();
            Console.WriteLine("--- HISTORIAL DE OPERACIONES ---");

            if (historial.EstaVacia())
            {
                Console.WriteLine("No hay operaciones registradas.");
                Pausa();
                return;
            }

            NodoPila actual = historial.VerCima();
            int numero = 1;

            while (actual != null)
            {
                Console.WriteLine("  " + numero + ". " + actual.Dato);
                actual = actual.Siguiente;
                numero = numero + 1;
            }

            Console.WriteLine();
            Console.WriteLine("  Orden LIFO: la operación más reciente aparece primero.");
            Pausa();
        }

        /// <summary>
        /// Encola a todos los clientes registrados y luego los desencola,
        /// mostrando que el orden de salida coincide con el de entrada.
        /// </summary>
        private void OpcionTaquilla()
        {
            Console.WriteLine();
            Console.WriteLine("--- FILA DE TAQUILLA ---");

            if (listaClientes.EstaVacia())
            {
                Console.WriteLine("No hay clientes cargados. Use la opción 1 primero.");
                Pausa();
                return;
            }

            Console.WriteLine("Ingreso de clientes a la fila:");

            NodoCliente actual = listaClientes.Primero;

            while (actual != null)
            {
                taquilla.Encolar(actual.Dato);
                Console.WriteLine("  ingresa -> " + actual.Dato.Nombre);
                actual = actual.Siguiente;
            }

            Console.WriteLine();
            Console.WriteLine("Atención de la fila:");

            while (!taquilla.EstaVacia())
            {
                Cliente atendido = taquilla.Desencolar();
                Console.WriteLine("  se atiende -> " + atendido.Nombre);
            }

            Console.WriteLine();
            Console.WriteLine("  Orden FIFO: la salida respeta el orden de entrada.");

            historial.Apilar("Se simuló la fila de taquilla");
            Pausa();
        }

        private void Pausa()
        {
            Console.WriteLine();
            Console.Write("Presione ENTER para continuar...");
            Console.ReadLine();
        }
    }
}
