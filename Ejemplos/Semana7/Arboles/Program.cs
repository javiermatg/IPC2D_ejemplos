using System;

namespace EjemploBasicoArbol
{
    // ============================================================================
    //  EJEMPLO BASICO DE ARBOL BINARIO EN C#
    //
    //  Un arbol no es una estructura magica: es una clase con un dato y dos
    //  referencias a objetos de su mismo tipo. Igual que una lista enlazada,
    //  pero con dos "Siguiente" en lugar de uno.
    //
    //  El arbol que se construye en este ejemplo:
    //
    //                    50
    //                  /    \
    //                30      70
    //               /  \    /  \
    //             20   40  60   80
    // ============================================================================

    /// <summary>
    /// Nodo del arbol binario. Guarda un valor entero y las referencias a sus dos
    /// hijos. Cuando una referencia vale null, ese hijo no existe.
    /// </summary>
    public class Nodo
    {
        /// <summary>Valor almacenado en el nodo.</summary>
        public int Valor { get; set; }

        /// <summary>Hijo izquierdo; null si no tiene.</summary>
        public Nodo Izquierda { get; set; }

        /// <summary>Hijo derecho; null si no tiene.</summary>
        public Nodo Derecha { get; set; }

        /// <summary>Crea un nodo suelto, sin hijos.</summary>
        /// <param name="valor">Valor a almacenar.</param>
        public Nodo(int valor)
        {
            Valor = valor;
            Izquierda = null;
            Derecha = null;
        }
    }

    /// <summary>
    /// Operaciones basicas sobre el arbol. Todas son recursivas porque un arbol
    /// esta definido de forma recursiva: un nodo con dos arboles adentro.
    /// </summary>
    public class ArbolBinario
    {
        /// <summary>Primer nodo del arbol; null cuando el arbol esta vacio.</summary>
        public Nodo Raiz { get; set; }

        /// <summary>
        /// Inserta un valor respetando la regla del arbol binario de busqueda:
        /// los menores van a la izquierda y los mayores a la derecha.
        /// </summary>
        /// <param name="valor">Valor a insertar.</param>
        public void Insertar(int valor)
        {
            Raiz = InsertarRecursivo(Raiz, valor);
        }

        /// <summary>
        /// Baja por el arbol hasta encontrar un espacio vacio (null) y coloca ahi
        /// el nodo nuevo. Devuelve el subarbol para que el padre lo reconecte.
        /// </summary>
        /// <param name="actual">Nodo que se esta revisando.</param>
        /// <param name="valor">Valor a insertar.</param>
        /// <returns>Raiz del subarbol despues de insertar.</returns>
        private Nodo InsertarRecursivo(Nodo actual, int valor)
        {
            if (actual == null)
            {
                return new Nodo(valor);
            }

            if (valor < actual.Valor)
            {
                actual.Izquierda = InsertarRecursivo(actual.Izquierda, valor);
            }
            else if (valor > actual.Valor)
            {
                actual.Derecha = InsertarRecursivo(actual.Derecha, valor);
            }

            return actual;
        }

        /// <summary>
        /// Recorrido inorden: izquierda, nodo, derecha.
        /// En un arbol de busqueda entrega los valores ordenados de menor a mayor.
        /// </summary>
        /// <param name="actual">Nodo actual del recorrido.</param>
        public void Inorden(Nodo actual)
        {
            if (actual == null)
            {
                return;
            }

            Inorden(actual.Izquierda);
            Console.Write(actual.Valor + " ");
            Inorden(actual.Derecha);
        }

        /// <summary>Recorrido preorden: nodo, izquierda, derecha.</summary>
        /// <param name="actual">Nodo actual del recorrido.</param>
        public void Preorden(Nodo actual)
        {
            if (actual == null)
            {
                return;
            }

            Console.Write(actual.Valor + " ");
            Preorden(actual.Izquierda);
            Preorden(actual.Derecha);
        }

        /// <summary>Recorrido postorden: izquierda, derecha, nodo.</summary>
        /// <param name="actual">Nodo actual del recorrido.</param>
        public void Postorden(Nodo actual)
        {
            if (actual == null)
            {
                return;
            }

            Postorden(actual.Izquierda);
            Postorden(actual.Derecha);
            Console.Write(actual.Valor + " ");
        }

