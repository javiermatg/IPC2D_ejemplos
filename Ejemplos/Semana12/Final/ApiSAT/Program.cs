// ===========================================================================
//  ARCHIVO:   Program.cs  (ApiSAT)
//
//  QUE ES:    El punto de arranque del BACKEND. Es el "Servicio 2" del
//             enunciado: una API que solo habla HTTP + JSON. No tiene vistas,
//             no tiene HTML, no tiene CSS.
//
//  POR QUE
//  EXISTE:    Arma el servidor, registra los servicios que se comparten entre
//             peticiones y lo pone a escuchar en http://localhost:5090.
//
//  QUE SE
//  HACE AQUI: Solo configuracion. Ninguna regla de la SAT vive aqui.
//
//  OJO:       Comparelo con el Program.cs del frontend:
//               - aqui es AddControllers()          (sin vistas)
//               - alla es AddControllersWithViews() (con vistas Razor)
//             Esa es toda la diferencia de arranque entre una API y una web.
// ===========================================================================

using ApiSAT.Servicios;

var builder = WebApplication.CreateBuilder(args);

// Controladores SIN vistas. Lo que devuelvan las acciones se convierte a JSON.
builder.Services.AddControllers()
    .AddJsonOptions(opciones =>
    {
        // Respuestas con sangria: se leen mejor en Postman y en el navegador.
        opciones.JsonSerializerOptions.WriteIndented = true;
    });

// Singleton = UNA sola instancia para toda la vida de la API.
// Es lo correcto aqui: el RTU en memoria debe ser el mismo para todas las
// peticiones. Si fuera AddScoped, cada peticion tendria un RTU vacio nuevo.
builder.Services.AddSingleton<AlmacenJson>();
builder.Services.AddSingleton<ServicioRtu>();

var app = builder.Build();

// Una ruta minima para comprobar rapido que la API esta encendida:
// abrir http://localhost:5090 en el navegador.
app.MapGet("/", () => "ApiSAT funcionando. Pruebe GET /api/rtu");

// Activa las rutas declaradas con [Route] y [HttpGet]/[HttpPost] en los controladores.
app.MapControllers();

app.Run();
