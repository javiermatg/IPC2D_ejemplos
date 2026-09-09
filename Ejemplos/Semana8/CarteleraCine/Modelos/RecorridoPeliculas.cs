namespace CarteleraCine.Modelos;

// ===========================================================================
//  ARCHIVO:   Modelos/RecorridoPeliculas.cs
//
//  QUE ES:    El objeto que sabe caminar la lista de un nodo al siguiente.
//             En ingles se le llama "enumerador".
//
//  POR QUE
//  EXISTE:    Para poder escribir @foreach en las paginas Razor sin usar
//             ninguna estructura prefabricada de C#.
//
//             El truco: foreach NO exige heredar de nada ni implementar
//             ninguna interfaz. Le basta con que el objeto ofrezca un metodo
//             GetEnumerator() que devuelva algo con:
//                 - una propiedad llamada  Current
//                 - un metodo   llamado    MoveNext()  que devuelva bool
//             Los nombres deben escribirse exactamente asi, con mayuscula.
//
//  QUE SE
//  HACE AQUI: Se guarda cual nodo toca visitar y se avanza uno por uno,
//             exactamente el mismo recorrido que ya hacen en consola con un
//             while, pero encapsulado dentro de un objeto.
// ===========================================================================
public class RecorridoPeliculas
{
    private NodoPelicula _siguiente;   // el nodo que todavia no se ha visitado
    private Pelicula _actual;          // la pelicula que se esta visitando ahora

    // Recibe el primer nodo de la lista: ahi empieza el recorrido.
    public RecorridoPeliculas(NodoPelicula primero)
    {
        _siguiente = primero;
    }

    // foreach lee esta propiedad en cada vuelta para saber el valor actual.
    public Pelicula Current => _actual;

    // foreach llama a este metodo antes de cada vuelta.
    // Devuelve true si logro avanzar, false cuando ya no hay mas nodos.
    public bool MoveNext()
    {
        if (_siguiente == null) return false;   // se acabo la lista: foreach se detiene

        _actual = _siguiente.Dato;              // se toma la pelicula del nodo actual
        _siguiente = _siguiente.Siguiente;      // y se deja listo el nodo de la proxima vuelta
        return true;
    }
}
