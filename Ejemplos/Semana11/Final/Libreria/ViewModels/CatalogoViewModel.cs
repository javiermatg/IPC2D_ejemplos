using Libreria.Models;

namespace Libreria.ViewModels;

// ===========================================================================
//  ARCHIVO:   ViewModels/CatalogoViewModel.cs
//
//  QUE ES:    Un ViewModel: una clase hecha a la medida de UNA pantalla, el
//             listado del catalogo. Junta en un solo objeto todo lo que esa
//             vista necesita mostrar.
//
//  POR QUE
//  EXISTE:    Una vista recibe un solo @model. El listado ahora necesita
//             cuatro cosas: los libros (ya filtrados), el texto que se busco,
//             el total y cuantos hay disponibles. En vez de repartirlas en
//             ViewBag (sin tipo, los errores salen frente al usuario), se
//             empaquetan aqui y el compilador revisa cada nombre.
//
//  QUE SE
//  HACE AQUI: Solo datos listos para mostrar y alguna propiedad calculada de
//             presentacion. Nada de reglas del negocio: esas siguen en
//             Models/ (Libro y Catalogo).
//
//  MODELO vs
//  VIEWMODEL: El modelo (Libro, Catalogo) describe el PROBLEMA y serviria
//             igual en una app de consola. El ViewModel describe una PANTALLA
//             y solo existe porque hay una vista que lo usa.
// ===========================================================================
public class CatalogoViewModel
{
    public Catalogo Libros { get; set; }       // los libros que se van a dibujar (ya filtrados)
    public string Texto { get; set; }          // lo que el usuario escribio en el buscador
    public int Total { get; set; }             // cuantos libros hay en TODO el catalogo
    public int Disponibles { get; set; }       // cuantos de ellos tienen existencias

    // Propiedades calculadas de presentacion: la vista las pregunta, no las calcula.
    public int Agotados => Total - Disponibles;
    public bool HayFiltro => !string.IsNullOrWhiteSpace(Texto);
    public int Encontrados => Libros.Cantidad;
}
