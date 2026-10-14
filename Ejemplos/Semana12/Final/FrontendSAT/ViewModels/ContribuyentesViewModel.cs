// ===========================================================================
//  ARCHIVO:   ViewModels/ContribuyentesViewModel.cs  (FrontendSAT)
//
//  QUE ES:    Lo que necesita la pantalla del RTU: la tabla completa o el
//             resultado de buscar un NIT.
//
//  OJO:       Se guarda el ResultadoApi completo (no solo la lista) para que la
//             vista pueda ensenar el codigo HTTP y el JSON crudo de la llamada.
// ===========================================================================

using FrontendSAT.Models;

namespace FrontendSAT.ViewModels
{
    public class ContribuyentesViewModel
    {
        // Texto que el usuario escribio en el buscador (vacio = ver todos).
        public string NitBuscado { get; set; }

        // Se llena cuando se piden todos:  GET api/rtu
        public ResultadoApi<List<Contribuyente>> Lista { get; set; }

        // Se llena cuando se busca uno:    GET api/rtu/{nit}
        public ResultadoApi<Contribuyente> Busqueda { get; set; }
    }
}