        /// <summary>
        /// Cuenta los nodos del arbol: uno por el nodo actual, mas los de cada lado.
        /// </summary>
        /// <param name="actual">Raiz del subarbol.</param>
        /// <returns>Cantidad de nodos.</returns>
        public int ContarNodos(Nodo actual)
        {
            if (actual == null)
            {
                return 0;
            }

            return 1 + ContarNodos(actual.Izquierda) + ContarNodos(actual.Derecha);
        }

        /// <summary>
        /// Altura del arbol en aristas: el camino mas largo de la raiz a una hoja.
        /// </summary>
        /// <param name="actual">Raiz del subarbol.</param>
        /// <returns>Altura del subarbol; -1 si esta vacio.</returns>
        public int Altura(Nodo actual)
        {
            if (actual == null)
            {
                return -1;
            }

            int izquierda = Altura(actual.Izquierda);
            int derecha = Altura(actual.Derecha);

            return (izquierda > derecha ? izquierda : derecha) + 1;
        }

        /// <summary>Busca un valor bajando por una sola rama del arbol.</summary>
        /// <param name="valor">Valor buscado.</param>
        /// <returns>true si el valor esta en el arbol.</returns>
        public bool Buscar(int valor)
        {
            Nodo actual = Raiz;

            while (actual != null)
            {
                if (valor == actual.Valor)
                {
                    return true;
                }

                actual = valor < actual.Valor ? actual.Izquierda : actual.Derecha;
            }

            return false;
        }
    }

    /// <summary>Programa de demostracion.</summary>
    public static class Programa
    {
        /// <summary>Construye el arbol de dos formas y muestra los recorridos.</summary>
        /// <param name="args">Argumentos de linea de comandos; no se utilizan.</param>
        public static void Main(string[] args)
        {
            // ---------- FORMA 1: armar el arbol a mano ----------
            // Se crean los nodos y se conectan asignando las referencias.
            // Esto deja claro que el arbol solo son objetos apuntando a objetos.
            Nodo raiz = new Nodo(50);
            raiz.Izquierda = new Nodo(30);
            raiz.Derecha = new Nodo(70);
            raiz.Izquierda.Izquierda = new Nodo(20);
            raiz.Izquierda.Derecha = new Nodo(40);
            raiz.Derecha.Izquierda = new Nodo(60);
            raiz.Derecha.Derecha = new Nodo(80);

            ArbolBinario manual = new ArbolBinario();
            manual.Raiz = raiz;

            Console.WriteLine("Arbol armado a mano");
            Console.Write("Inorden   : ");
            manual.Inorden(manual.Raiz);
            Console.WriteLine();

            // ---------- FORMA 2: dejar que el arbol se acomode solo ----------
            // Se insertan los mismos numeros en desorden. La regla "menor a la
            // izquierda, mayor a la derecha" reconstruye exactamente el arbol
            // anterior, y el inorden vuelve a salir ordenado.
            ArbolBinario automatico = new ArbolBinario();
            automatico.Insertar(50);
            automatico.Insertar(30);
            automatico.Insertar(70);
            automatico.Insertar(20);
            automatico.Insertar(40);
            automatico.Insertar(60);
            automatico.Insertar(80);

            Console.WriteLine();
            Console.WriteLine("Arbol construido con Insertar");
            Console.Write("Inorden   : ");
            automatico.Inorden(automatico.Raiz);
            Console.WriteLine();

            Console.Write("Preorden  : ");
            automatico.Preorden(automatico.Raiz);
            Console.WriteLine();

            Console.Write("Postorden : ");
            automatico.Postorden(automatico.Raiz);
            Console.WriteLine();

            Console.WriteLine();
            Console.WriteLine("Nodos  : " + automatico.ContarNodos(automatico.Raiz));
            Console.WriteLine("Altura : " + automatico.Altura(automatico.Raiz));
            Console.WriteLine("Busca 40: " + automatico.Buscar(40));
            Console.WriteLine("Busca 45: " + automatico.Buscar(45));

            Console.WriteLine();
            Console.WriteLine("Presione una tecla para salir.");
            Console.ReadKey();
        }
    }
}
