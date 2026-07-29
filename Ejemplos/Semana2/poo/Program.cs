



// ============================================================================
// CURSO: Introducción a la Programación y Computación 2
// MÓDULO: Los 4 Pilares de la Programación Orientada a Objetos (POO) en C#
// ENTORNO: .NET 9.0 (Top-Level Statements)
// ============================================================================

Console.WriteLine("=================================================");
Console.WriteLine("=== SISTEMA DE NÓMINA - DEMOSTRACIÓN DE POO ===");
Console.WriteLine("=================================================\n");

// ----------------------------------------------------------------------------
// PILAR 1 Y 3: ABSTRACCIÓN Y HERENCIA
// No podemos hacer: 'Empleado e = new Empleado()' porque es una clase ABSTRACTA.
// Creamos instancias de las clases DERIVADAS (concretas).
// ----------------------------------------------------------------------------


EmpleadoTiempoCompleto desarrolador = new EmpleadoTiempoCompleto(
    id: 101,
    nombre: "Ana Martínez",
    salarioMensual: 8500.00m, // 'm' indica literal decimal para precisión de dinero
    bonoRendimiento: 1200.00m
);

EmpleadoPorHora consultor = new EmpleadoPorHora(
    id: 102,
    nombre: "Carlos Gómez",
    tarifaPorHora: 150.00m,
    horasTrabajadas: 160
);


//#3######################################################################################

// ============================================================================
// DEMOSTRACIÓN DEL USO DE GET Y SET DESDE FUERA DE LA CLASE (USO DE OBJETOS)
// Copia este fragmento justo después de instanciar los objetos en tu Program.cs
// ============================================================================

Console.WriteLine("\n=== DEMOSTRACIÓN DE LECTURA (GET) Y ESCRITURA (SET) ===");

// ----------------------------------------------------------------------------
// A. USO DEL 'SET' (Escribir / Asignar un valor)
// Cada vez que pones la propiedad del lado IZQUIERDO de un signo '=', ejecutas el 'set'.
// ----------------------------------------------------------------------------
Console.WriteLine("\n--- 1. Ejecutando el 'set' (Modificando propiedades) ---");

// Asignación válida: C# ejecuta el 'set' de 'Nombre' y 'BonoRendimiento'
desarrolador.Nombre = "Ana Martínez de Icaza"; // Asigna el nuevo valor
desarrolador.BonoRendimiento = 1500.00m;      // Modifica el bono

// Intentamos una asignación INVÁLIDA para ver la validación del 'set' en acción:
Console.WriteLine("Intentando asignar un salario negativo:");
desarrolador.SalarioMensual = -2000.00m; // El 'set' intercepta el valor y muestra la advertencia.


// ----------------------------------------------------------------------------
// B. USO DEL 'GET' (Leer / Obtener un valor)
// Cada vez que usas la propiedad en un Console.WriteLine, una ecuación o del lado
// DERECHO de un '=', C# ejecuta el 'get' para retornar el valor guardado.
// ----------------------------------------------------------------------------
Console.WriteLine("\n--- 2. Ejecutando el 'get' (Leyendo propiedades) ---");

// Leemos el nombre modificado usando 'get'
string nombreActual = desarrolador.Nombre; // 'get' de Nombre
Console.WriteLine($"   Nombre actualizado (vía get): {nombreActual}");

// Leemos valores en una operación matemática (ejecuta el 'get' de cada propiedad)
decimal ingresoTotalCalculado = desarrolador.SalarioMensual + desarrolador.BonoRendimiento;
Console.WriteLine($"   Suma de Salario + Bono (vía get): ${ingresoTotalCalculado:F2}");


// ----------------------------------------------------------------------------
// C. RESTRICCIONES DE ACCESO (Propiedades Especiales)
// ----------------------------------------------------------------------------
Console.WriteLine("\n--- 3. Intentando violar las reglas de encapsulamiento ---");

// 1. Intentar LEER un campo 'private' o 'protected' directamente desde el objeto:
// Console.WriteLine(desarrolador._nombre);            //  ERROR: '_nombre' es private.
// Console.WriteLine(desarrolador.FechaContratacion); //  ERROR: 'FechaContratacion' es protected.

