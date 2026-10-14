// ===========================================================================
//  ARCHIVO:   ViewModels/EnvioViewModel.cs  (FrontendSAT)
//
//  QUE ES:    Lo que necesita la pantalla "Enviar mensaje" para dibujarse:
//             el archivo que se mando, su contenido y lo que contesto la API.
//
//  OJO:       Resultado es null la primera vez que se abre la pagina (GET):
//             todavia no se ha enviado nada. La vista lo revisa antes de usarlo.
// ===========================================================================

using FrontendSAT.Models;

namespace FrontendSAT.ViewModels
{
    public class EnvioViewModel
    {
        public string NombreArchivo { get; set; }

        public string JsonEnviado { get; set; }

        public string ErrorArchivo { get; set; }

        public ResultadoApi<RespuestaRtu> Resultado { get; set; }
    }
}
