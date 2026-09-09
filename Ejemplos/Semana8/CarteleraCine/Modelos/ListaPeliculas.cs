namespace CarteleraCine.Modelos;

// ===========================================================================
//  ARCHIVO:   Modelos/ListaPeliculas.cs
//
//  QUE ES:    Una lista simplemente enlazada de peliculas, construida a mano.
//             Es el TDA (Tipo de Dato Abstracto) de este ejemplo.
//
//  POR QUE
//  EXISTE:    Es el reemplazo de List<Pelicula>. En el Proyecto 2 esta
//             prohibido usar las estructuras que trae C#, asi que la
//             estructura se programa. Aqui esta la version mas sencilla:
//             cada nodo apunta al siguiente y la lista guarda el primero.
//
//             "Abstracto" significa que quien la usa no necesita saber que
//             hay nodos adentro: solo pide Agregar o Buscar. Esa es la idea
//             central de la programacion orientada a objetos, y por eso los
//             campos son privados.
//
//  QUE SE
//  HACE AQUI: Insertar al final, buscar por Id, contar, y entregar el objeto
//             que permite recorrerla. Si manana quieren cambiarla por una
//             lista doble o un arbol, solo se toca este archivo: las paginas
//             no se enteran.
// ===========================================================================
public class ListaPeliculas
{
    // Campos privados: nadie fuera de esta clase puede tocarlos directamente.
    private NodoPelicula _primero;   // cabeza de la lista, null si esta vacia
    private NodoPelicula _ultimo;    // se guarda para insertar al final sin recorrer todo
    private int _cantidad;           // se lleva la cuenta en vez de recorrer cada vez

    // ----- Consultas de solo lectura --------------------------------------
    // Solo tienen get: se pueden leer desde afuera, pero no modificar.
    public NodoPelicula Primero => _primero;      // lo usa la pagina que recorre con while
    public int Cantidad => _cantidad;             // lo usa la pagina para mostrar el total
    public bool EstaVacia => _primero == null;    // se lee mejor que preguntar Cantidad == 0

    // ----- Insertar al final ----------------------------------------------
    public void Agregar(Pelicula pelicula)
    {
        var nodo = new NodoPelicula(pelicula);    // se envuelve la pelicula en un nodo

        if (_primero == null)                     // caso 1: la lista estaba vacia
        {
            _primero = nodo;                      // el nuevo nodo es el primero
            _ultimo = nodo;                       // y tambien el ultimo
        }
        else                                      // caso 2: ya habia elementos
        {
            _ultimo.Siguiente = nodo;             // el que era ultimo ahora apunta al nuevo
            _ultimo = nodo;                       // y el nuevo pasa a ser el ultimo
        }

        _cantidad++;                              // se actualiza el contador
    }

    // ----- Busqueda lineal por Id -----------------------------------------
    // Se recorre nodo por nodo hasta encontrarlo. Devuelve null si no existe,
    // asi que quien llame a este metodo DEBE prever ese caso.
    public Pelicula Buscar(int id)
    {
        var actual = _primero;                    // se arranca en la cabeza

        while (actual != null)                    // mientras queden nodos
        {
            if (actual.Dato.Id == id)             // se compara el dato guardado
            {
                return actual.Dato;               // encontrado: se devuelve y se corta el ciclo
            }
            actual = actual.Siguiente;            // no era: se avanza un eslabon
        }

        return null;                              // se recorrio todo y no aparecio
    }

    // ----- Permitir foreach -----------------------------------------------
    // Este metodo es el que hace posible escribir @foreach en las paginas.
    // No hace falta implementar ninguna interfaz: basta con que exista un
    // GetEnumerator() publico que devuelva un objeto con Current y MoveNext().
    public RecorridoPeliculas GetEnumerator()
    {
        return new RecorridoPeliculas(_primero);  // cada foreach recibe su propio recorrido
    }
}
