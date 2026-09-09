using CarteleraCine.Modelos;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarteleraCine.Pages;

// ===========================================================================
//  ARCHIVO:   Pages/Detalle.cshtml.cs
//
//  QUE ES:    El PageModel de la ficha de una sola pelicula.
//
//  POR QUE
//  EXISTE:    Para demostrar como una pagina recibe un dato desde afuera. La
//             cartelera manda el codigo en la direccion:  /Detalle?id=2
//
//  QUE SE
//  HACE AQUI: Tomar ese codigo, pedirle a la Cartelera que busque la pelicula
//             y dejarla lista para la plantilla. La busqueda NO se programa
//             aqui: se delega en el TDA, que es quien sabe recorrer nodos.
// ===========================================================================
public class DetalleModel : PageModel
{
    // Puede quedar en null si el codigo no existe. La plantilla esta preparada para eso.
    public Pelicula Pelicula { get; set; }

    // El parametro "id" se llena SOLO con lo que venga en la URL despues de ?id=
    // El nombre del parametro debe coincidir con el nombre que va en la direccion.
    public void OnGet(int id)
    {
        Pelicula = Cartelera.Instancia.Buscar(id);   // recorre la lista enlazada hasta encontrarlo
    }
}
