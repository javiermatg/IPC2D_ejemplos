namespace Libreria.Models;

// ===========================================================================
//  ARCHIVO:   Models/NodoLibro.cs
//
//  QUE ES:    Un eslabon de la cadena: guarda UN libro y la direccion del
//             siguiente eslabon.
//
//  POR QUE
//  EXISTE:    En el Proyecto 2 esta prohibido usar List, Stack, Queue y
//             arreglos, asi que la estructura se programa. El nodo es la
//             pieza mas pequena de esa estructura.
//
//  QUE SE
//  HACE AQUI: Nada mas que guardar. El nodo no busca, no ordena y no cuenta:
//             de eso se encarga Catalogo. Una clase, un trabajo.
//
//  OJO:       El nodo NO es generico a proposito. Se llama NodoLibro y guarda
//             libros y nada mas, para que se vea con claridad quien apunta a
//             quien antes de estudiar genericos. Es el mismo NodoPelicula de
//             la Semana 8, con otro nombre.
// ===========================================================================
public class NodoLibro
{
    public Libro Valor { get; set; }          // el dato que este eslabon guarda
    public NodoLibro Siguiente { get; set; }  // el eslabon que viene despues; null si es el ultimo

    // Constructor: obliga a que un nodo nazca siempre con su libro adentro.
    // Siguiente queda en null hasta que alguien lo encadene.
    public NodoLibro(Libro valor)
    {
        Valor = valor;
    }
}
