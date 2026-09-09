using CarteleraCine.Modelos;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarteleraCine.Pages;

// ===========================================================================
//  ARCHIVO:   Pages/Index.cshtml.cs
//
//  QUE ES:    El "cerebro" de la pagina Index.cshtml. En Razor Pages cada
//             pagina son DOS archivos: la plantilla (.cshtml) y esta clase
//             (.cshtml.cs), que en ingles se llama PageModel.
//
//  POR QUE
//  EXISTE:    Para separar el pensar del mostrar. Todo lo que sea buscar,
//             calcular o decidir ocurre aqui; la plantilla solo dibuja lo que
//             este archivo le dejo listo.
//
//  QUE SE
//  HACE AQUI: Preparar los datos ANTES de que la pagina se dibuje y dejarlos
//             en propiedades publicas. La plantilla las lee con @Model.
// ===========================================================================
public class IndexModel : PageModel        // toda pagina hereda de PageModel: de ahi saca Request, Response, etc.
{
    // Propiedad publica = lo que la plantilla puede leer como @Model.Peliculas.
    // Es del tipo ListaPeliculas, el TDA propio: no hay ningun arreglo ni List.
    public ListaPeliculas Peliculas { get; set; }

    // OnGet se ejecuta cuando el navegador PIDE la pagina (peticion GET).
    // El nombre no es casualidad: ASP.NET busca un metodo llamado asi.
    public void OnGet()
    {
        Peliculas = Cartelera.Instancia.Peliculas;   // se pide la lista al unico objeto Cartelera
    }
}
