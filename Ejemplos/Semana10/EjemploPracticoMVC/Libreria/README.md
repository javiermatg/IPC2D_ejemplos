# Librería IPC2 — ejemplo práctico de ASP.NET Core MVC

Ejemplo de clase para **Introducción a la Programación y Computación 2 (USAC)**, Semana 10.
Es una aplicación web pequeña que muestra un catálogo de libros, permite ver el detalle de uno
y agregar otro desde un formulario. Todo el código está comentado.

Es la misma idea de la **Semana 8** (cartelera de cine con Razor Pages), pero repartida en
**Modelo, Vista y Controlador**.

---

## 1. Qué se necesita instalar

**SDK de .NET**, versión 9 o 10. Se descarga de `https://dotnet.microsoft.com/download`.

El proyecto viene configurado para **.NET 9**. Si su SDK es la versión 10, cambie la línea
`<TargetFramework>` de `Libreria.csproj` a `net10.0`. Si al compilar aparece el error
`NETSDK1045`, es exactamente eso: el número de esa línea no coincide con el SDK instalado.

Importante: hay que instalar el **SDK**, no el *Runtime*. El Runtime solo ejecuta programas ya
compilados; el SDK permite compilarlos.

**Editor**, cualquiera de los dos:

- Visual Studio Community, con la carga de trabajo *Desarrollo de ASP.NET y web*.
- Visual Studio Code, con la extensión *C# Dev Kit*.

## 2. Verificar que quedó instalado

```
dotnet --version
```

Si responde con un número de versión, todo está listo. Si dice que el comando no existe, hay que
cerrar la terminal y volver a abrirla.

## 3. Ejecutar este proyecto

```
cd Libreria
dotnet watch
```

`dotnet watch` recompila y recarga el navegador cada vez que se guarda un archivo. Con
`dotnet run` también funciona, pero hay que detener y volver a arrancar en cada cambio.

La dirección queda fija en **`http://localhost:5080`** (está en `Properties/launchSettings.json`).
Se usa `http` y no `https` a propósito, para no depender del certificado de desarrollo. Si lo
quieren con `https`, ejecuten una sola vez `dotnet dev-certs https --trust` y agreguen
`https://localhost:5443` a `applicationUrl`.

## 4. Crear un proyecto como este desde cero

```
dotnet new mvc -n NombreDelProyecto
```

`mvc` es la plantilla de Modelo-Vista-Controlador. Genera la misma estructura que se ve aquí,
más Bootstrap y jQuery en `wwwroot/lib`, que en este ejemplo se borraron a propósito.

---

## Mapa de archivos

Todos los archivos empiezan con un encabezado que explica **qué es**, **por qué existe** y **qué
se hace ahí**. Los comentarios de detalle van línea por línea.

| Archivo | Qué hace | Qué tema demuestra |
|---|---|---|
| `Program.cs` | Enciende el servidor y define la plantilla de URLs | ASP.NET Core, Kestrel, middleware, ruteo (6.1.2) |
| `Models/Libro.cs` | La entidad: un libro | Modelo, propiedades, propiedad calculada, DataAnnotations (6.1.5) |
| `Models/NodoLibro.cs` | Un eslabón de la cadena | Nodo propio, no genérico |
| `Models/Catalogo.cs` | La colección y el acceso a los datos | Lista enlazada propia, `GetEnumerator()` propio, singleton (6.1.5) |
| `Models/RecorridoLibros.cs` | El recorrido que usa `foreach` | `Current` y `MoveNext()` escritos a mano, sin genéricos |
| `Controllers/HomeController.cs` | La página de bienvenida | Valores por omisión del ruteo |
| `Controllers/LibrosController.cs` | Atiende `/Libros` | Acciones, `IActionResult`, `View(objeto)`, `[HttpGet]`/`[HttpPost]`, `ModelState`, `RedirectToAction` (6.1.3, 6.1.7) |
| `Views/_ViewImports.cshtml` | Directivas comunes a todas las vistas | `@using`, tag helpers |
| `Views/_ViewStart.cshtml` | Define el layout por defecto | Plantilla compartida |
| `Views/Shared/_Layout.cshtml` | Esqueleto HTML del sitio | `@RenderBody()`, `ViewData`, `asp-controller` |
| `Views/Home/Index.cshtml` | Bienvenida | Vista sin modelo |
| `Views/Libros/Index.cshtml` | El catálogo en tarjetas | `@model`, `@foreach` sobre el TDA propio, `asp-route-id` (6.1.1, 6.1.8) |
| `Views/Libros/Detalle.cshtml` | Ficha de un libro | Vista tipada de un solo objeto, dato leído de la URL |
| `Views/Libros/Nueva.cshtml` | Formulario de captura | `asp-for`, `asp-validation-summary`, el `name` que arma el objeto |
| `wwwroot/css/estilos.css` | Todos los estilos | Variables, rejilla, modelo de caja |

---

## Las tres direcciones que hay que probar

| URL | Qué pasa por dentro |
|---|---|
| `/` | Sin controlador ni acción en la URL: el ruteo completa con `Home` e `Index` |
| `/Libros` | `LibrosController.Index()` → `Views/Libros/Index.cshtml` |
| `/Libros/Detalle/9788437604947` | El ISBN viaja en `{id?}` y llega al parámetro `id` de la acción |
| `/Libros/Detalle/0000` | No existe: el controlador responde `NotFound()`, es decir 404 |
| `/Libros/Nueva` | Con `GET` muestra el formulario; al enviarlo, el `POST` lo recibe |

---

## Advertencias

- Los datos viven **en memoria**: al detener el servidor se pierden. En el Proyecto 2 el origen
  de los datos será el archivo XML.
- `Catalogo` es un singleton compartido por todos los usuarios. Sirve para un ejemplo de clase,
  no para una aplicación real.
- No se usa `List<T>`, `Stack`, `Queue` **ni arreglos**: la estructura está programada con
  objetos propios, igual que exige el Proyecto 2.
- El nodo no es genérico a propósito. Se llama `NodoLibro` y guarda libros y nada más, para que
  se vea con claridad quién apunta a quién antes de estudiar genéricos.
- La validación que cuenta es la del **servidor** (`ModelState`). La del navegador solo ayuda al
  usuario y se puede quitar desde el inspector.

## Lo que este ejemplo NO trae, y les toca a ustedes

Este ejemplo llega justo hasta donde empieza el Proyecto 2:

- **Leer el XML de entrada**: aquí los libros están escritos a mano en el constructor de
  `Catalogo`. En el proyecto hay que cargarlos desde el archivo.
- **El árbol de categorías**: aquí la estructura es una lista simplemente enlazada, la más
  sencilla que existe.
- **Graphviz**: aquí no se genera ningún reporte gráfico.
- **Editar y eliminar**: solo está listar, ver y agregar.
