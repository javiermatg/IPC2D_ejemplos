// ===========================================================================
//  ARCHIVO:   Models/Contribuyente.cs  (FrontendSAT)
//
//  QUE ES:    La misma forma del contribuyente que maneja la API.
//
//  POR QUE
//  EXISTE:    El frontend recibe JSON de la API y lo convierte a objetos para
//             mostrarlos en las vistas. Para eso necesita una clase con los
//             mismos nombres JSON.
//
//  OJO:       Esta clase esta REPETIDA a proposito en los dos proyectos.
//             Son dos programas distintos que solo comparten el CONTRATO:
//             el formato del JSON. Si la API cambia "NIT" por "nit", esta clase
//             tambien tiene que cambiar, o los datos llegaran vacios.
// ===========================================================================

using System.Text.Json.Serialization;

namespace FrontendSAT.Models
{
    public class Contribuyente
    {
        [JsonPropertyName("NIT")]
        public string Nit { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }
    }
}
