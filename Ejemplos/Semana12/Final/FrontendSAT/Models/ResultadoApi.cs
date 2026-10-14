// ===========================================================================
//  ARCHIVO:   Models/ResultadoApi.cs  (FrontendSAT)
//
//  QUE ES:    Todo lo que se sabe despues de llamar a la API, en un objeto:
//               - si salio bien o no
//               - el codigo de estado HTTP (200, 400, 404...)
//               - el JSON crudo tal como llego (para mostrarlo en clase)
//               - los datos ya convertidos a objeto de C#
//               - un mensaje de error legible, si algo fallo
//
//  POR QUE
//  EXISTE:    Una llamada HTTP puede fallar de muchas formas (API apagada,
//             400, 404, JSON raro). En vez de lanzar excepciones hasta la
//             vista, el cliente devuelve siempre un ResultadoApi y la vista
//             decide que mostrar.
//
//  OJO:       Son dos clases con el mismo nombre:
//               ResultadoApi      -> lo comun a cualquier llamada. La usa la
//                                    vista parcial _RespuestaHttp.
//               ResultadoApi<T>   -> hereda lo anterior y agrega Datos, del
//                                    tipo que corresponda: RespuestaRtu,
//                                    List<Contribuyente>, etc.
// ===========================================================================

namespace FrontendSAT.Models
{
    public class ResultadoApi
    {
        public bool Exito { get; set; }

        // 0 significa que ni siquiera hubo respuesta (la API esta apagada).
        public int CodigoHttp { get; set; }

        public string JsonCrudo { get; set; }

        public string Error { get; set; }
    }

    public class ResultadoApi<T> : ResultadoApi
    {
        public T Datos { get; set; }
    }
}
