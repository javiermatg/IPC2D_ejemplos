// ===========================================================================
//  ARCHIVO:   Controllers/ContribuyentesController.cs  (FrontendSAT)
//
//  QUE ES:    La consulta del RTU.
//
//               /Contribuyentes              -> GET api/rtu          (todos)
//               /Contribuyentes?nit=7378106  -> GET api/rtu/7378106  (uno)
//
//  OJO:       El buscador usa un formulario con method="get": el NIT viaja en
//             la URL (?nit=...) y llega solo al parametro nit de la accion.
//             Buscar no cambia datos, por eso es GET y no POST.
// ===========================================================================

using FrontendSAT.Servicios;
using FrontendSAT.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FrontendSAT.Controllers
{
    public class ContribuyentesController : Controller
    {
        private readonly ClienteApiSAT api;

        public ContribuyentesController(ClienteApiSAT api)
        {
            this.api = api;
        }

        public async Task<IActionResult> Index(string nit)
        {
            ContribuyentesViewModel modelo = new ContribuyentesViewModel { NitBuscado = nit };

            if (string.IsNullOrWhiteSpace(nit))
            {
                modelo.Lista = await api.ObtenerContribuyentesAsync();
            }
            else
            {
                modelo.Busqueda = await api.BuscarAsync(nit.Trim());
            }

            return View(modelo);
        }
    }
}
