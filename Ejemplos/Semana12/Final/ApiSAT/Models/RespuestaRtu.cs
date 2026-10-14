// ===========================================================================
//  ARCHIVO:   Models/RespuestaRtu.cs  (ApiSAT)
//
//  QUE ES:    La respuesta a un mensaje de actualizacion del RTU.
//
//  POR QUE
//  EXISTE:    El enunciado fija la estructura de salida, letra por letra:
//                 {
//                   "CONTRIBUYENTES_NUEVOS": 2,
//                   "CONTRIBUYENTES_ACTUALIZADOS": 0,
//                   "NITS_INVALIDOS": 0
//                 }
//             La API devuelve un objeto de esta clase y ASP.NET Core lo
//             serializa a ese JSON automaticamente.
//
//  OJO:       Sin [JsonPropertyName], ASP.NET Core escribiria
//             "contribuyentesNuevos" (camelCase) y la respuesta ya no
//             cumpliria con el enunciado.
// ===========================================================================

using System.Text.Json.Serialization;

namespace ApiSAT.Models
{
    public class RespuestaRtu
    {
        [JsonPropertyName("CONTRIBUYENTES_NUEVOS")]
        public int ContribuyentesNuevos { get; set; }

        [JsonPropertyName("CONTRIBUYENTES_ACTUALIZADOS")]
        public int ContribuyentesActualizados { get; set; }

        [JsonPropertyName("NITS_INVALIDOS")]
        public int NitsInvalidos { get; set; }
    }
}
