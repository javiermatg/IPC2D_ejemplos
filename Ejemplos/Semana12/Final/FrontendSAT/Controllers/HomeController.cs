// ===========================================================================
//  ARCHIVO:   Controllers/HomeController.cs  (FrontendSAT)
//
//  QUE ES:    La pagina de inicio. Ademas de dar la bienvenida, le pregunta a
//             la API cuantos contribuyentes hay, y con eso se sabe de un
//             vistazo si la API esta encendida.
//
//  OJO:       La accion es async Task<IActionResult> porque adentro hay un
//             await. Si una accion llama a la API, tiene que ser async.
// ===========================================================================

using FrontendSAT.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace FrontendSAT.Controllers
{
    public class HomeController : Controller
    {
        private readonly ClienteApiSAT api;

        public HomeController(ClienteApiSAT api)
        {
            this.api = api;
        }

        public async Task<IActionResult> Index()
        {
            var estado = await api.ObtenerContribuyentesAsync();
            return View(estado);
        }
    }
}
