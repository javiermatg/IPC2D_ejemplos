// ===========================================================================
//  ARCHIVO:   Program.cs  (FrontendSAT)
//
//  QUE ES:    El punto de arranque del FRONTEND. Es el "Programa 1" del
//             enunciado: una aplicacion web MVC con vistas Razor que NO guarda
//             datos. Todo lo que muestra se lo pide a la API por HTTP.
//
//  POR QUE
//  EXISTE:    Arma el servidor web, registra MVC y registra el cliente HTTP
//             que habla con ApiSAT.
//
//  OJO:       Aqui hay DOS servidores en juego:
//               el navegador  -->  FrontendSAT (5080)  -->  ApiSAT (5090)
//             El navegador nunca habla con la API directamente: lo hace el
//             C# del frontend con HttpClient. Por eso no hace falta CORS.
// ===========================================================================

using FrontendSAT.Servicios;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Registra ClienteApiSAT con un HttpClient ya configurado.
// La URL de la API se lee de appsettings.json (seccion "ApiSAT"), para no
// dejarla escrita en el codigo: si la API cambia de puerto, se cambia alla.
builder.Services.AddHttpClient<ClienteApiSAT>(cliente =>
{
    cliente.BaseAddress = new Uri(builder.Configuration["ApiSAT:UrlBase"]);
    cliente.Timeout = TimeSpan.FromSeconds(10);   // si la API no contesta, no esperar para siempre
});

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
