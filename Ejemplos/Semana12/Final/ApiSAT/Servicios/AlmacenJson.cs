// ===========================================================================
//  ARCHIVO:   Servicios/AlmacenJson.cs  (ApiSAT)
//
//  QUE ES:    La capa de datos: guarda y lee objetos de C# en archivos .json
//             dentro de la carpeta  ApiSAT/Datos.
//
//  POR QUE
//  EXISTE:    El enunciado pide que la informacion se almacene en archivos.
//             Si la API se apaga, el RTU en memoria se pierde; el archivo no.
//             Al encender, se vuelve a leer y todo sigue como estaba.
//
//  QUE SE
//  HACE AQUI: Solo leer y escribir archivos. No sabe nada de NITs ni de la SAT:
//             es generico (<T>), asi que sirve igual para guardar el RTU, las
//             autorizaciones o lo que haga falta despues.
//
//  OJO:       Los archivos quedan en la carpeta del proyecto (ContentRootPath),
//             no en bin/Debug. Asi se pueden abrir en el editor durante la clase.
// ===========================================================================

using System.Text.Json;

namespace ApiSAT.Servicios
{
    public class AlmacenJson
    {
        private readonly string carpeta;

        private static readonly JsonSerializerOptions Opciones = new JsonSerializerOptions
        {
            WriteIndented = true    // el archivo queda legible, con saltos de linea y sangria
        };

        // ASP.NET Core entrega IWebHostEnvironment por inyeccion de dependencias.
        public AlmacenJson(IWebHostEnvironment entorno)
        {
            carpeta = Path.Combine(entorno.ContentRootPath, "Datos");
            Directory.CreateDirectory(carpeta);   // si ya existe, no hace nada
        }

        // Objeto de C#  ->  texto JSON  ->  archivo
        public void Guardar<T>(string archivo, T datos)
        {
            string json = JsonSerializer.Serialize(datos, Opciones);
            File.WriteAllText(Path.Combine(carpeta, archivo), json);
        }

        // archivo  ->  texto JSON  ->  objeto de C#
        // Si el archivo no existe, devuelve el valor por defecto (null para clases).
        public T Leer<T>(string archivo)
        {
            string ruta = Path.Combine(carpeta, archivo);

            if (!File.Exists(ruta))
            {
                return default;
            }

            string json = File.ReadAllText(ruta);
            return JsonSerializer.Deserialize<T>(json);
        }

        public void Borrar(string archivo)
        {
            string ruta = Path.Combine(carpeta, archivo);

            if (File.Exists(ruta))
            {
                File.Delete(ruta);
            }
        }
    }
}
