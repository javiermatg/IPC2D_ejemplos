namespace CarteleraCine.Modelos;

// ===========================================================================
//  ARCHIVO:   Modelos/NodoPelicula.cs
//
//  QUE ES:    Un eslabon de la cadena. Guarda UNA pelicula y la direccion de
//             la que sigue.
//
//  POR QUE
//  EXISTE:    El Proyecto 2 prohibe usar las estructuras de C# (List, Stack,
//             Queue) y tambien los arreglos como solucion final. La forma
//             correcta es construir la estructura con objetos propios, y el
//             nodo es la pieza mas pequena de esa construccion.
//
//             Este nodo NO es generico a proposito: guarda peliculas y nada
//             mas. Un nodo generico se estudia despues; aqui interesa que se
//             vea con claridad quien apunta a quien.
//
//  QUE SE
//  HACE AQUI: Nada de logica. El nodo solo carga un dato y una referencia.
//             Toda la inteligencia esta en ListaPeliculas.
// ===========================================================================
public class NodoPelicula
{
    public Pelicula Dato { get; set; }          // la pelicula que este nodo guarda
    public NodoPelicula Siguiente { get; set; } // referencia al proximo nodo; null si es el ultimo

    // Constructor: obliga a que todo nodo nazca con su pelicula adentro.
    // Asi es imposible crear un nodo vacio por accidente.
    public NodoPelicula(Pelicula dato)
    {
        Dato = dato;
        Siguiente = null;      // al crearse todavia no tiene a quien apuntar
    }
}
