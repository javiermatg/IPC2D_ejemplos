// ============================================================================
//ARRAYS (ARREGLOS)
// ============================================================================

Console.WriteLine("=== EXPLICACIÓN PRÁCTICA DE ARRAYS (ARREGLOS) EN C# ===\n");

// ============================================================================
// 1. DECLARACIÓN E INICIALIZACIÓN DE ARRAYS UNIDIMENSIONALES
// ----------------------------------------------------------------------------
// REGLAS CLAVE DE UN ARRAY EN C#:
// 1. Son de TAMAÑO FIJO: Una vez definido su tamaño, NO puede crecer ni achicarse.
// 2. Son HOMOGÉNEOS: Todos sus elementos deben ser del mismo tipo de dato.
// 3. Basados en ÍNDICE CERO: El primer elemento siempre está en la posición [0].
// ----------------------------------------------------------------------------
Console.WriteLine("--- 1. DECLARACIÓN E ÍNDICES ---");

// Forma 1: Declarar indicando el tamaño exacto (se inicializa con valores por defecto, en int es 0)
int[] numeros = new int[3]; 
numeros[0] = 10; // Asignamos valor en la primera posición (índice 0)
numeros[1] = 20; // Segunda posición
numeros[2] = 30; // Tercera posición

// Forma 2: Declarar e inicializar los valores directamente (C# deduce el tamaño automáticamente, en este caso 4)
string[] nombres = { "Javier", "Ana", "Carlos", "Sofia" };

Console.WriteLine($"   Primer nombre (índice 0): {nombres[0]}");
Console.WriteLine($"   Último nombre (índice 3): {nombres[3]}");
Console.WriteLine($"   Tamaño total del array (.Length): {nombres.Length}");

// Acceso rápido en C# moderno al último elemento usando el operador '^'
Console.WriteLine($"   Forma moderna de obtener el último (.NET): {nombres[^1]}");


// ============================================================================
// 2. RECORRER ARRAYS (FOR vs FOREACH)
// ----------------------------------------------------------------------------
// - Usamos 'for' si necesitamos saber o manipular el ÍNDICE (posición).
// - Usamos 'foreach' si solo queremos LEER los valores secuencialmente.
// ----------------------------------------------------------------------------
Console.WriteLine("\n--- 2. RECORRIENDO UN ARRAY ---");

Console.WriteLine("   Con bucle FOR (tenemos acceso al índice 'i'):");
for (int i = 0; i < nombres.Length; i++)
{
    Console.WriteLine($"      [Posición {i}]: {nombres[i]}");
}

Console.WriteLine("\n   Con bucle FOREACH (sintaxis más limpia, solo lectura):");
foreach (string nombre in nombres)
{
    Console.WriteLine($"      -> Hola, {nombre}");
}


// ============================================================================
// 3. MÉTODOS ÚTILES DE LA CLASE 'Array' (Ordenamiento y Búsqueda)
// ----------------------------------------------------------------------------
// C# incluye herramientas nativas para manipular arrays rápidamente.
// ----------------------------------------------------------------------------
Console.WriteLine("\n--- 3. MÉTODOS NATIVOS DE LA CLASE ARRAY ---");

int[] calificaciones = { 85, 92, 60, 74, 100 };

Console.WriteLine($"   Originales: {string.Join(", ", calificaciones)}");

// Ordenar de menor a mayor (In-place: modifica el array original)
Array.Sort(calificaciones);
Console.WriteLine($"   Ordenados (Array.Sort): {string.Join(", ", calificaciones)}");

// Invertir el orden
Array.Reverse(calificaciones);
Console.WriteLine($"   Invertidos (Array.Reverse): {string.Join(", ", calificaciones)}");

// Buscar la posición (índice) de un elemento
int posicion = Array.IndexOf(calificaciones, 85);
Console.WriteLine($"   El número 85 está en el índice: {posicion}");


// ============================================================================
// 4. ARRAYS MULTIDIMENSIONALES / MATRICES (2D)
// ----------------------------------------------------------------------------
// ¿CUÁNDO USARLOS?: Para representar tablas, tableros, coordenadas (Filas x Columnas).
// Sintaxis: tipo[,] nombre = new tipo[filas, columnas];
// ----------------------------------------------------------------------------
Console.WriteLine("\n--- 4. MATRICES (ARRAYS EN 2 DIMENSIONES) ---");

// Matriz de 2 filas por 3 columnas
int[,] matriz2D = {
    { 1, 2, 3 }, // Fila 0
    { 4, 5, 6 }  // Fila 1
};

Console.WriteLine($"   Elemento en Fila 1, Columna 2: {matriz2D[1, 2]}"); // Imprime 6

Console.WriteLine("\n   Recorriendo la matriz completa con bucles anidados:");
for (int fila = 0; fila < matriz2D.GetLength(0); fila++) // GetLength(0) da el número de filas
{
    Console.Write("      Fila " + fila + ": ");
    for (int col = 0; col < matriz2D.GetLength(1); col++) // GetLength(1) da el número de columnas
    {
        Console.Write($"[{matriz2D[fila, col]}] ");
    }
    Console.WriteLine(); // Salto de línea por cada fila
}