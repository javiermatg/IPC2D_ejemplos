using Libreria.Models;
using Microsoft.AspNetCore.Mvc;

namespace Libreria.Controllers;

// ===========================================================================
//  ARCHIVO:   Controllers/LibrosController.cs
//
//  QUE ES:    La C de MVC: el controlador del modulo de libros. Es una clase
//             de C# que hereda de Controller, y cada metodo publico suyo es
//             una direccion que la aplicacion sabe atender.
//
//  POR QUE
//  EXISTE:    Es el que coordina. Recibe la peticion, le pide los datos al
//             modelo y escoge que vista se muestra. Ni el modelo ni la vista
//             se hablan entre si: todo pasa por aqui.
//
//  QUE SE
//  HACE AQUI: Decidir. No se guarda nada (eso lo hace Catalogo) y no se
//             escribe HTML (eso lo hacen las vistas). Si un metodo de este
//             archivo crece mas de diez o quince lineas, casi siempre es
//             senal de que hay logica que le toca al modelo.
//
//  EL NOMBRE
//  IMPORTA:   La clase se llama LibrosController, entonces:
//               - la URL es  /Libros          (sin el sufijo Controller)
//               - las vistas van en  Views/Libros/
//             Esa coincidencia no es adorno: es el mecanismo con el que el
//             framework encuentra las cosas.
// ===========================================================================
public class LibrosController : Controller
{
    // El catalogo compartido. Es una propiedad de solo lectura que siempre
    // devuelve la misma instancia (el singleton), asi que lo que se agrega en
    // una peticion sigue ahi en la siguiente.
    private Catalogo Catalogo => Models.Catalogo.Instancia;

    // =======================================================================
    //  ACCION 1: el listado
    //  Responde a:   /Libros      /Libros/Index
    // =======================================================================
    public IActionResult Index()
    {
        // View(objeto) hace dos cosas de un solo golpe:
        //   1. escoge la vista Views/Libros/Index.cshtml (por convencion)
        //   2. le entrega ese objeto, que alla adentro se llama Model
        // El tipo de lo que se manda aqui tiene que ser el mismo que declara
        // la vista en su linea @model; si no, el error sale al compilar.
        return View(Catalogo);
    }

    // =======================================================================
    //  ACCION 2: el detalle de un libro
    //  Responde a:   /Libros/Detalle/9788437604947
    //                              \_____________/
    //                               esto llega en el parametro id
    // =======================================================================
    public IActionResult Detalle(string id)
    {
        // De donde sale el valor de id: del pedazo {id?} de la plantilla de
        // ruteo. El model binding lo busca POR NOMBRE, y por eso el parametro
        // tiene que llamarse id y no otra cosa. Si la URL no trae ese pedazo,
        // id llega en null, porque el ? lo declara opcional.
        var libro = Catalogo.Buscar(id);

        // El metodo Buscar devuelve null cuando no encuentra nada, asi que
        // hay que preverlo SIEMPRE. Sin estas tres lineas, la vista reventaria
        // con NullReferenceException frente al usuario.
        if (libro == null)
        {
            // NotFound() devuelve el codigo 404 de HTTP. Devolver el codigo
            // correcto es parte de programar del lado del servidor: el
            // navegador y los buscadores se guian por ese numero.
            return NotFound();
        }

        // Aqui la vista recibe UN libro, no el catalogo completo. Cada vista
        // declara que tipo espera; son dos vistas distintas con dos @model
        // distintos aunque las atienda el mismo controlador.
        return View(libro);
    }

    // =======================================================================
    //  ACCION 3a: mostrar el formulario vacio
    //  Responde a:   /Libros/Nueva   cuando el navegador PIDE la pagina
    // =======================================================================
    [HttpGet]                                    // GET = "dame la pagina"
    public IActionResult Nueva()
    {
        // Se manda un libro recien creado, con todo en blanco. La vista lo
        // usa para dibujar el formulario: cada campo se amarra a una
        // propiedad de este objeto con asp-for.
        return View(new Libro());
    }

    // =======================================================================
    //  ACCION 3b: recibir lo que el usuario escribio
    //  Responde a:   /Libros/Nueva   cuando el formulario se ENVIA
    //
    //  Dos metodos con el mismo nombre y la misma URL. Los distingue el
    //  atributo: uno atiende GET y el otro POST. Es el patron normal de un
    //  formulario en MVC.
    // =======================================================================
    [HttpPost]                                   // POST = "aqui van estos datos"
    [ValidateAntiForgeryToken]                   // exige el token oculto que puso el <form>:
                                                 // impide que otra pagina envie datos a la nuestra
    public IActionResult Nueva(Libro libro)
    {
        // Nadie escribio el codigo que llena este objeto: lo armo el model
        // binding. Tomo cada campo del formulario y lo emparejo con la
        // propiedad del mismo nombre (name="Titulo" -> libro.Titulo).
        // Por eso el atributo name de los <input> no es opcional.

        // ModelState trae el resultado de revisar las DataAnnotations que
        // estan en Models/Libro.cs. Esta es LA validacion que cuenta: la del
        // navegador ayuda al usuario, pero se puede quitar desde el inspector.
        if (!ModelState.IsValid)
        {
            // Se devuelve la MISMA vista con el objeto que el usuario mando,
            // para que no pierda lo que ya habia escrito y vea los mensajes.
            return View(libro);
        }

        // Regla del negocio: no puede haber dos libros con el mismo ISBN.
        // Se pregunta al modelo, que es quien sabe lo que ya existe.
        if (Catalogo.Buscar(libro.Isbn) != null)
        {
            // Se puede agregar un error a mano; sale en el mismo resumen de
            // validacion que los de las DataAnnotations.
            ModelState.AddModelError("Isbn", "Ya existe un libro con ese ISBN.");
            return View(libro);
        }

        Catalogo.Agregar(libro);                 // se delega en el modelo: el controlador no sabe de nodos

        // RedirectToAction NO devuelve HTML: devuelve un 302 que le dice al
        // navegador "vaya a esta otra direccion". Se usa despues de todo POST
        // para que, si el usuario recarga, no se reenvie el formulario y se
        // agregue el libro dos veces.
        return RedirectToAction("Index");
    }
}
