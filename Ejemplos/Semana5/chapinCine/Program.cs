using ChapinCine.TDA;
using ChapinCine.Vista;

namespace ChapinCine
{
    /// <summary>
    /// Punto de entrada de la aplicación. Instancia las estructuras de
    /// datos del sistema, las inyecta en la capa de presentación y cede
    /// el control al ciclo principal del menú.
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            // Estructuras que persisten durante toda la ejecución.
            ListaSalas listaSalas = new ListaSalas();
            ListaClientes listaClientes = new ListaClientes();
            PilaOperaciones historial = new PilaOperaciones();
            ColaClientes taquilla = new ColaClientes();
            PilaReservaciones reservaciones = new PilaReservaciones();

            // Se instancia el menú con las estructuras ya creadas para que
            // todas las operaciones trabajen sobre los mismos datos.
            Menu menu = new Menu(listaSalas, listaClientes, historial,
                                 taquilla, reservaciones);
            menu.Iniciar();
        }
    }
}
