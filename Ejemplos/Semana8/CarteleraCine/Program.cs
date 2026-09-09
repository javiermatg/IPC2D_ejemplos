// ===========================================================================
//  ARCHIVO:   Program.cs
//
//  QUE ES:    El punto de arranque de la aplicacion. Es el equivalente exacto
//             del Main() de un programa de consola.
//
//  POR QUE
//  EXISTE:    Una aplicacion web no "corre y termina": queda encendida
//             esperando peticiones. Este archivo configura ese servidor y lo
//             pone a escuchar.
//
//  QUE SE
//  HACE AQUI: Solo configuracion. Nunca logica del negocio ni HTML. Si manana
//             hay que activar una funcion nueva de ASP.NET Core, se agrega
//             una linea aqui y en ningun otro lado.
// ===========================================================================

var builder = WebApplication.CreateBuilder(args);   // prepara la configuracion de la aplicacion

builder.Services.AddRazorPages();                   // habilita Razor Pages, es decir las paginas .cshtml

var app = builder.Build();                          // arma la aplicacion ya configurada

app.UseStaticFiles();                               // permite servir lo que esta en wwwroot (css e imagenes)
app.MapRazorPages();                                // conecta cada URL con su pagina: /Nueva -> Pages/Nueva.cshtml

app.Run();                                          // enciende el servidor y se queda esperando peticiones
                                                    // (esta linea no termina hasta que se detiene con Ctrl+C)
