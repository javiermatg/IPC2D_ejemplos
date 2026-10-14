// ===========================================================================
//  ARCHIVO:   Servicios/ServicioRtu.cs  (ApiSAT)
//
//  QUE ES:    La logica del negocio del RTU. Une las tres piezas:
//               ValidadorNit       -> decide si el NIT sirve
//               RegistroTributario -> guarda en memoria
//               AlmacenJson        -> guarda en archivo
//
//  POR QUE
//  EXISTE:    Para que el controlador quede delgado. El controlador solo
//             traduce HTTP (recibe la peticion y elige el codigo de estado);
//             las reglas de la SAT viven aqui. Asi la misma logica se puede
//             probar o reutilizar sin pasar por HTTP.
//
//  OJO:       Es Singleton (ver Program.cs) y varias peticiones pueden llegar
//             al mismo tiempo. El lock evita que dos mensajes modifiquen el
//             RTU y escriban el archivo a la vez.
// ===========================================================================

using ApiSAT.Models;

namespace ApiSAT.Servicios
{
    public class ServicioRtu
    {
        private const string Archivo = "rtu.json";

        private readonly AlmacenJson almacen;
        private readonly RegistroTributario registro;
        private readonly object candado = new object();

        public ServicioRtu(AlmacenJson almacen)
        {
            this.almacen = almacen;

            // Al encender la API se recupera lo que quedo guardado la vez anterior.
            List<Contribuyente> guardados = almacen.Leer<List<Contribuyente>>(Archivo);
            registro = new RegistroTributario(guardados);
        }

        // Procesa un mensaje completo de actualizacion del RTU y arma la respuesta.
        public RespuestaRtu ProcesarMensaje(List<Contribuyente> entrada)
        {
            RespuestaRtu respuesta = new RespuestaRtu();

            lock (candado)
            {
                foreach (Contribuyente recibido in entrada)
                {
                    // Un elemento vacio ({}) o sin NIT cuenta como NIT invalido.
                    if (recibido == null || !ValidadorNit.EsValido(recibido.Nit))
                    {
                        respuesta.NitsInvalidos++;
                        continue;
                    }

                    Contribuyente limpio = new Contribuyente
                    {
                        Nit = ValidadorNit.Normalizar(recibido.Nit),
                        Nombre = (recibido.Nombre ?? "").Trim()
                    };

                    if (registro.AgregarOActualizar(limpio))
                    {
                        respuesta.ContribuyentesNuevos++;
                    }
                    else
                    {
                        respuesta.ContribuyentesActualizados++;
                    }
                }

                // Se guarda UNA vez al final del mensaje, no por cada contribuyente.
                almacen.Guardar(Archivo, registro.Todos());
            }

            return respuesta;
        }

        public List<Contribuyente> Todos()
        {
            lock (candado)
            {
                return registro.Todos();
            }
        }

        public Contribuyente Buscar(string nit)
        {
            lock (candado)
            {
                return registro.Buscar(ValidadorNit.Normalizar(nit));
            }
        }

        // Opcion "Inicializar" del enunciado: el sistema vuelve a quedar sin datos.
        public void Inicializar()
        {
            lock (candado)
            {
                registro.Limpiar();
                almacen.Borrar(Archivo);
            }
        }
    }
}
