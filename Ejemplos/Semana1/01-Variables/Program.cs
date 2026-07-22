

Console.WriteLine("=== EXPLICACIÓN PRÁCTICA DE VARIABLES Y TIPOS EN C# ===\n");

// ============================================================================
// 1. TIPOS DE DATOS NUMÉRICOS ENTEROS Y FLOTANTES
// ----------------------------------------------------------------------------
// C# es un lenguaje fuertemente tipado: cada variable debe tener un tipo de dato
// claro que define cuánto espacio ocupa en memoria y qué operaciones se permiten.
// ----------------------------------------------------------------------------
Console.WriteLine("--- 1. TIPOS NUMÉRICOS ---");

// 'int': Entero de 32 bits. El más usado para números sin decimales.
int edad = 25;

// 'long': Entero de 64 bits. Se usa para números enteros muy grandes.
// Nota la 'L' al final del valor para indicarle al compilador que es un 'long'.
long poblacionMundial = 8000000000L;

// 'double': Punto flotante de doble precisión (64 bits). Es el tipo con decimales por defecto.
double precioEstandar = 19.99;

// 'decimal': Alta precisión (128 bits). 
// En aplicaciones financieras o financieras siempre se usa 'decimal'.
// Requiere la letra 'm' o 'M' al final del valor. Evita errores de redondeo binario.
decimal precioFinanciero = 19.99m;

// 'float': Precisión simple (32 bits). Ocupa menos memoria. Requiere 'f' al final.
float temperatura = 36.6f;

Console.WriteLine($"   int: {edad}");
Console.WriteLine($"   long: {poblacionMundial}");
Console.WriteLine($"   double: {precioEstandar}");
Console.WriteLine($"   decimal (para dinero): {precioFinanciero}");
Console.WriteLine($"   float: {temperatura}");


// ============================================================================
// 2. TIPOS TEXTUALES Y LÓGICOS
// ----------------------------------------------------------------------------
// Representan texto, caracteres individuales y valores de verdadero/falso.
// ----------------------------------------------------------------------------
Console.WriteLine("\n--- 2. TEXTO Y LÓGICA ---");

// 'string': Cadena de texto (secuencia de caracteres). Se define con comillas DOBLES.
string nombre = "Javier";

// 'char': Un único carácter Unicode (16 bits). Se define con comillas SIMPLES.
char inicial = 'J';

// 'bool': Valor booleano. Solo puede ser 'true' (verdadero) o 'false' (falso).
bool esDesarrollador = true;

Console.WriteLine($"   string: {nombre}");
Console.WriteLine($"   char: {inicial}");
Console.WriteLine($"   bool: {esDesarrollador}");


// ============================================================================
// 3. INFERENCIA DE TIPOS CON 'var'
// ----------------------------------------------------------------------------
// 'var' NO significa que la variable no tenga tipo o que cambie dinámicamente.
// Le ordena al compilador de C#: "Deduce el tipo automáticamente según el valor".
// Una vez asignado el valor inicial, el tipo QUEDA FIJO para siempre.
// ----------------------------------------------------------------------------
Console.WriteLine("\n--- 3. INFERENCIA DE TIPOS (var) ---");

var ciudad = "Guatemala"; // C# detecta automáticamente que es 'string'
var codigoPostal = 01001;  // C# detecta automáticamente que es 'int'

// Intentar hacer esto daría ERROR DE COMPILACIÓN:
// ciudad = 12345; // No se puede asignar un int a una variable declarada como string

Console.WriteLine($"   var (string detectado): {ciudad}");
Console.WriteLine($"   var (int detectado): {codigoPostal}");


// ============================================================================
// 4. CONSTANTES (Valores Inmutables)
// ----------------------------------------------------------------------------
// Se usa la palabra clave 'const'. Debe asignarse su valor en el momento de
// la declaración y NUNCA se podrá cambiar durante la ejecución del programa.
// ----------------------------------------------------------------------------
Console.WriteLine("\n--- 4. CONSTANTES ---");

const double PI = 3.14159;
const string NOMBRE_EMPRESA = "Mi Empresa Tech";

// PI = 3.14; // ERROR: No se puede modificar una constante.

Console.WriteLine($"   Constante PI: {PI}");
Console.WriteLine($"   Constante Empresa: {NOMBRE_EMPRESA}");


// ============================================================================
// 5. TIPOS ANULABLES / NULLABLE TYPES (int?, double?, etc.)
// ----------------------------------------------------------------------------
// Por defecto, los tipos por valor (int, double, bool) NO pueden ser nulos ('null').
// Agregando el símbolo '?' al tipo, le permitimos almacenar 'null' (ausencia de valor).
// Muy útil cuando trabajamos con bases de datos donde un campo puede estar vacío.
// ----------------------------------------------------------------------------
Console.WriteLine("\n--- 5. TIPOS ANULABLES (NULLABLE) ---");

int? edadOpcional = null; // No sabemos la edad aún

if (edadOpcional.HasValue)
{
    Console.WriteLine($"   La edad es: {edadOpcional.Value}");
}
else
{
    Console.WriteLine("   La edad no ha sido registrada (es null).");
}

// Asignamos un valor
edadOpcional = 30;
Console.WriteLine($"   Ahora la edad es: {edadOpcional}");