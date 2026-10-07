using System.ComponentModel.DataAnnotations;

namespace Libreria.Models;

// ===========================================================================
//  ARCHIVO:   Models/Libro.cs
//
//  QUE ES:    La entidad del ejemplo: un libro del catalogo. Es una clase de
//             C# normal y corriente, de las que ya escribian en IPC1.
//
//  POR QUE
//  EXISTE:    Es la M de MVC. El modelo representa una cosa del problema que
//             el programa resuelve, con sus datos y sus reglas.
//
//  QUE SE
//  HACE AQUI: Solo guardar datos y una regla propia del libro. Fijense en lo
//             que NO hay: ni una etiqueta de HTML, ni una peticion, ni nada
//             que sepa que existe una pantalla. Por eso este mismo archivo
//             serviria igual en una aplicacion de consola.
//
//  DETALLE:   Los atributos entre corchetes ([Required], [StringLength]) se
//             llaman DataAnnotations. Son reglas de validacion que el
//             controlador revisa con ModelState. Se escriben una sola vez,
//             aqui, y valen para cualquier pantalla que use esta clase.
// ===========================================================================
public class Libro
{
    // ----- Los datos ------------------------------------------------------
    // Propiedades automaticas: { get; set; } es la forma corta de un campo
    // privado con sus metodos para leerlo y escribirlo.

    [Required(ErrorMessage = "El ISBN es obligatorio.")]
    [StringLength(20, MinimumLength = 4,
                  ErrorMessage = "El ISBN debe tener entre 4 y 20 caracteres.")]
    public string Isbn { get; set; }          // identifica al libro: viaja en la URL del detalle

    [Required(ErrorMessage = "El titulo es obligatorio.")]
    [StringLength(80, ErrorMessage = "El titulo no puede pasar de 80 caracteres.")]
    public string Titulo { get; set; }

    [Required(ErrorMessage = "El autor es obligatorio.")]
    public string Autor { get; set; }

    public string Editorial { get; set; }

    [Range(1, 3000, ErrorMessage = "El precio debe estar entre Q1 y Q3000.")]
    public double Precio { get; set; }

    [Range(0, 9999, ErrorMessage = "Las existencias no pueden ser negativas.")]
    public int Existencias { get; set; }

    // ----- Una regla que le pertenece al libro ----------------------------
    // Propiedad calculada: no guarda nada, se calcula cada vez que se lee.
    // Esta aqui y no en la vista porque "hay o no hay libro disponible" es una
    // regla del negocio. Si manana cambia, se cambia en este archivo y todas
    // las pantallas quedan corregidas de una vez.
    public bool HayExistencias => Existencias > 0;

    // Otra regla del mismo tipo: la vista solo la imprime, no la calcula.
    public string PrecioEnTexto => "Q " + Precio.ToString("0.00");
}
