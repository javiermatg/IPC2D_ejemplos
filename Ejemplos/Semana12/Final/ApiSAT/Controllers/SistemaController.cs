// ===========================================================================
//  ARCHIVO:   Controllers/SistemaController.cs  (ApiSAT)
//
//  QUE ES:    Operaciones sobre el sistema completo, no sobre un recurso.
//
//               POST  /api/sistema/inicializar   deja la API sin datos
//
//  POR QUE
//  EXISTE:    El frontend tiene la "Opcion inicializar" del enunciado, que
//             debe devolver el sistema al estado inicial.
//
//  OJO:       Es POST y no GET a proposito: un GET nunca deberia cambiar
//             datos. Un navegador o un buscador pueden repetir un GET por su
//             cuenta; nadie quiere borrar la base por abrir un enlace.
// ===========================================================================

using ApiSAT.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace ApiSAT.Controllers
{
    [ApiController]
    [Route("api/sistema")]
    public class SistemaController : ControllerBase
    {
        private readonly ServicioRtu servicioRtu;

        public SistemaController(ServicioRtu servicioRtu)
        {
            this.servicioRtu = servicioRtu;
        }

        [HttpPost("inicializar")]
        public IActionResult Inicializar()
        {
            servicioRtu.Inicializar();
            // Cuando agreguen las autorizaciones, tambien se limpian aqui.
            return Ok(new { mensaje = "Sistema inicializado. No quedan datos." });
        }
    }
}
