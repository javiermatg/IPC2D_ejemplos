# Librería IPC2 — ejemplo práctico de Web Development (Semana 11)

Ejemplo de clase para **Introducción a la Programación y Computación 2 (USAC)**, Semana 11,
Unidad 7: *Web Development*. Es la misma aplicación MVC de la **Semana 10** (catálogo de libros,
detalle y formulario), ahora con:

- **JavaScript en el navegador**: filtro en vivo del catálogo y validación de cortesía del formulario.
- **Layouts y herencia de templates**: secciones (`@section Scripts`) y un layout anidado.
- **Vistas parciales**: la tarjeta de cada libro se dibuja con `_TarjetaLibro.cshtml`.
- **Archivos estáticos**: CSS, JavaScript e imágenes en `wwwroot`, con `~/` y `asp-append-version`.
- **Inlines de Razor**: expresiones implícitas y explícitas, `@:` y `<text>`.
- **ViewModels**: el listado recibe un `CatalogoViewModel` en lugar del `Catalogo` suelto.

Todo el código está comentado.

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
se hace ahí**. Los marcados con **(S11)** son nuevos o cambiaron esta semana.

| Archivo | Qué hace | Qué tema demuestra |
|---|---|---|
| `Program.cs` | Enciende el servidor y define la plantilla de URLs | `UseStaticFiles`: quién entrega `wwwroot` (7.4.3) |
| `Models/Libro.cs` | La entidad: un libro | Modelo con DataAnnotations |
| `Models/NodoLibro.cs` | Un eslabón de la cadena | Nodo propio, no genérico |
| `Models/Catalogo.cs` **(S11)** | La colección | Lista enlazada propia; nuevos `Filtrar(texto)` y `CantidadDisponibles` |
| `ViewModels/CatalogoViewModel.cs` **(S11)** | Todo lo que necesita el listado | ViewModel (7.4.5) |
| `Controllers/LibrosController.cs` **(S11)** | Atiende `/Libros` | `Index(string texto)` arma el ViewModel |
| `Views/_ViewImports.cshtml` **(S11)** | Directivas comunes | `@using Libreria.ViewModels` |
| `Views/Shared/_Layout.cshtml` **(S11)** | Esqueleto del sitio | `@RenderBody`, `RenderSectionAsync`, `~/`, `asp-append-version`, `defer` (7.4.1, 7.4.3) |
| `Views/Shared/_LayoutFormulario.cshtml` **(S11)** | Layout de los formularios | Layout anidado y reenvío de secciones (7.4.2) |
| `Views/Shared/_TarjetaLibro.cshtml` **(S11)** | La tarjeta de un libro | Vista parcial e inlines de Razor (7.4.2, 7.4.4) |
| `Views/Libros/Index.cshtml` **(S11)** | El catálogo | `@model CatalogoViewModel`, `<partial>`, `@section Scripts`, búsqueda en servidor y en cliente |
| `Views/Libros/Detalle.cshtml` | Ficha de un libro | Vista tipada de un solo objeto |
| `Views/Libros/Nueva.cshtml` **(S11)** | Formulario de captura | `Layout = "_LayoutFormulario"`, `@section Scripts` |
| `wwwroot/css/estilos.css` **(S11)** | Todos los estilos | Archivo estático |
| `wwwroot/js/sitio.js` **(S11)** | JavaScript común | DOM: `querySelectorAll`, `classList` (7.2, 7.3) |
| `wwwroot/js/catalogo.js` **(S11)** | Filtro en vivo del catálogo | Eventos `input`, `dataset`, template strings (7.3) |
| `wwwroot/js/formulario.js` **(S11)** | Validación de cortesía | Evento `submit`, `preventDefault`, crear elementos (7.3) |
| `wwwroot/img/logo.svg` **(S11)** | Logo del sitio | Imagen estática (7.4.3) |

---

## Las direcciones que hay que probar

| URL | Qué pasa por dentro |
|---|---|
| `/Libros` | `Index(null)`: el ViewModel trae todos los libros y los totales |
| `/Libros?texto=asturias` | Búsqueda en el **servidor**: `Catalogo.Filtrar("asturias")` |
| `/Libros` + escribir en *Filtrar mientras escribe* | Búsqueda en el **cliente**: `catalogo.js` esconde tarjetas, sin petición nueva |
| `/Libros/Nueva` | Layout anidado; `formulario.js` avisa antes de enviar |
| `/js/catalogo.js`, `/css/estilos.css`, `/img/logo.svg` | Archivos estáticos: los entrega `UseStaticFiles`, sin controlador |

Para ver el JavaScript en acción: **F12 > Consola** (mensaje de `sitio.js`) y **F12 > Red**
(cada archivo estático debe responder 200).

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
- La validación que cuenta es la del **servidor** (`ModelState`). `formulario.js` solo ayuda al
  usuario: para comprobarlo, comente la `@section Scripts` de `Nueva.cshtml` y envíe el formulario
  vacío; los errores vuelven igual, ahora desde el servidor.
- El filtro en vivo solo ve los libros que ya llegaron en el HTML. Con miles de libros conviene el
  buscador del servidor.

## Lo que este ejemplo NO trae, y les toca a ustedes

Este ejemplo llega justo hasta donde empieza el Proyecto 2:

- **Leer el XML de entrada**: aquí los libros están escritos a mano en el constructor de
  `Catalogo`. En el proyecto hay que cargarlos desde el archivo.
- **El árbol de categorías**: aquí la estructura es una lista simplemente enlazada, la más
  sencilla que existe.
- **Graphviz**: aquí no se genera ningún reporte gráfico.
- **Editar y eliminar**: solo está listar, ver y agregar.
