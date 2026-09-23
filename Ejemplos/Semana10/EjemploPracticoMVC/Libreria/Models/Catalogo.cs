namespace Libreria.Models;

// ===========================================================================
//  ARCHIVO:   Models/Catalogo.cs
//
//  QUE ES:    El modelo que guarda la coleccion completa de libros. Por
//             dentro es una lista simplemente enlazada hecha a mano; por
//             fuera se usa como cualquier coleccion de C#.
//
//  POR QUE
//  EXISTE:    Dos razones. La primera: reemplaza a List<Libro>, que esta
//             prohibida en el Proyecto 2. La segunda: es el unico lugar donde
//             se decide COMO se guardan los datos. Si manana esto se cambia
//             por un arbol, solo se toca este archivo; ni el controlador ni
//             las vistas se enteran. A eso se le llama abstraccion.
//
//  QUE SE
//  HACE AQUI: Agregar al final, buscar por ISBN, contar y entregar el
//             recorrido (RecorridoLibros) que hace posible el @foreach de
//             la vista.
//
//  SINGLETON: Existe UN solo catalogo en toda la aplicacion y se llega a el
//             con Catalogo.Instancia. Hace falta porque HTTP no tiene
//             memoria: cada peticion es un viaje distinto, y si cada una
//             creara su propio catalogo, el libro agregado desapareceria en
//             la siguiente pantalla.
//
//  OJO:       Los datos viven en memoria: al detener el servidor se pierden.
//             En el Proyecto 2 el origen de los datos sera el archivo XML.
// ===========================================================================
public class Catalogo
{
    // ----- El singleton ---------------------------------------------------
    private static Catalogo _instancia;          // la unica instancia; es de la clase, no de un objeto

    public static Catalogo Instancia             // puerta de entrada: la crea la primera vez
    {                                            // y despues devuelve siempre la misma
        get
        {
            if (_instancia == null)
            {
                _instancia = new Catalogo();     // aqui se ejecuta el constructor privado de abajo
            }
            return _instancia;
        }
    }

    // ----- Por dentro: la lista enlazada ----------------------------------
    // Campos privados. Nadie fuera de esta clase puede tocarlos, y por eso
    // nadie fuera de esta clase necesita saber que existen nodos.
    private NodoLibro _primero;                  // cabeza de la lista, null si esta vacia
    private NodoLibro _ultimo;                   // se guarda para insertar al final sin recorrer todo
    private int _cantidad;                       // se lleva la cuenta en vez de contar cada vez

    // Consultas de solo lectura: se pueden leer desde la vista, no modificar.
    public int Cantidad => _cantidad;
    public bool EstaVacio => _primero == null;

    // Constructor PRIVADO: impide escribir new Catalogo() desde fuera.
    // Es lo que garantiza que solo exista uno. Se ejecuta una sola vez, la
    // primera vez que alguien pide Catalogo.Instancia.
    private Catalogo()
    {
        Agregar(new Libro
        {
            Isbn = "9788437604947", Titulo = "Rayuela", Autor = "Julio Cortazar",
            Editorial = "Catedra", Precio = 185.50, Existencias = 4
        });
        Agregar(new Libro
        {
            Isbn = "9789684114012", Titulo = "Hombres de maiz", Autor = "Miguel Angel Asturias",
            Editorial = "Piedra Santa", Precio = 120.00, Existencias = 7
        });
        Agregar(new Libro
        {
            Isbn = "9780307474728", Titulo = "Cien anos de soledad", Autor = "Gabriel Garcia Marquez",
            Editorial = "Vintage", Precio = 210.75, Existencias = 0
        });
        Agregar(new Libro
        {
            Isbn = "9789929800014", Titulo = "El senor presidente", Autor = "Miguel Angel Asturias",
            Editorial = "Alianza", Precio = 145.00, Existencias = 2
        });
    }

    // ----- Insertar al final ----------------------------------------------
    public void Agregar(Libro libro)
    {
        var nodo = new NodoLibro(libro);         // se envuelve el libro en un nodo

        if (_primero == null)                    // caso 1: el catalogo estaba vacio
        {
            _primero = nodo;                     // el nuevo nodo es el primero
            _ultimo = nodo;                      // y tambien el ultimo
        }
        else                                     // caso 2: ya habia libros
        {
            _ultimo.Siguiente = nodo;            // el que era ultimo ahora apunta al nuevo
            _ultimo = nodo;                      // y el nuevo pasa a ser el ultimo
        }

        _cantidad++;                             // se actualiza el contador
    }

    // ----- Busqueda lineal por ISBN ---------------------------------------
    // Se recorre nodo por nodo hasta encontrarlo. Devuelve null si no existe,
    // asi que quien llame a este metodo DEBE prever ese caso: el controlador
    // lo hace respondiendo NotFound().
    public Libro Buscar(string isbn)
    {
        var actual = _primero;                   // se arranca en la cabeza

        while (actual != null)                   // mientras queden nodos
        {
            if (actual.Valor.Isbn == isbn)       // se compara el dato guardado
            {
                return actual.Valor;             // encontrado: se devuelve y se corta el ciclo
            }
            actual = actual.Siguiente;           // no era: se avanza un eslabon
        }

        return null;                             // se recorrio todo y no aparecio
    }

    // ----- Lo que hace posible el @foreach de la vista --------------------
    // foreach NO exige ninguna interfaz. El compilador de C# solo revisa que
    // el objeto tenga un metodo publico llamado GetEnumerator() y que lo que
    // ese metodo devuelve tenga:
    //     - una propiedad  Current   (el elemento actual)
    //     - un metodo      MoveNext() (avanza y dice si todavia hay elementos)
    // Si esas tres piezas existen, foreach funciona. No hace falta
    // IEnumerable<T>, ni yield, ni nada generico: todo es codigo nuestro.
    //
    // Por eso la vista puede escribir @foreach sobre el catalogo sin saber
    // que adentro hay nodos. Es el mismo mecanismo de RecorridoPeliculas en
    // la Semana 8.
    public RecorridoLibros GetEnumerator()
    {
        return new RecorridoLibros(_primero);   // un recorrido nuevo que arranca en la cabeza
    }
}
