# Cartelera de cine — ejemplo práctico de HTML5, CSS y Razor

Ejemplo de clase para **Introducción a la Programación y Computación 2 (USAC)**.
Es una aplicación web pequeña que muestra una cartelera de películas y permite agregar
una nueva desde un formulario. Todo el código está comentado línea por línea.

---

## 1. Qué se necesita instalar

**SDK de .NET**, versión 9 o 10. Se descarga de `https://dotnet.microsoft.com/download`.

El proyecto viene configurado para **.NET 9**. Si su SDK es la versión 10, cambie la
línea `<TargetFramework>` de `CarteleraCine.csproj` a `net10.0`. Si al compilar aparece
el error `NETSDK1045`, es exactamente eso: el número de esa línea no coincide con el
SDK instalado.

Importante: hay que instalar el **SDK**, no el *Runtime*. El Runtime solo ejecuta
programas ya compilados; el SDK permite compilarlos.

**Editor**, cualquiera de los dos:

- Visual Studio Community, con la carga de trabajo *Desarrollo de ASP.NET y web*.
- Visual Studio Code, con la extensión *C# Dev Kit*.

## 2. Verificar que quedó instalado

```
dotnet --version
```

Si responde con un número de versión, todo está listo. Si dice que el comando no existe,
hay que cerrar la terminal y volver a abrirla.

## 3. Confiar en el certificado de desarrollo

Se hace una sola vez por computadora. Sin esto el navegador muestra una advertencia roja:

```
dotnet dev-certs https --trust
```

## 4. Ejecutar este proyecto

```
cd CarteleraCine
dotnet watch
```

`dotnet watch` recompila y recarga el navegador cada vez que se guarda un archivo.
Con `dotnet run` también funciona, pero hay que detener y volver a arrancar en cada cambio.

La dirección aparece en la terminal, normalmente `https://localhost:7xxx`.

## 5. Crear un proyecto como este desde cero

```
dotnet new webapp -n NombreDelProyecto
```

`webapp` es la plantilla de Razor Pages. Genera la misma estructura que se ve aquí.

---

## Mapa de archivos

Todos los archivos empiezan con un encabezado que explica **qué es**, **por qué existe**
y **qué se hace ahí**. Los comentarios de detalle van línea por línea.

| Archivo | Qué hace | Qué tema demuestra |
|---|---|---|
| `Program.cs` | Enciende el servidor y conecta las páginas | ASP.NET Core, Kestrel |
| `Modelos/Pelicula.cs` | La entidad: una película | Clase, propiedades, propiedad calculada |
| `Modelos/NodoPelicula.cs` | Un eslabón de la cadena | Nodo propio, no genérico |
| `Modelos/ListaPeliculas.cs` | La lista enlazada: el TDA | Estructura propia en vez de `List<T>` |
| `Modelos/RecorridoPeliculas.cs` | Sabe caminar la lista | Enumerador propio, hace posible `@foreach` |
| `Modelos/Cartelera.cs` | Administra la cartelera | Singleton, encapsulamiento, delegación |
| `Pages/_ViewImports.cshtml` | Directivas comunes a todas las páginas | `@using`, tag helpers |
| `Pages/_ViewStart.cshtml` | Define el layout por defecto | Plantilla compartida |
| `Pages/Shared/_Layout.cshtml` | Esqueleto HTML del sitio | Estructura básica, `head`, semántica, `@RenderBody()` |
| `Pages/Index.cshtml` | Cartelera con tarjetas | `@if`, `@foreach`, imágenes, rejilla CSS |
| `Pages/Detalle.cshtml` | Ficha de una película | Dato leído de la URL, tabla, caso "no existe" |
| `Pages/Nueva.cshtml` | Formulario de captura | `form`, `label`, tipos y atributos de `input`, `select`, `textarea` |
| `Pages/Nueva.cshtml.cs` | Recibe el POST y valida | `[BindProperty]`, `OnPost`, validación en servidor |
| `wwwroot/css/estilos.css` | Todos los estilos | Variables, selectores, modelo de caja, pseudo-clases |
| `wwwroot/img/` | Afiches de ejemplo | Rutas relativas y `alt` |

---

## Orden sugerido para presentarlo en vivo

1. **`dotnet watch`** y abrir el sitio. Mostrar clic derecho → *Ver código fuente*:
   no aparece ni una línea de C#. El navegador solo recibió HTML.
2. **`_Layout.cshtml`**: el esqueleto y las etiquetas semánticas. Cambiar el texto del
   `footer` y mostrar que cambia en todas las páginas a la vez.
3. **`estilos.css`**: cambiar `--acento` a otro color y guardar. Todo el sitio cambia
   con una sola línea: para eso sirven las variables.
4. **`Index.cshtml`**: el `@foreach`. Agregar una película en el constructor de
   `Cartelera.cs` y ver aparecer una tarjeta nueva sin tocar el HTML.
   Luego abrir `ListaPeliculas.cs` y mostrar que ese `@foreach` funciona sobre una
   estructura hecha a mano: basta con el método `GetEnumerator()`.
5. **`Nueva.cshtml`**: llenar el formulario y guardar. Luego, la parte importante:
   borrar el `name` de un campo, volver a enviar y mostrar que ese dato llega vacío.
6. **Validación**: quitar `required` desde el inspector del navegador y enviar vacío.
   El servidor lo rechaza igual, porque `OnPost` vuelve a revisar.
7. **Las dos formas de recorrer**: comparar el `@foreach` de `Index.cshtml` con el
   `while` nodo por nodo de la tabla de `Nueva.cshtml`. Por dentro son lo mismo.

## Advertencias

- Los datos viven en memoria: al detener el servidor se pierden. En el Proyecto 2 el
  origen de los datos será el archivo XML.
- `Cartelera` es estática y compartida por todos los usuarios. Sirve para un ejemplo de
  clase, no para una aplicación real.
- No se usa `List<T>`, `Stack`, `Queue` **ni arreglos**: la estructura está programada
  con objetos propios, igual que exige el Proyecto 2.
- El nodo no es genérico a propósito. Se llama `NodoPelicula` y guarda películas y nada
  más, para que se vea con claridad quién apunta a quién antes de estudiar genéricos.
