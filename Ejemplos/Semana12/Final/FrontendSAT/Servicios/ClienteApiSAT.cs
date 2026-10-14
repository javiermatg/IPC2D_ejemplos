// ===========================================================================
//  ARCHIVO:   Servicios/ClienteApiSAT.cs  (FrontendSAT)
//
//  QUE ES:    El UNICO lugar del frontend que sabe hablar con la API.
//             Cada metodo es una peticion HTTP:
//
//               EnviarRtuAsync          POST  api/rtu
//               ObtenerContribuyentes   GET   api/rtu
//               BuscarAsync             GET   api/rtu/{nit}
//               InicializarAsync        POST  api/sistema/inicializar
//
//  POR QUE
//  EXISTE:    Los controladores no deberian armar URLs ni leer JSON. Si la API
//             cambia una ruta, se corrige aqui y en ningun otro lado.
//
//  QUE SE
//  HACE AQUI: 1) mandar la peticion con HttpClient
//             2) leer el codigo de estado y el cuerpo
//             3) convertir el JSON a objeto (deserializar)
//             4) atrapar los errores de red para que la pagina no se caiga
//
//  OJO:       El HttpClient NO se crea con new aqui: lo entrega ASP.NET Core
//             ya configurado (ver AddHttpClient en Program.cs). Crear un
//             new HttpClient() por cada peticion agota los sockets del equipo.
// ===========================================================================

using System.Text;
using System.Text.Json;
using FrontendSAT.Models;

namespace FrontendSAT.Servicios
{
    public class ClienteApiSAT
    {
        private readonly HttpClient http;

        public ClienteApiSAT(HttpClient http)
        {
            this.http = http;
        }

        // El archivo se manda TAL CUAL se leyo. El enunciado dice que los
        // mensajes de entrada no se modifican: el frontend no los toca.
        public Task<ResultadoApi<RespuestaRtu>> EnviarRtuAsync(string json)
        {
            StringContent cuerpo = new StringContent(json, Encoding.UTF8, "application/json");
            return LlamarAsync<RespuestaRtu>(() => http.PostAsync("api/rtu", cuerpo));
        }

        public Task<ResultadoApi<List<Contribuyente>>> ObtenerContribuyentesAsync()
        {
            return LlamarAsync<List<Contribuyente>>(() => http.GetAsync("api/rtu"));
        }

        public Task<ResultadoApi<Contribuyente>> BuscarAsync(string nit)
        {
            // EscapeDataString protege la URL si el usuario escribe espacios o simbolos.
            return LlamarAsync<Contribuyente>(() => http.GetAsync("api/rtu/" + Uri.EscapeDataString(nit)));
        }

        public Task<ResultadoApi<JsonElement>> InicializarAsync()
        {
            return LlamarAsync<JsonElement>(() => http.PostAsync("api/sistema/inicializar", null));
        }

        // -------------------------------------------------------------------
        // El trabajo comun de TODAS las llamadas.
        // "peticion" es la llamada HTTP que se quiere hacer, escrita como
        // lambda: () => http.GetAsync("api/rtu"). Asi el try/catch se escribe
        // una sola vez y no en cada metodo.
        // -------------------------------------------------------------------
        private async Task<ResultadoApi<T>> LlamarAsync<T>(Func<Task<HttpResponseMessage>> peticion)
        {
            ResultadoApi<T> resultado = new ResultadoApi<T>();

            try
            {
                // await: se espera la respuesta SIN bloquear el servidor.
                HttpResponseMessage respuesta = await peticion();

                resultado.CodigoHttp = (int)respuesta.StatusCode;
                resultado.JsonCrudo = await respuesta.Content.ReadAsStringAsync();
                resultado.Exito = respuesta.IsSuccessStatusCode;     // true para 200-299

                if (resultado.Exito)
                {
                    resultado.Datos = JsonSerializer.Deserialize<T>(resultado.JsonCrudo);
                }
                else
                {
                    resultado.Error = $"La API respondio {resultado.CodigoHttp} ({respuesta.ReasonPhrase}).";
                }
            }
            catch (HttpRequestException)
            {
                // No hubo respuesta: la API esta apagada o la URL esta mal.
                resultado.Error = $"No se pudo conectar con la API en {http.BaseAddress}. Esta encendida?";
            }
            catch (TaskCanceledException)
            {
                resultado.Error = "La API tardo demasiado en responder.";
            }
            catch (JsonException)
            {
                resultado.Exito = false;
                resultado.Error = "La API respondio algo que no es el JSON esperado.";
            }

            return resultado;
        }
    }
}
