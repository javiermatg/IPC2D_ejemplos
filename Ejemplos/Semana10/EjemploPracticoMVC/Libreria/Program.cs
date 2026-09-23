// ===========================================================================
//  ARCHIVO:   Program.cs
//
//  QUE ES:    El punto de arranque de la aplicacion. Es el equivalente exacto
//             del Main() de un programa de consola: la primera linea que
//             ejecuta .NET cuando se escribe  dotnet run.
//
//  POR QUE
//  EXISTE:    Una aplicacion web no "corre y termina": queda encendida
//             esperando peticiones. Este archivo arma ese servidor, le dice
//             que va a usar MVC y lo pone a escuchar.
//
//  QUE SE
//  HACE AQUI: Solo configuracion. Nunca logica del negocio ni HTML. Si manana
//             hay que activar una funcion nueva de ASP.NET Core, se agrega
//             una linea aqui y en ningun otro lado.
//
//  OJO:       Se lee de arriba hacia abajo y el ORDEN importa. El middleware
//             es una fila: cada peticion pasa por UseStaticFiles antes que por
//             UseRouting, y si se invierten, deja de funcionar.
// ===========================================================================

var builder = WebApplication.CreateBuilder(args);   // prepara la configuracion de la aplicacion

// Registra en el contenedor de servicios todo lo que MVC necesita:
// el buscador de controladores, el motor Razor que compila las vistas y
// el model binding que llena los parametros de las acciones.
// Si esta linea falta, el servidor arranca pero ninguna URL responde.
builder.Services.AddControllersWithViews();

var app = builder.Build();                          // arma la aplicacion ya configurada

// ----- Middleware: la fila por la que pasa cada peticion -------------------
app.UseStaticFiles();                               // sirve lo que esta en wwwroot (css, imagenes)
                                                    // tal cual, sin pasar por ningun controlador
app.UseRouting();                                   // activa el ruteo: examina la URL que llego

// ----- La plantilla de URLs (subtema 6.1.2) --------------------------------
// Asi se traduce una direccion a una llamada de C#:
//
//     /Libros/Detalle/9788437604947
//      |      |       |
//      |      |       +--> {id}          -> parametro de la accion
//      |      +----------> {action}      -> el metodo Detalle(...)
//      +-----------------> {controller}  -> la clase LibrosController
//
// Los valores con = son los que se usan cuando la URL no los trae:
// por eso la direccion raiz  /  termina llamando a HomeController.Index().
// La interrogacion en {id?} significa que ese pedazo es opcional.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();                                          // enciende Kestrel y se queda esperando peticiones
                                                    // (esta linea no termina hasta que se detiene con Ctrl+C)