// 2. Intentar ESCRIBIR en una propiedad de solo lectura o 'init':
// desarrolador.Id = 202; // ERROR: 'Id' usa 'init', no se puede modificar tras crearse.
//########################################################################################

// ----------------------------------------------------------------------------
// PILAR 2: ENCAPSULAMIENTO EN ACCIÓN
// Probamos accesos, modificaciones seguras y reglas de negocio.
// ----------------------------------------------------------------------------
Console.WriteLine("--- 1. PRUEBA DE ENCAPSULAMIENTO ---");

// Intentamos asignar un salario inválido (negativo)
desarrolador.SalarioMensual = -500.00m; // Se activará la validación del 'set'

// Intentamos modificar el ID
// desarrolador.Id = 999; //ERROR DE COMPILACIÓN: 'Id' es 'init' (solo lectura tras construir)

Console.WriteLine();


// ----------------------------------------------------------------------------
// PILAR 4: POLIMORFISMO EN ACCIÓN
// Esta es la parte más potente que debes mostrar a tus alumnos:
// Guardamos objetos de DIFERENTES clases hijas en una misma Lista de tipo PADRE (Empleado).
// C# ejecutará automáticamente el método correcto según el objeto REAL.
// ----------------------------------------------------------------------------
Console.WriteLine("--- 2. PRUEBA DE POLIMORFISMO (LISTA HETEROGÉNEA) ---");

List<Empleado> planilla = new List<Empleado>
{
    desarrolador,
    consultor
};

foreach (Empleado emp in planilla)
{
    // POLIMORFISMO: Aunque 'emp' está tipado como 'Empleado', C# llama al 'CalcularPago()'
    // específico de cada clase hija (Tiempo Completo o Por Hora) gracias a 'override'.
    Console.WriteLine($"Empleado: {emp.Nombre} (ID: {emp.Id})");
    Console.WriteLine($"   Cargo: {emp.ObtenerTipoEmpleado()}");
    Console.WriteLine($"   Pago Total Quincenal/Mensual: ${emp.CalcularPago():F2}");
    Console.WriteLine(new string('-', 45));
}


// ============================================================================
// DEFINICIÓN DE CLASES Y LOS 4 PILARES DE LA POO
// ============================================================================

// ============================================================================
// PILAR 1: ABSTRACCIÓN
// ----------------------------------------------------------------------------
// ¿QUÉ ES?: Es el proceso de simplificar el mundo real modelando ÚNICAMENTE
// los atributos y comportamientos que le interesan a nuestro sistema, ignorando
// detalles irrelevantes (ej. no nos interesa la estatura ni el color de ojos del empleado).
//
// Usamos 'abstract' para indicar que 'Empleado' es un concepto general del cual
// no se pueden crear objetos directos (no existe un "Empleado" a secas, siempre
// es de algún tipo específico).
// ============================================================================

abstract class Empleado
{
    // ========================================================================
    // PILAR 2: ENCAPSULAMIENTO (Campos, Propiedades y Modificadores de Acceso)
    // ------------------------------------------------------------------------
    // ¿QUÉ ES?: Ocultar los datos internos de un objeto y exponer solo puertas
    // de acceso controladas (get/set/métodos) para evitar estados corruptos.
    // ========================================================================

    // ATRIBUTO PRIVADO (private): Solo la clase 'Empleado' puede leerlo/modificarlo directamente.
    private string _nombre;

    // PROPIEDAD CON NITOR DE SEGURIDAD (Full Property):
    public string Nombre
    {
        get { return _nombre; }
        set
        {
            // Regla de negocio / Validación: El nombre no puede ser nulo ni vacío
            if (string.IsNullOrWhiteSpace(value))
            {
                Console.WriteLine("Error de Encapsulamiento: El nombre no puede estar vacío.");
            }
            else
            {
                _nombre = value;
            }
        }
    }

    // AUTO-PROPIEDAD CON INMUTABILIDAD ('init'):
    // El ID se asigna en el constructor y NUNCA más se puede cambiar desde ningún lado.
    public int Id { get; init; }

