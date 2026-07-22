// ============================================================================
// FUNCIONES
// ============================================================================

// ----------------------------------------------------------------------------
// SECCIÓN 1: EJECUCIÓN DEL CÓDIGO
// Gracias a los "Top-Level Statements" de .NET, no necesitamos envolver
// todo este código dentro de 'class Program' o 'static void Main()'.
// El código se ejecuta de arriba hacia abajo.
// ----------------------------------------------------------------------------

Console.WriteLine("=== EXPLICACIÓN PRÁCTICA DE FUNCIONES EN C# ===\n");

// --- Pruebas de las funciones ---

int resultadoSuma = Sumar(10, 5);
Console.WriteLine($"1. Resultado de la suma: {resultadoSuma}");

Saludar();

int resultadoMultiplicacion = Multiplicar(4, 3);
Console.WriteLine($"3. Resultado de la multiplicación: {resultadoMultiplicacion}");

Console.WriteLine("4. Parámetros opcionales:");
MostrarMensaje("¡Hola a todos!");           // No le pasamos el segundo dato, usa "Invitado"
MostrarMensaje("¡Hola!", "Ana Maria");     // Le pasamos el segundo dato, reemplaza "Invitado"

// Desestructuración de Tupla: extraemos 'nombre' y 'edad' directamente en variables
var (nombre, edad) = ObtenerPerfil();
Console.WriteLine($"5. Tupla -> Nombre: {nombre}, Edad: {edad}");


// ============================================================================
// SECCIÓN 2: DEFINICIÓN DE FUNCIONES (Funciones Locales / Local Functions)
// ----------------------------------------------------------------------------

// ----------------------------------------------------------------------------
// 1. FUNCIÓN TRADICIONAL CON RETORNO Y PARÁMETROS
// - 'int' al inicio: Indica que esta función OBLIGATORIAMENTE debe devolver
//   un número entero (int) mediante la palabra clave 'return'.
// - '(int a, int b)': Son los parámetros. Indican qué tipo de datos necesita
//   recibir la función para poder hacer su trabajo.
// ----------------------------------------------------------------------------
int Sumar(int a, int b)
{
    return a + b; // Devuelve la suma de los dos enteros
}

// ----------------------------------------------------------------------------
// 2. FUNCIÓN SIN RETORNO (VOID) Y SIN PARÁMETROS
// - 'void': Significa "vacío". Le indica a C# que esta función ejecuta una
//   acción (imprimir en consola) pero NO devuelve ningún valor que podamos
//   guardar en una variable.
// - '()': Los paréntesis vacíos indican que no necesita datos para ejecutarse.
// ----------------------------------------------------------------------------
void Saludar()
{
    Console.WriteLine("2. ¡Hola! Soy una función void (no retorno nada).");
}

// ----------------------------------------------------------------------------
// 3. SINTAXIS COMPACTA (Expression-bodied member)
// - Es exactamente igual a una función normal, pero usa la flecha '=>' (lambda).
// - Se usa cuando la función realiza su tarea en UNA SOLA LÍNEA.
// - Reemplaza tanto las llaves {} como la palabra 'return'.
// ----------------------------------------------------------------------------
int Multiplicar(int x, int y) => x * y;

// ----------------------------------------------------------------------------
// 4. PARÁMETROS OPCIONALES Y CON VALORES POR DEFECTO
// - 'string usuario = "Invitado"': Al asignarle un valor dentro de los paréntesis,
//   hacemos que 'usuario' sea un parámetro OPCIONAL.
// - Si quien llama a la función no manda este argumento, C# usará "Invitado".
// - Si lo manda, C# usará el valor que le enviaron.
// ----------------------------------------------------------------------------
void MostrarMensaje(string mensaje, string usuario = "Invitado")
{
    Console.WriteLine($"   [{usuario}]: {mensaje}");
}

// ----------------------------------------------------------------------------
// 5. DEVOLVER MÚLTIPLES VALORES (TUPLAS)
// - Tradicionalmente una función solo devuelve 1 cosa. Si necesitas devolver 2 o más,
//   C# permite usar Tuplas declarando los tipos entre paréntesis: '(string, int)'.
// - Esto evita tener que crear clases complejas solo para mover dos datos juntos.
// ----------------------------------------------------------------------------
(string Nombre, int Edad) ObtenerPerfil()
{
    return ("Carlos", 28); // Retorna ambos valores emparejados
}