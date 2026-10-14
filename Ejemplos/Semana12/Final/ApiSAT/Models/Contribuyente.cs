// ===========================================================================
//  ARCHIVO:   Models/Contribuyente.cs  (ApiSAT)
//
//  QUE ES:    Un contribuyente del RTU: su NIT y su nombre.
//
//  POR QUE
//  EXISTE:    Es la misma forma que trae cada elemento del mensaje de entrada:
//                 {"NIT": "7378106", "nombre": "Empresa ABC, S.A."}
//             Con esta clase el JSON se convierte en objeto (deserializar) y
//             el objeto en JSON (serializar) sin escribir ni un solo parseo a mano.
//
//  OJO:       [JsonPropertyName] amarra la propiedad de C# con el nombre EXACTO
//             del JSON. El enunciado dice que los mensajes de entrada y salida
//             no se pueden cambiar: por eso se respeta "NIT" en mayusculas y
//             "nombre" en minusculas, aunque en C# se llamen Nit y Nombre.
// ===========================================================================

using System.Text.Json.Serialization;

namespace ApiSAT.Models
{
    public class Contribuyente
    {
        [JsonPropertyName("NIT")]
        public string Nit { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }
    }
}
