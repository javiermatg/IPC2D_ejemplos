using System.Collections;

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
//             recorrido que hace posible el @foreach de la vista.
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
public class Catalogo : IEnumerable<Libro>
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
        CargarLibrosDeEjemplo();
    }

    // Segundo constructor, tambien privado: crea un catalogo VACIO. Lo usa
    // Filtrar() para armar el resultado de una busqueda sin tocar el catalogo
    // principal. El parametro solo sirve para distinguirlo del de arriba.
    private Catalogo(bool vacio)
    {
    }

    private void CargarLibrosDeEjemplo()
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
            Editorial = "Alianza", Precio = 145.00, Existencias = 1
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

    // ----- Semana 11: filtrar y contar -----------------------------------
    // Devuelve OTRO catalogo con los libros cuyo titulo o autor contienen el
    // texto. El catalogo original no se modifica. Se sigue sin usar List:
    // el resultado es otra lista enlazada, armada con el mismo Agregar().
    public Catalogo Filtrar(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
        {
            return this;                         // sin texto no hay nada que filtrar
        }

        var resultado = new Catalogo(true);      // catalogo vacio (constructor privado)
        var actual = _primero;

        while (actual != null)
        {
            var libro = actual.Valor;
            if (Contiene(libro.Titulo, texto) || Contiene(libro.Autor, texto))
            {
                resultado.Agregar(libro);        // el mismo objeto Libro, en otro nodo
            }
            actual = actual.Siguiente;
        }

        return resultado;
    }

    // Cuantos libros tienen existencias. Es una regla del negocio, por eso se
    // calcula aqui y no en la vista.
    public int CantidadDisponibles
    {
        get
        {
            int cuenta = 0;
            foreach (var libro in this)          // this tambien se puede recorrer: es IEnumerable
            {
                if (libro.HayExistencias) cuenta++;
            }
            return cuenta;
        }
    }

    // Comparacion sin distinguir mayusculas de minusculas.
    private static bool Contiene(string campo, string texto)
    {
        return campo != null
            && campo.Contains(texto.Trim(), StringComparison.OrdinalIgnoreCase);
    }

    // ----- Lo que hace posible el @foreach de la vista --------------------
    // Implementar IEnumerable<Libro> es el contrato que le dice a C#:
    // "yo se entregar mis elementos uno por uno". Eso es todo lo que necesita
    // un foreach, y por eso la vista puede escribir @foreach sobre el
    // catalogo sin saber que adentro hay nodos.
    //
    // yield return entrega un libro y deja el metodo en pausa; en la vuelta
    // siguiente del foreach, el metodo continua justo en la linea de abajo.
    // El compilador arma con esto la clase que recorre la lista.
    public IEnumerator<Libro> GetEnumerator()
    {
        var actual = _primero;

        while (actual != null)
        {
            yield return actual.Valor;           // entrega este libro y espera
            actual = actual.Siguiente;           // al volver, avanza un eslabon
        }
    }

    // Version antigua de la misma interfaz, sin tipo. C# la exige; se resuelve
    // en una linea reutilizando la de arriba.
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    // NOTA: en la Semana 8 esto se hizo distinto, con un GetEnumerator()
    // propio que devolvia la clase RecorridoPeliculas. Las dos formas son
    // validas: foreach se conforma con que exista GetEnumerator(). Aqui se usa
    // IEnumerable<Libro> porque ademas lo entienden los tag helpers y el resto
    // de la libreria de .NET.
}
