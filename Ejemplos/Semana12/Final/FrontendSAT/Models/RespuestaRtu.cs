// ===========================================================================
//  ARCHIVO:   Models/RespuestaRtu.cs  (FrontendSAT)
//
//  QUE ES:    La respuesta de la API al actualizar el RTU, ya como objeto.
//
//  OJO:       Igual que Contribuyente: es el contrato que comparte con la API.
// ===========================================================================

using System.Text.Json.Serialization;

namespace FrontendSAT.Models
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
