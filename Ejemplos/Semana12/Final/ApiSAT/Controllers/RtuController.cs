// ===========================================================================
//  ARCHIVO:   Controllers/RtuController.cs  (ApiSAT)
//
//  QUE ES:    Los endpoints del RTU. Cada metodo publico es una puerta HTTP:
//
//               POST  /api/rtu          actualiza el RTU con un mensaje JSON
//               GET   /api/rtu          devuelve todos los contribuyentes
//               GET   /api/rtu/{nit}    devuelve un contribuyente (o 404)
//
//  POR QUE
//  EXISTE:    Es la traduccion entre HTTP y C#. Recibe la peticion, llama al
//             servicio y escoge el CODIGO DE ESTADO correcto:
//               Ok(...)         -> 200
//               BadRequest(...) -> 400  (el cliente mando algo mal)
//               NotFound(...)   -> 404  (lo que pidio no existe)
//
//  OJO:       ControllerBase (no Controller): una API no tiene vistas.
//             [ApiController] hace dos cosas solas:
//               1) toma el cuerpo de la peticion y lo deserializa al parametro
//               2) si el JSON viene mal escrito, responde 400 sin entrar al metodo
// ===========================================================================

using ApiSAT.Models;
using ApiSAT.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace ApiSAT.Controllers
{
    [ApiController]
    [Route("api/rtu")]
    public class RtuController : ControllerBase
    {
        private readonly ServicioRtu servicio;

        // El servicio llega por inyeccion de dependencias (registrado en Program.cs).
        public RtuController(ServicioRtu servicio)
        {
            this.servicio = servicio;
        }

        // POST /api/rtu
        // Cuerpo esperado (formato del enunciado):
        //   [ {"NIT": "7378106", "nombre": "Empresa ABC, S.A."}, ... ]
        [HttpPost]
        public ActionResult<RespuestaRtu> Actualizar([FromBody] List<Contribuyente> contribuyentes)
        {
            if (contribuyentes == null || contribuyentes.Count == 0)
            {
                return BadRequest(new { error = "El mensaje no trae contribuyentes." });
            }

            RespuestaRtu respuesta = servicio.ProcesarMensaje(contribuyentes);
            return Ok(respuesta);
        }

        // GET /api/rtu
        [HttpGet]
        public ActionResult<List<Contribuyente>> Listar()
        {
            return Ok(servicio.Todos());
        }

        // GET /api/rtu/7378106
        // {nit} en la ruta se copia al parametro nit.
        [HttpGet("{nit}")]
        public ActionResult<Contribuyente> Buscar(string nit)
        {
            Contribuyente encontrado = servicio.Buscar(nit);

            if (encontrado == null)
            {
                return NotFound(new { error = $"El NIT {nit} no existe en el RTU." });
            }

            return Ok(encontrado);
        }
    }
}
