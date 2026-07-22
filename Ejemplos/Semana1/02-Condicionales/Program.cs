// ============================================================================
// CONDICIONALES
// ============================================================================

Console.WriteLine("=== EXPLICACIÓN PRÁCTICA DE CONDICIONALES EN C# ===\n");

// ============================================================================
// 1. ESTRUCTURA IF - ELSE IF - ELSE (Toma de decisiones básica)
// ----------------------------------------------------------------------------
// ¿CUÁNDO USARLO?: Para evaluar condiciones lógicas complejas, rangos
// o variables booleanas mediante operadores (<, >, ==, !=, &&, ||).
// ----------------------------------------------------------------------------
Console.WriteLine("--- 1. ESTRUCTURA IF / ELSE IF / ELSE ---");

int edad = 18;
bool tieneLicencia = true;

// Evaluamos múltiples condiciones combinadas con el operador lógico AND (&&)
if (edad >= 18 && tieneLicencia)
{
    Console.WriteLine("   -> Puedes conducir legalmente.");
}
else if (edad >= 18 && !tieneLicencia) // '!' invierte el valor (NOT)
{
    Console.WriteLine("   -> Eres mayor de edad, pero no tienes licencia.");
}
else
{
    Console.WriteLine("   -> Eres menor de edad, no puedes conducir.");
}


// ============================================================================
// 2. OPERADOR TERNARIO (Sintaxis corta para IF/ELSE simple)
// ----------------------------------------------------------------------------
// ¿CUÁNDO USARLO?: Cuando solo quieres asignar un valor o ejecutar una línea
// según una condición de Verdadero/Falso.
// SINTAXIS: condición ? valor_si_verdadero : valor_si_falso
// ----------------------------------------------------------------------------
Console.WriteLine("\n--- 2. OPERADOR TERNARIO ---");

int saldo = 150;

// En lugar de usar un bloque 'if-else' de 5 líneas, lo resumimos en una:
string estadoCuenta = (saldo >= 0) ? "Cuenta Activa / Saldo Positivo" : "Cuenta en Números Rojos";

Console.WriteLine($"   Estado: {estadoCuenta}");


// ============================================================================
// 3. SWITCH TRADICIONAL (Evaluación de casos específicos)
// ----------------------------------------------------------------------------
// ¿CUÁNDO USARLO?: Cuando comparas UNA SOLA variable contra múltiples valores
// concretos (como un menú o días de la semana).
// REGLA: Cada 'case' debe terminar con un 'break;' para salir de la estructura.
// ----------------------------------------------------------------------------
Console.WriteLine("\n--- 3. SWITCH TRADICIONAL ---");

int opcionMenu = 2;

switch (opcionMenu)
{
    case 1:
        Console.WriteLine("   -> Opción 1: Ver Perfil del Usuario.");
        break; // Sale del switch
    case 2:
        Console.WriteLine("   -> Opción 2: Configuración del Sistema.");
        break;
    case 3:
        Console.WriteLine("   -> Opción 3: Cerrar Sesión.");
        break;
    default: // Se ejecuta si no coincidió con ningún 'case' previo (como un 'else')
        Console.WriteLine("   -> Opción inválida seleccionada.");
        break;
}


// ============================================================================
// 4. EXPRESIÓN SWITCH / SWITCH EXPRESSION (Sintaxis Moderna de C#)
// ----------------------------------------------------------------------------
// ¿CUÁNDO USARLO?: Introducido en versiones recientes de C#. Es la forma
// limpia y funcional de usar switch para RETORNAR un valor directamente.
// Reemplaza los 'case', 'break' y 'default' por flechas '=>' y '_'.
// ----------------------------------------------------------------------------
Console.WriteLine("\n--- 4. EXPRESIÓN SWITCH (MODERNO) ---");

string codigoRol = "ADM";

// Evaluamos 'codigoRol' y asignamos el resultado devuelto a 'nombreRol'
string nombreRol = codigoRol switch
{
    "ADM" => "Administrador del Sistema",
    "USR" => "Usuario Estándar",
    "GST" => "Invitado (Guest)",
    _     => "Rol Desconocido" // El guion bajo '_' actúa como 'default'
};

Console.WriteLine($"   Rol asignado: {nombreRol}");


// ============================================================================
// 5. MATCHING DE PATRONES Y RANGOS EN SWITCH (C# Avanzado)
// ----------------------------------------------------------------------------
// En C# moderno, 'switch' no solo evalúa valores exactos; también puede
// evaluar rangos numéricos usando 'when' o relacionales (<, >, and, or).
// ----------------------------------------------------------------------------
Console.WriteLine("\n--- 5. SWITCH CON RANGOS Y PATRONES ---");

int notaExamen = 85;

string calificacion = notaExamen switch
{
    >= 90             => "Excelente (A)",
    >= 80 and < 90    => "Muy Bueno (B)",
    >= 70 and < 80    => "Suficiente (C)",
    _                 => "Reprobado (F)"
};

Console.WriteLine($"   Nota: {notaExamen} | Calificación: {calificacion}");