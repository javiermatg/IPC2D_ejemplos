namespace Libreria.Models;

// ===========================================================================
//  ARCHIVO:   Models/RecorridoLibros.cs
//
//  QUE ES:    Un "dedo" que va senalando los libros del catalogo, uno por
//             uno, desde el primero hasta el ultimo.
//
//  POR QUE
//  EXISTE:    Para que @foreach pueda recorrer el Catalogo sin usar
//             IEnumerable<T>, IEnumerator<T> ni yield, que son parte de las
//             colecciones genericas de C# y estan prohibidas en el
//             Proyecto 2.
//
//  QUE SE
//  HACE AQUI: Solo avanzar y recordar donde va. No agrega, no busca y no
//             cuenta: de eso se encarga Catalogo. Una clase, un trabajo.
//
//  COMO LO
//  USA FOREACH: Esto que se escribe en la vista
//
//                   foreach (Libro libro in catalogo) { ... }
//
//             el compilador lo convierte, por dentro, en algo asi:
//
//                   RecorridoLibros r = catalogo.GetEnumerator();
//                   while (r.MoveNext())
//                   {
//                       Libro libro = r.Current;
//                       ...
//                   }
//
//             Es decir: el foreach no es magia, es un while que llama a los
//             dos metodos de esta clase.
// ===========================================================================
public class RecorridoLibros
{
    private NodoLibro _siguiente;               // el nodo que se va a entregar en la proxima vuelta
    private Libro _actual;                      // el libro que se entrego en la vuelta actual

    // Nace apuntando a la cabeza de la lista. Todavia no entrego nada:
    // foreach llama a MoveNext() ANTES de leer Current por primera vez.
    public RecorridoLibros(NodoLibro primero)
    {
        _siguiente = primero;
        _actual = null;
    }

    // Lo que foreach pone en la variable del ciclo (libro, en la vista).
    public Libro Current => _actual;

    // Avanza un eslabon. Devuelve true si habia un libro que entregar y
    // false cuando la lista se termino; con false el foreach se detiene.
    public bool MoveNext()
    {
        if (_siguiente == null)                 // ya no quedan nodos
        {
            return false;
        }

        _actual = _siguiente.Valor;             // se toma el libro de este nodo
        _siguiente = _siguiente.Siguiente;      // y se deja listo el que sigue
        return true;
    }
}
