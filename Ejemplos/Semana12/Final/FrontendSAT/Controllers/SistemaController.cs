// ===========================================================================
//  ARCHIVO:   Controllers/SistemaController.cs  (FrontendSAT)
//
//  QUE ES:    La "Opcion inicializar" del enunciado.
//
//               GET  /Sistema/Inicializar   pide confirmacion
//               POST /Sistema/Inicializar   llama a la API y regresa al inicio
//
//  OJO:       Despues del POST se hace RedirectToAction (patron PRG:
//             Post-Redirect-Get). Si el usuario presiona F5 en la pagina de
//             inicio, no se vuelve a borrar todo. El mensaje viaja en TempData,
//             que sobrevive exactamente a una redireccion.
// ===========================================================================

using FrontendSAT.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace FrontendSAT.Controllers
{
    public class SistemaController : Controller
    {
        private readonly ClienteApiSAT api;

        public SistemaController(ClienteApiSAT api)
        {
            this.api = api;
        }

        [HttpGet]
        public IActionResult Inicializar()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Inicializar")]
        public async Task<IActionResult> InicializarConfirmado()
        {
            var resultado = await api.InicializarAsync();

            TempData["Aviso"] = resultado.Exito
                ? "El sistema quedo en su estado inicial, sin datos."
                : resultado.Error;
            TempData["AvisoTipo"] = resultado.Exito ? "ok" : "error";

            return RedirectToAction("Index", "Home");
        }
    }
}
