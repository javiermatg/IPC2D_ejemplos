using System;
using ChapinCine.Modelo;
using ChapinCine.TDA;
using ChapinCine.Persistencia;
using ChapinCine.Demo;
using ChapinCine.Logica;

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

        /// <summary>Pila con las reservaciones realizadas, para poder deshacerlas.</summary>
        private PilaReservaciones reservaciones;

        /// <summary>Componente que ejecuta la lógica de reservación.</summary>
        private GestorReservas gestor;

        /// <summary>
        /// Recibe las estructuras ya instanciadas para que todas las
        /// opciones del menú operen sobre los mismos datos.
        /// </summary>
        public Menu(ListaSalas salas, ListaClientes clientes,
                    PilaOperaciones pila, ColaClientes cola,
                    PilaReservaciones pilaReservaciones)
        {
            listaSalas = salas;
            listaClientes = clientes;
            historial = pila;
            taquilla = cola;
            reservaciones = pilaReservaciones;
            lector = new LectorXML();
            gestor = new GestorReservas();
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
                        OpcionDibujarSala();
                        break;

                    case "5":
                        OpcionInspeccionarCelda();
                        break;

                    case "6":
                        OpcionReservar();
                        break;

                    case "7":
                        OpcionDeshacer();
                        break;

                    case "8":
                        OpcionVerReservaciones();
                        break;

                    case "9":
                        OpcionEliminarSala();
                        break;

                    case "10":
                        OpcionVerHistorial();
                        break;

                    case "11":
                        OpcionTaquilla();
                        break;

                    case "12":
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
            Console.WriteLine("  Salas: " + listaSalas.Cantidad
                              + "   |   Clientes: " + listaClientes.Cantidad
                              + "   |   Reservaciones: " + reservaciones.Cantidad);
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("  0. Demostración: limitaciones de los arreglos estáticos");
            Console.WriteLine("  1. Cargar archivo de configuración (XML)");
            Console.WriteLine("  2. Listar salas");
            Console.WriteLine("  3. Listar clientes");
            Console.WriteLine("  4. Dibujar la malla de una sala");
            Console.WriteLine("  5. Inspeccionar una celda y sus vecinos");
            Console.WriteLine("  6. Reservar asientos");
            Console.WriteLine("  7. Deshacer la última reservación");
            Console.WriteLine("  8. Ver reservaciones realizadas");
            Console.WriteLine("  9. Eliminar una sala");
            Console.WriteLine(" 10. Ver historial de operaciones");
            Console.WriteLine(" 11. Simular fila de taquilla");
            Console.WriteLine(" 12. Salir");
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
        /// Permite elegir una sala y dibuja su malla de asientos en la
        /// consola, junto con la referencia de símbolos y el conteo de
        /// asientos reservables.
        /// </summary>
        private void OpcionDibujarSala()
        {
            Console.WriteLine();
            Console.WriteLine("--- MALLA DE ASIENTOS ---");

            if (listaSalas.EstaVacia())
            {
                Console.WriteLine("No hay salas cargadas. Use la opción 1 primero.");
                Pausa();
                return;
            }

            Sala sala = SeleccionarSala();

            if (sala == null)
            {
                Console.WriteLine(">> Selección no válida.");
                Pausa();
                return;
            }

            if (!sala.TieneMalla())
            {
                Console.WriteLine(">> Esta sala no tiene una malla cargada.");
                Pausa();
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Sala: " + sala.Nombre);
            Console.WriteLine();

            DibujarMalla(sala.Asientos);

            Console.WriteLine();
            Console.WriteLine("  Referencia:");
            Console.WriteLine("    *  pasillo o estructura");
            Console.WriteLine("    .  asiento disponible");
            Console.WriteLine("    O  asiento ocupado");
            Console.WriteLine("    E  espacio accesible");
            Console.WriteLine("    V  asiento VIP (con recargo)");

            Console.WriteLine();
            Console.WriteLine("  Asientos reservables sin considerar presupuesto: "
                              + sala.Asientos.ContarReservablesPor(null));

            // El mismo método se invoca una vez por cliente. El resultado
            // varía porque los asientos con recargo consultan el
            // presupuesto de quien se les pasa.
            if (!listaClientes.EstaVacia())
            {
                Console.WriteLine();
                Console.WriteLine("  Reservables según el cliente:");

                NodoCliente actual = listaClientes.Primero;

                while (actual != null)
                {
                    Console.WriteLine("    " + actual.Dato.Nombre + " -> "
                                      + sala.Asientos.ContarReservablesPor(actual.Dato));
                    actual = actual.Siguiente;
                }
            }

            historial.Apilar("Se dibujó la malla de: " + sala.Nombre);
            Pausa();
        }

        /// <summary>
        /// Dibuja la malla recorriendo fila por fila mediante los enlaces
        /// horizontales de cada nodo.
        ///
        /// El asiento estándar se representa con un punto en lugar de un
        /// espacio para que sea visible en pantalla; el símbolo definido
        /// en la clase sigue siendo el espacio.
        /// </summary>
        private void DibujarMalla(MallaAsientos malla)
        {
            // Encabezado con los números de columna, en dos líneas para
            // que los números de dos dígitos queden alineados.
            Console.Write("      ");

            int c = 1;
            while (c <= malla.TotalColumnas)
            {
                Console.Write(c / 10);
                c = c + 1;
            }
            Console.WriteLine();

            Console.Write("      ");

            c = 1;
            while (c <= malla.TotalColumnas)
            {
                Console.Write(c % 10);
                c = c + 1;
            }
            Console.WriteLine();

            int fila = 1;

            while (fila <= malla.TotalFilas)
            {
                Console.Write("  " + fila.ToString().PadLeft(2) + "  ");

                // Recorrido horizontal siguiendo los punteros Derecha.
                NodoAsiento actual = malla.ObtenerPrimeroDeFila(fila);

                while (actual != null)
                {
                    char simbolo = actual.Dato.Simbolo;

                    if (simbolo == ' ')
                    {
                        simbolo = '.';
                    }

                    Console.Write(simbolo);
                    actual = actual.Derecha;
                }

                Console.WriteLine();
                fila = fila + 1;
            }
        }

        /// <summary>
        /// Permite elegir una sala del listado. Devuelve null si el
        /// usuario cancela o si la selección no es válida.
        /// </summary>
        private Sala SeleccionarSala()
        {
            NodoSala nodo = listaSalas.Primero;
            int numero = 1;

            while (nodo != null)
            {
                Console.WriteLine("  " + numero + ". " + nodo.Dato.Nombre);
                nodo = nodo.Siguiente;
                numero = numero + 1;
            }

            Console.WriteLine();
            Console.Write("Seleccione el número de la sala: ");

            string entrada = Console.ReadLine();
            int seleccion;

            if (!int.TryParse(entrada, out seleccion) ||
                seleccion < 1 || seleccion > listaSalas.Cantidad)
            {
                return null;
            }

            return listaSalas.ObtenerPorIndice(seleccion - 1);
        }

        /// <summary>
        /// Permite elegir un cliente del listado. Devuelve null si la
        /// selección no es válida.
        /// </summary>
        private Cliente SeleccionarCliente()
        {
            NodoCliente nodo = listaClientes.Primero;
            int numero = 1;

            while (nodo != null)
            {
                Console.WriteLine("  " + numero + ". " + nodo.Dato.ObtenerDescripcion());
                nodo = nodo.Siguiente;
                numero = numero + 1;
            }

            Console.WriteLine();
            Console.Write("Seleccione el número del cliente: ");

            string entrada = Console.ReadLine();
            int seleccion;

            if (!int.TryParse(entrada, out seleccion) ||
                seleccion < 1 || seleccion > listaClientes.Cantidad)
            {
                return null;
            }

            return listaClientes.ObtenerPorIndice(seleccion - 1);
        }

        /// <summary>
        /// Muestra los datos de una celda de la malla junto con sus
        /// cuatro vecinos, obtenidos directamente de las referencias
        /// Arriba, Abajo, Izquierda y Derecha del nodo.
        /// </summary>
        private void OpcionInspeccionarCelda()
        {
            Console.WriteLine();
            Console.WriteLine("--- INSPECCIONAR CELDA ---");

            if (listaSalas.EstaVacia())
            {
                Console.WriteLine("No hay salas cargadas. Use la opción 1 primero.");
                Pausa();
                return;
            }

            Sala sala = SeleccionarSala();

            if (sala == null)
            {
                Console.WriteLine(">> Selección no válida.");
                Pausa();
                return;
            }

            if (!sala.TieneMalla())
            {
                Console.WriteLine(">> Esta sala no tiene una malla cargada.");
                Pausa();
                return;
            }

            Console.WriteLine();
            Console.Write("Fila (1 a " + sala.Asientos.TotalFilas + "): ");
            string textoFila = Console.ReadLine();

            Console.Write("Columna (1 a " + sala.Asientos.TotalColumnas + "): ");
            string textoColumna = Console.ReadLine();

            int fila;
            int columna;

            if (!int.TryParse(textoFila, out fila) || !int.TryParse(textoColumna, out columna))
            {
                Console.WriteLine(">> Debe ingresar números.");
                Pausa();
                return;
            }

            NodoAsiento nodo = sala.Asientos.ObtenerNodo(fila, columna);

            if (nodo == null)
            {
                Console.WriteLine(">> Esa posición no existe en la malla.");
                Pausa();
                return;
            }

            Console.WriteLine();
            Console.WriteLine("  Celda (" + nodo.Fila + "," + nodo.Columna + ")");
            Console.WriteLine("    Tipo ......... " + nodo.Dato.ObtenerTipo());
            Console.WriteLine("    Símbolo ...... '" + nodo.Dato.Simbolo + "'");
            Console.WriteLine("    Recargo ...... Q" + nodo.Dato.ObtenerRecargo());
            Console.WriteLine();
            Console.WriteLine("  Vecinos (obtenidos con los cuatro punteros):");
            Console.WriteLine("    Arriba ....... " + DescribirVecino(nodo.Arriba));
            Console.WriteLine("    Abajo ........ " + DescribirVecino(nodo.Abajo));
            Console.WriteLine("    Izquierda .... " + DescribirVecino(nodo.Izquierda));
            Console.WriteLine("    Derecha ...... " + DescribirVecino(nodo.Derecha));
            Console.WriteLine();
            Console.WriteLine("  Un vecino en 'null' significa que la celda está en un borde.");

            Pausa();
        }

        /// <summary>
        /// Devuelve una descripción corta de un nodo vecino, o el texto
        /// correspondiente cuando la referencia es null.
        /// </summary>
        private string DescribirVecino(NodoAsiento vecino)
        {
            if (vecino == null)
            {
                return "null (borde de la malla)";
            }

            return "(" + vecino.Fila + "," + vecino.Columna + ") " + vecino.Dato.ObtenerTipo();
        }

        /// <summary>
        /// Solicita sala, cliente y cantidad, y ejecuta la reservación a
        /// través del gestor. Muestra el resultado y la malla actualizada.
        /// </summary>
        private void OpcionReservar()
        {
            Console.WriteLine();
            Console.WriteLine("--- RESERVAR ASIENTOS ---");

            if (listaSalas.EstaVacia() || listaClientes.EstaVacia())
            {
                Console.WriteLine("Se necesitan salas y clientes cargados. Use la opción 1.");
                Pausa();
                return;
            }

            Sala sala = SeleccionarSala();

            if (sala == null || !sala.TieneMalla())
            {
                Console.WriteLine(">> Sala no válida o sin malla cargada.");
                Pausa();
                return;
            }

            Console.WriteLine();
            Cliente cliente = SeleccionarCliente();

            if (cliente == null)
            {
                Console.WriteLine(">> Cliente no válido.");
                Pausa();
                return;
            }

            Console.WriteLine();
            Console.Write("¿Cuántos asientos contiguos necesita? ");

            string entrada = Console.ReadLine();
            int cantidad;

            if (!int.TryParse(entrada, out cantidad) || cantidad < 1)
            {
                Console.WriteLine(">> Debe ingresar un número mayor que cero.");
                Pausa();
                return;
            }

            Reservacion reservacion = gestor.Reservar(sala, cliente, cantidad);

            Console.WriteLine();

            if (reservacion == null)
            {
                int maximo = gestor.CalcularBloqueMaximo(sala, cliente);

                Console.WriteLine(">> No fue posible reservar " + cantidad + " asiento(s).");
                Console.WriteLine("   Como máximo hay " + maximo + " asiento(s) seguidos que este");
                Console.WriteLine("   cliente podría tomar, evaluados uno por uno.");
                Console.WriteLine();
                Console.WriteLine("   Ese número es un LÍMITE SUPERIOR, no una garantía: al");
                Console.WriteLine("   reservar de verdad el presupuesto se va gastando, y un");
                Console.WriteLine("   bloque con varios asientos VIP puede resultar impagable");
                Console.WriteLine("   aunque cada asiento por separado sí fuera alcanzable.");

                Pausa();
                return;
            }

            reservaciones.Apilar(reservacion);
            historial.Apilar("Reservación: " + reservacion.ObtenerDescripcion());

            Console.WriteLine(">> Reservación realizada.");
            Console.WriteLine("   " + reservacion.ObtenerDescripcion());

            ClientePremium premium = cliente as ClientePremium;

            if (premium != null)
            {
                Console.WriteLine("   Presupuesto: Q" + premium.PresupuestoInicial
                                  + " inicial, Q" + premium.Presupuesto + " restante.");
            }

            Console.WriteLine();
            Console.WriteLine("Malla actualizada:");
            Console.WriteLine();
            DibujarMalla(sala.Asientos);

            Pausa();
        }

        /// <summary>
        /// Deshace la última reservación realizada, extrayéndola de la
        /// pila y solicitando al gestor que restaure el estado anterior.
        /// </summary>
        private void OpcionDeshacer()
        {
            Console.WriteLine();
            Console.WriteLine("--- DESHACER ÚLTIMA RESERVACIÓN ---");

            if (reservaciones.EstaVacia())
            {
                Console.WriteLine("No hay reservaciones que deshacer.");
                Pausa();
                return;
            }

            Reservacion reservacion = reservaciones.Desapilar();

            if (!gestor.Deshacer(reservacion))
            {
                Console.WriteLine(">> No fue posible deshacer la reservación.");
                Pausa();
                return;
            }

            Console.WriteLine(">> Reservación deshecha:");
            Console.WriteLine("   " + reservacion.ObtenerDescripcion());

            ClientePremium premium = reservacion.Cliente as ClientePremium;

            if (premium != null)
            {
                Console.WriteLine("   Presupuesto restaurado a Q" + premium.Presupuesto + ".");
            }

            historial.Apilar("Se deshizo: " + reservacion.ObtenerDescripcion());

            Console.WriteLine();
            Console.WriteLine("Malla actualizada:");
            Console.WriteLine();
            DibujarMalla(reservacion.Sala.Asientos);

            Pausa();
        }

        /// <summary>
        /// Muestra las reservaciones vigentes, de la más reciente a la
        /// más antigua, recorriendo la pila sin extraer elementos.
        /// </summary>
        private void OpcionVerReservaciones()
        {
            Console.WriteLine();
            Console.WriteLine("--- RESERVACIONES REALIZADAS ---");

            if (reservaciones.EstaVacia())
            {
                Console.WriteLine("No hay reservaciones registradas.");
                Pausa();
                return;
            }

            NodoReservacion actual = reservaciones.VerCima();
            int numero = 1;

            while (actual != null)
            {
                Console.WriteLine("  " + numero + ". " + actual.Dato.ObtenerDescripcion());
                actual = actual.Siguiente;
                numero = numero + 1;
            }

            Console.WriteLine();
            Console.WriteLine("  La primera de la lista es la que se deshace con la opción 7.");

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
