// ===========================================================================
//  ARCHIVO:   Controllers/MensajesController.cs  (FrontendSAT)
//
//  QUE ES:    El componente "Enviar mensaje" del enunciado.
//
//               GET  /Mensajes/Enviar   muestra el formulario para elegir el .json
//               POST /Mensajes/Enviar   lee el archivo y se lo manda a la API
//
//  POR QUE
//  EXISTE:    El usuario sube un archivo al FRONTEND; el frontend lee su texto
//             y lo reenvia a la API con HttpClient. Son dos viajes HTTP:
//               1) navegador -> frontend   (multipart/form-data, el archivo)
//               2) frontend  -> API        (application/json, el contenido)
//
//  OJO:       IFormFile es como ASP.NET Core entrega un archivo subido. El
//             nombre del parametro (archivo) debe ser igual al name del
//             <input type="file"> en la vista, o llega null.
// ===========================================================================

using FrontendSAT.Servicios;
using FrontendSAT.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FrontendSAT.Controllers
{
    public class MensajesController : Controller
    {
        private readonly ClienteApiSAT api;

        public MensajesController(ClienteApiSAT api)
        {
            this.api = api;
        }

        [HttpGet]
        public IActionResult Enviar()
        {
            return View(new EnvioViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Enviar(IFormFile archivo)
        {
            EnvioViewModel modelo = new EnvioViewModel();

            // 1) Validar el archivo ANTES de llamar a la API.
            if (archivo == null || archivo.Length == 0)
            {
                modelo.ErrorArchivo = "Seleccione un archivo antes de enviar.";
                return View(modelo);
            }

            if (!archivo.FileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            {
                modelo.ErrorArchivo = "Solo se aceptan archivos con extension .json.";
                return View(modelo);
            }

            // 2) Leer el contenido del archivo como texto.
            using (StreamReader lector = new StreamReader(archivo.OpenReadStream()))
            {
                modelo.JsonEnviado = await lector.ReadToEndAsync();
            }
            modelo.NombreArchivo = archivo.FileName;

            // 3) Mandarlo a la API y guardar lo que conteste.
            modelo.Resultado = await api.EnviarRtuAsync(modelo.JsonEnviado);

            return View(modelo);
        }
    }
}