    // ATRIBUTO PROTEGIDO (protected):
    // Solo es visible para esta clase Y sus clases hijas (Herencia).
    protected DateTime FechaContratacion { get; set; }

    // CONSTRUCTOR PADRE:
    // Garantiza que todo empleado nazca con ID y Nombre válidos.
    public Empleado(int id, string nombre)
    {
        Id = id;
        _nombre = nombre; // Asignación directa al campo respaldado
        FechaContratacion = DateTime.Now;
    }

    // ========================================================================
    // PILAR 4: POLIMORFISMO (Preparación en la Clase Padre)
    // ------------------------------------------------------------------------
    // ¿QUÉ ES?: La capacidad de que objetos de distintas clases respondan al
    // mismo mensaje (método) de formas diferentes.
    //
    // - 'abstract method': La clase padre NO define el código de este método,
    //   OBLIGA a todas las clases hijas a escribir su propia versión.
    // - 'virtual method': La clase padre da una implementación por defecto,
    //   pero las clases hijas PUEDEN sobreescribirla si quieren.
    // ========================================================================

    // Método abstracto (Cada tipo de empleado calcula su pago de forma totalmente distinta)
    public abstract decimal CalcularPago();

    // Método virtual (Implementación base que puede o no ser modificada)
    public virtual string ObtenerTipoEmpleado()
    {
        return "Empleado Genérico";
    }
}


// ============================================================================
// PILAR 3: HERENCIA
// ----------------------------------------------------------------------------
// ¿QUÉ ES?: Permite crear una nueva clase (Hija/Derivada) basada en una clase
// existente (Padre/Base), reutilizando sus atributos y métodos (sintaxis ':').
//
// "EmpleadoTiempoCompleto ES UN Empleado"
// ============================================================================

class EmpleadoTiempoCompleto : Empleado
{
    // Atributos y propiedades propios de esta clase hija
    private decimal _salarioMensual;

    public decimal SalarioMensual
    {
        get { return _salarioMensual; }
        set
        {
            if (value < 0)
            {
                Console.WriteLine("  Error de Encapsulamiento: El salario no puede ser negativo.");
            }
            else
            {
                _salarioMensual = value;
            }
        }
    }

    public decimal BonoRendimiento { get; set; }

    // CONSTRUCTOR DE LA CLASE HIJA:
    // Usa la palabra reservada 'base(...)' para enviar los datos obligatorios al constructor del Padre.
    public EmpleadoTiempoCompleto(int id, string nombre, decimal salarioMensual, decimal bonoRendimiento)
        : base(id, nombre)
    {
        SalarioMensual = salarioMensual; // Pasa por la validación del 'set'
        BonoRendimiento = bonoRendimiento;
    }

    // ========================================================================
    // PILAR 4: POLIMORFISMO (Implementación con 'override')
    // ------------------------------------------------------------------------
    // Usamos 'override' para REDEFINIR el comportamiento del método abstracto/virtual.
    // ========================================================================

    public override decimal CalcularPago()
    {
        // El pago de tiempo completo es su salario base más su bono
        return SalarioMensual + BonoRendimiento;
    }

    public override string ObtenerTipoEmpleado()
    {
        return "Empleado de Tiempo Completo";
    }
}


// ============================================================================
// OTRA CLASE HIJA (Demuestra la reutilización de Herencia y la variación de Polimorfismo)
// ============================================================================

class EmpleadoPorHora : Empleado
{
    public decimal TarifaPorHora { get; set; }
    public int HorasTrabajadas { get; set; }

    public EmpleadoPorHora(int id, string nombre, decimal tarifaPorHora, int horasTrabajadas)
        : base(id, nombre)
    {
        TarifaPorHora = tarifaPorHora;
        HorasTrabajadas = horasTrabajadas;
    }

    // POLIMORFISMO: Este empleado calcula su pago multiplicando Horas x Tarifa
    public override decimal CalcularPago()
    {
        return TarifaPorHora * HorasTrabajadas;
    }

    public override string ObtenerTipoEmpleado()
    {
        return "Contratista por Horas";
    }
}