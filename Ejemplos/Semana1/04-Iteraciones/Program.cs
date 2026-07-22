// ============================================================================
// ITERACIONES (BUCLES)
// ============================================================================

Console.WriteLine("=== EXPLICACIÓN PRÁCTICA DE ITERACIONES (BUCLES) EN C# ===\n");

// Array de prueba que utilizaremos en varios ejemplos
string[] lenguajes = { "C#", "Python", "JavaScript", "Java" };


// ============================================================================
// 1. BUCLE FOR (Para un número conocido de repeticiones)
// ----------------------------------------------------------------------------
// ¿CUÁNDO USARLO?: Cuando sabes EXACTAMENTE cuántas veces quieres repetir algo,
// o cuando necesitas controlar explícitamente el índice (posición).
// SINTAXIS: for (inicio; condición; incremento)
// ----------------------------------------------------------------------------
Console.WriteLine("--- 1. BUCLE FOR ---");

// - 'int i = 1': Crea la variable contador inicial.
// - 'i <= 5': Condición. El bucle sigue ejecutándose MIENTRAS esto sea verdadero.
// - 'i++': Es el incremento (equivale a 'i = i + 1'). Se ejecuta al final de cada vuelta.
for (int i = 1; i <= 5; i++)
{
    Console.WriteLine($"   Vuelta número: {i}");
}

// Ejemplo de FOR para recorrer un Array usando su propiedad '.Length'
Console.WriteLine("\n   Recorriendo un array con FOR usando el índice:");
for (int i = 0; i < lenguajes.Length; i++)
{
    // 'lenguajes[i]' accede al elemento en la posición actual
    Console.WriteLine($"   - Índice {i}: {lenguajes[i]}");
}



// ============================================================================
// 2. BUCLE FOREACH (Para recorrer colecciones fácilmente)
// ----------------------------------------------------------------------------
// ¿CUÁNDO USARLO?: Cuando quieres recorrer TODOS los elementos de una lista,
// array o colección de principio a fin, sin preocuparte por los índices.
// SINTAXIS: foreach (tipo elemento in colección)
// ----------------------------------------------------------------------------
Console.WriteLine("\n--- 2. BUCLE FOREACH ---");

// Lee de forma natural: "Para cada 'lenguaje' EN 'lenguajes'..."
// C# extrae cada elemento automáticamente en la variable 'lenguaje'.
// NOTA: 'foreach' es de solo lectura; no debes intentar modificar la colección dentro.
foreach (string lenguaje in lenguajes)
{
    Console.WriteLine($"   Lenguaje: {lenguaje}");
}


// ============================================================================
// 3. BUCLE WHILE (Mientras se cumpla una condición)
// ----------------------------------------------------------------------------
// ¿CUÁNDO USARLO?: Cuando NO sabes cuántas veces se va a repetir el bucle,
// ya que depende de una condición que puede cambiar en cualquier momento.
// NOTA: Si la condición es FALSA desde el inicio, NUNCA se ejecuta.
// ----------------------------------------------------------------------------
Console.WriteLine("\n--- 3. BUCLE WHILE ---");

int contadorWhile = 1;

// Mientras el contador sea menor o igual a 3, sigue ejecutando el bloque
while (contadorWhile <= 3)
{
    Console.WriteLine($"   While iteración: {contadorWhile}");
    
    // ¡IMPORTANTE! Si no incrementamos la variable, creamos un BUCLE INFINITO
    contadorWhile++;
}


// ============================================================================
// 4. BUCLE DO-WHILE (Hacer... mientras)
// ----------------------------------------------------------------------------
// ¿CUÁNDO USARLO?: Cuando necesitas que el código se ejecute AL MENOS UNA VEZ,
// sin importar si la condición es verdadera o falsa de entrada.
// La condición se evalúa AL FINAL de la iteración, no al principio.
// ----------------------------------------------------------------------------
Console.WriteLine("\n--- 4. BUCLE DO-WHILE ---");

int numeroFalso = 10;

do
{
    // Este código SE EJECUTA SIEMPRE al menos 1 vez, aunque 10 no sea menor que 5
    Console.WriteLine($"   Do-While ejecutó esto con numeroFalso = {numeroFalso}");
    numeroFalso++;
} 
while (numeroFalso < 5); // Como 11 < 5 es Falso, aquí termina y no vuelve a repetir.


// ============================================================================
// 5. CONTROL DE BUCLES: BREAK y CONTINUE
// ----------------------------------------------------------------------------
// - 'break': Rompe y DETIENE por completo el bucle.
// - 'continue': SALTA la vuelta actual y pasa directamente a la siguiente.
// ----------------------------------------------------------------------------
Console.WriteLine("\n--- 5. BREAK Y CONTINUE ---");

Console.WriteLine("   Ejemplo de 'continue' (Saltar el número 3):");
for (int i = 1; i <= 5; i++)
{
    if (i == 3)
    {
        // Se salta el resto del código para i=3 y continúa con i=4
        continue; 
    }
    Console.WriteLine($"   Número: {i}");
}

Console.WriteLine("\n   Ejemplo de 'break' (Detenerse al encontrar el número 3):");
for (int i = 1; i <= 5; i++)
{
    if (i == 3)
    {
        Console.WriteLine("   -> ¡Se encontró el 3! Rompiendo el bucle...");
        break; // Cancela el bucle por completo
    }
    Console.WriteLine($"   Número: {i}");
}