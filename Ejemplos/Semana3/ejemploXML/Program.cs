// ============================================================================

// MÓDULO: Formato XML y Lectura de Archivos XML en C# (.NET 9)
// HERRAMIENTA: LINQ to XML (XDocument / XElement)
// ============================================================================

using System.Xml.Linq; //  LIBRERÍA OBLIGATORIA para trabajar con LINQ to XML

Console.WriteLine("=================================================");
Console.WriteLine("=== LECTURA Y PROCESAMIENTO DE ARCHIVOS XML ===");
Console.WriteLine("=================================================\n");

// ============================================================================
// 1. SIMULACIÓN DEL ARCHIVO XML (Cadena con estructura XML válida)
// ----------------------------------------------------------------------------
//
// 1. Elemento Raíz: Solo puede haber UNO (<Empresa>).
// 2. Etiquetas/Tags de Apertura y Cierre: <Empleado> ... </Empleado>
// 3. Atributos: Datos dentro del tag (ej. id="101").
// 4. Elementos Hijos: Contenido anidado (<Nombre>, <Puesto>, <Salario>).
// ============================================================================

string contenidoXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Empresa nombre=""TechSolutions Inc."">
    <Empleados>
        <Empleado id=""101"" departamento=""Sistemas"">
            <Nombre>Ana Martínez</Nombre>
            <Puesto>Desarrolladora Senior</Puesto>
            <Salario>8500.00</Salario>
        </Empleado>
        <Empleado id=""102"" departamento=""Sistemas"">
            <Nombre>Carlos Gómez</Nombre>
            <Puesto>Analista de Datos</Puesto>
            <Salario>6200.50</Salario>
        </Empleado>
        <Empleado id=""103"" departamento=""Recursos Humanos"">
            <Nombre>Lucía Pérez</Nombre>
            <Puesto>Gerente de Talento</Puesto>
            <Salario>9000.00</Salario>
        </Empleado>
    </Empleados>
</Empresa>";


// ============================================================================
// 2. PARSEO / LECTURA DEL XML CON XDocument
// ----------------------------------------------------------------------------
// - Usamos 'XDocument.Parse(...)' para leer desde un string.
// - En un entorno real con archivo físico usarías: 'XDocument.Load("datos.xml")'
// ============================================================================

XDocument docXml = XDocument.Parse(contenidoXml);

// A. Leer datos del Elemento Raíz y sus Atributos
XElement nodoRaiz = docXml.Root;
string nombreEmpresa = nodoRaiz.Attribute("nombre")?.Value;

Console.WriteLine($"Empresa Procesada: {nombreEmpresa}");
Console.WriteLine(new string('=', 50));


// ============================================================================
// 3. RECORRIDO DE ELEMENTOS XML CON BUCLE FOREACH
// ----------------------------------------------------------------------------
// Obtendremos la colección de todos los tags <Empleado> usando:
// 'docXml.Descendants("Empleado")' -> Busca todos los nodos <Empleado> en cualquier nivel.
// ============================================================================

Console.WriteLine("\n--- LISTADO GENERAL DE EMPLEADOS (Recorrido Tradicional) ---");

foreach (XElement nodoEmpleado in docXml.Descendants("Empleado"))
{
    // Extracción de Atributos (id, departamento)
    string id = nodoEmpleado.Attribute("id")?.Value;
    string depto = nodoEmpleado.Attribute("departamento")?.Value;

    // Extracción de Elementos Hijos (<Nombre>, <Puesto>, <Salario>)
    string nombre = nodoEmpleado.Element("Nombre")?.Value;
    string puesto = nodoEmpleado.Element("Puesto")?.Value;
    
    // Parseo seguro del Salario desde texto a tipo 'decimal'
    decimal salario = decimal.Parse(nodoEmpleado.Element("Salario")?.Value ?? "0");

    Console.WriteLine($"   ID: {id} | Nombre: {nombre}");
    Console.WriteLine($"   Puesto: {puesto} ({depto})");
    Console.WriteLine($"   Salario: ${salario:F2}\n");
}


// ============================================================================
// 4. BÚSQUEDA Y FILTRADO AVANZADO CON LINQ
// ----------------------------------------------------------------------------
// el poder de LINQ para consultar XML como si fuera
// una Base de Datos en SQL.
//
// EJEMPLO: Obtener solo empleados del departamento de "Sistemas".
// ============================================================================

Console.WriteLine("--- FILTRADO CON LINQ: Empleados de 'Sistemas' ---");

var empleadosSistemas = from emp in docXml.Descendants("Empleado")
                        where emp.Attribute("departamento")?.Value == "Sistemas"
                        select new
                        {
                            Id = emp.Attribute("id")?.Value,
                            Nombre = emp.Element("Nombre")?.Value,
                            Salario = decimal.Parse(emp.Element("Salario")?.Value ?? "0")
                        };

foreach (var emp in empleadosSistemas)
{
    Console.WriteLine($"   [{emp.Id}] {emp.Nombre} - Salario: ${emp.Salario:F2}");
}


// ============================================================================
// 5. MAPEANDO XML A OBJETOS DE POO (Conexión con la clase anterior)
// ----------------------------------------------------------------------------
// Esto es vital: Los datos leídos de XML se transforman en Objetos C# (POO).
// ============================================================================

Console.WriteLine("\n--- MAPEANDO A OBJETOS C# ---");

List<EmpleadoDTO> listaObjetos = docXml.Descendants("Empleado")
    .Select(emp => new EmpleadoDTO
    {
        Id = int.Parse(emp.Attribute("id")?.Value ?? "0"),
        Nombre = emp.Element("Nombre")?.Value,
        Puesto = emp.Element("Puesto")?.Value,
        Salario = decimal.Parse(emp.Element("Salario")?.Value ?? "0")
    })
    .ToList();

Console.WriteLine($" Se cargaron exitosamente {listaObjetos.Count} objetos 'EmpleadoDTO' en memoria.");


// 2. Impresión: Recorremos la lista de OBJETOS (ya no leemos el XML)
foreach (EmpleadoDTO empObj in listaObjetos)
{
    Console.WriteLine($"[Objeto EmpleadoDTO] ID: {empObj.Id} | Nombre: {empObj.Nombre} | Puesto: {empObj.Puesto} | Salario: ${empObj.Salario:F2}");
}

Console.WriteLine($"\n Verificación exitosa: Se cargaron e imprimieron {listaObjetos.Count} objetos desde la lista.");

// ============================================================================
// CLASE DTO (Data Transfer Object)
// ============================================================================

class EmpleadoDTO
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Puesto { get; set; }
    public decimal Salario { get; set; }
}