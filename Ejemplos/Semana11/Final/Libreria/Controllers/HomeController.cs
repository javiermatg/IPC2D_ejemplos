using Microsoft.AspNetCore.Mvc;

namespace Libreria.Controllers;

// ===========================================================================
//  ARCHIVO:   Controllers/HomeController.cs
//
//  QUE ES:    El controlador de la pagina de bienvenida.
//
//  POR QUE
//  EXISTE:    Para demostrar los valores por omision de la plantilla de
//             ruteo. En Program.cs esta escrito
//             {controller=Home}/{action=Index}/{id?}, asi que cuando alguien
//             pide la direccion raiz  /  sin decir nada mas, ASP.NET Core
//             completa los huecos y termina llamando a este metodo.
//
//  QUE SE
//  HACE AQUI: Nada mas que devolver una vista. Es el controlador mas simple
//             que puede existir y sirve para comparar: si este responde en
//             tres direcciones distintas (/, /Home y /Home/Index), es porque
//             el ruteo es por convencion.
// ===========================================================================
public class HomeController : Controller
{
    // Responde a:   /      /Home      /Home/Index
    public IActionResult Index()
    {
        // View() sin argumentos busca, por convencion, el archivo
        // Views/Home/Index.cshtml: la carpeta se llama como el controlador
        // (sin el sufijo Controller) y el archivo como la accion.
        return View();
    }
}
