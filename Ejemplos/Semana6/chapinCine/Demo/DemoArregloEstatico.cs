using System;

namespace ChapinCine.Demo
{
    /// <summary>
    /// Rutina de demostración que expone las limitaciones de un arreglo
    /// de tamaño fijo: capacidad definida de antemano, desbordamiento al
    /// llenarse, desplazamiento manual de elementos al eliminar y espacio
    /// reservado sin utilizar.
    /// Es el único componente del sistema que trabaja con un arreglo; el
    /// resto de las estructuras son dinámicas.
    /// </summary>
    public class DemoArregloEstatico
    {
        /// <summary>Arreglo de tamaño fijo definido al construir el objeto.</summary>
        private string[] salas;

        /// <summary>
        /// Cantidad de posiciones ocupadas. Se administra manualmente
        /// porque el arreglo solo conoce su capacidad total.
        /// </summary>
        private int cantidad;

        public DemoArregloEstatico(int capacidad)
        {
            salas = new string[capacidad];
            cantidad = 0;
        }

        /// <summary>
        /// Ejecuta la secuencia de demostración mostrando cada limitación
        /// con una pausa intermedia.
        /// </summary>
        public void Ejecutar()
        {
            try
            {
                Console.Clear();
            }
            catch (Exception)
            {
                Console.WriteLine();
            }

            Console.WriteLine("========================================================");
            Console.WriteLine("   DEMOSTRACIÓN: LIMITACIONES DE LOS ARREGLOS ESTÁTICOS");
            Console.WriteLine("========================================================");
            Console.WriteLine();

            Console.WriteLine("LIMITACIÓN 1: La capacidad se define antes de conocer los datos.");
            Console.WriteLine("Se crea un arreglo con capacidad para 3 salas.");
            Console.WriteLine();
            Mostrar();
            Pausa();

            Console.WriteLine("Se agregan 3 salas:");
            Agregar("Sala Premier");
            Agregar("Sala VIP");
            Agregar("Sala 3D");
            Mostrar();
            Pausa();

            Console.WriteLine("LIMITACIÓN 2: Desbordamiento. Se intenta agregar una cuarta sala.");
            Agregar("Sala IMAX");
            Mostrar();
            Console.WriteLine(">> El arreglo alcanzó su capacidad. Para crecer sería necesario");
            Console.WriteLine(">> crear otro arreglo mayor y copiar todos los elementos.");
            Pausa();

            Console.WriteLine("LIMITACIÓN 3: Eliminar un elemento intermedio.");
            Console.WriteLine("Se elimina 'Sala VIP', ubicada en la posición 1.");
            Console.WriteLine();
            EliminarPorIndice(1);
            Mostrar();
            Pausa();

            Console.WriteLine("LIMITACIÓN 4: Espacio reservado sin utilizar.");
            Console.WriteLine("El arreglo mantiene capacidad para " + salas.Length + " elementos");
            Console.WriteLine("aunque solo se estén usando " + cantidad + ".");
            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("RESUMEN:");
            Console.WriteLine("  1. La capacidad se define antes de conocer los datos.");
            Console.WriteLine("  2. Al llenarse, se debe copiar todo a un arreglo mayor.");
            Console.WriteLine("  3. Eliminar un elemento intermedio desplaza los siguientes.");
            Console.WriteLine("  4. Se reserva memoria en posiciones no utilizadas.");
            Console.WriteLine();
            Console.WriteLine("Las estructuras dinámicas asignan memoria por nodo conforme");
            Console.WriteLine("se necesita, y sus operaciones solo modifican referencias.");
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine();
            Console.Write("Presione ENTER para volver al menú...");
            Console.ReadLine();
        }

        /// <summary>
        /// Agrega un elemento validando previamente que exista espacio
        /// disponible en el arreglo.
        /// </summary>
        private void Agregar(string nombre)
        {
            if (cantidad >= salas.Length)
            {
                Console.WriteLine("  [ERROR] No cabe '" + nombre + "': el arreglo está lleno.");
                return;
            }

            salas[cantidad] = nombre;
            cantidad = cantidad + 1;
            Console.WriteLine("  [OK] Agregada: " + nombre);
        }

        /// <summary>
        /// Elimina el elemento de la posición indicada desplazando una
        /// posición a la izquierda todos los elementos posteriores.
        /// </summary>
        private void EliminarPorIndice(int indice)
        {
            if (indice < 0 || indice >= cantidad)
            {
                return;
            }

            Console.WriteLine("  Eliminando '" + salas[indice] + "' en la posición " + indice + ".");
            Console.WriteLine("  Se desplazan a la izquierda los elementos posteriores:");

            int i = indice;

            while (i < cantidad - 1)
            {
                Console.WriteLine("     mover '" + salas[i + 1] + "' de la posición "
                                  + (i + 1) + " a la " + i);
                salas[i] = salas[i + 1];
                i = i + 1;
            }

            salas[cantidad - 1] = null;
            cantidad = cantidad - 1;
            Console.WriteLine();
        }

        /// <summary>
        /// Muestra el contenido completo del arreglo, incluyendo las
        /// posiciones reservadas que no están en uso.
        /// </summary>
        private void Mostrar()
        {
            Console.WriteLine("  Estado del arreglo (capacidad " + salas.Length
                              + ", usadas " + cantidad + "):");

            int i = 0;

            while (i < salas.Length)
            {
                string contenido = salas[i];

                if (contenido == null)
                {
                    contenido = "(posición reservada sin usar)";
                }

                Console.WriteLine("     [" + i + "] " + contenido);
                i = i + 1;
            }

            Console.WriteLine();
        }

        private void Pausa()
        {
            Console.Write("   ...ENTER para continuar... ");
            Console.ReadLine();
            Console.WriteLine();
        }
    }
}
