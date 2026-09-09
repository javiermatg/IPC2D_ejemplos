using CarteleraCine.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarteleraCine.Pages;

// ===========================================================================
//  ARCHIVO:   Pages/Nueva.cshtml.cs
//
//  QUE ES:    El PageModel de la pagina que captura una pelicula nueva.
//
//  POR QUE
//  EXISTE:    Es el otro lado del formulario. Hasta ahora las paginas solo
//             mostraban datos; esta los RECIBE. Es el equivalente web del
//             Console.ReadLine() y del posterior "guardar en la estructura".
//
//  QUE SE
//  HACE AQUI: Dos metodos con nombres que ASP.NET reconoce solos:
//                 OnGet   se ejecuta cuando el usuario ABRE la pagina
//                 OnPost  se ejecuta cuando el usuario ENVIA el formulario
//
//  REGLA CLAVE:
//             El atributo name="" de cada control del formulario debe
//             coincidir EXACTAMENTE con el nombre de la propiedad marcada con
//             [BindProperty]. Si no coinciden, la propiedad llega vacia y no
//             hay ningun error que lo avise. Es el fallo mas comun.
// ===========================================================================
public class NuevaModel : PageModel
{
    // [BindProperty] significa: "llename sola con lo que venga del formulario".
    // Sin ese atributo, la propiedad se queda en su valor por defecto.
    [BindProperty] public string Titulo { get; set; }         // <input name="Titulo">
    [BindProperty] public string Genero { get; set; }         // <input name="Genero">
    [BindProperty] public int Duracion { get; set; }          // <input name="Duracion" type="number">
    [BindProperty] public string Clasificacion { get; set; }  // <select name="Clasificacion">
    [BindProperty] public string Sinopsis { get; set; }       // <textarea name="Sinopsis">

    public string Error { get; set; }             // mensaje que la plantilla muestra si algo sale mal
    public ListaPeliculas Peliculas { get; set; } // el TDA propio, para la tabla del final

    // GET: el usuario abrio la pagina. Solo hay que dejar lista la tabla.
    public void OnGet()
    {
        Peliculas = Cartelera.Instancia.Peliculas;
    }

    // POST: el usuario presiono Guardar y el navegador envio los datos.
    // Devuelve IActionResult porque hay que DECIDIR que responder:
    // volver a dibujar esta pagina, o mandar al usuario a otra.
    public IActionResult OnPost()
    {
        // ---- VALIDACION EN EL SERVIDOR ------------------------------------
        // El formulario ya trae required, min y max en el HTML, pero eso solo
        // protege al usuario honesto: cualquiera puede desactivarlo desde el
        // inspector del navegador. Por eso se vuelve a revisar aqui.
        if (string.IsNullOrWhiteSpace(Titulo))
        {
            Error = "El titulo es obligatorio.";
            Peliculas = Cartelera.Instancia.Peliculas;   // hay que volver a dejar los datos de la vista
            return Page();                               // se vuelve a dibujar ESTA pagina, con el error
        }

        if (Duracion < 30 || Duracion > 300)
        {
            Error = "La duracion debe estar entre 30 y 300 minutos.";
            Peliculas = Cartelera.Instancia.Peliculas;
            return Page();
        }

        // ---- GUARDAR ------------------------------------------------------
        // Se delega en la Cartelera: ella asigna el Id y ella se lo pasa al TDA.
        // Esta pagina no sabe ni le importa que por dentro hay nodos enlazados.
        Cartelera.Instancia.Agregar(Titulo, Genero, Duracion, Clasificacion, Sinopsis);

        // Redirigir despues de guardar evita que al recargar con F5 se agregue
        // la misma pelicula dos veces. Es un patron estandar en la web.
        return RedirectToPage("Index");
    }
}
