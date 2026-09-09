namespace CarteleraCine.Modelos;

// ===========================================================================
//  ARCHIVO:   Modelos/Cartelera.cs
//
//  QUE ES:    El objeto que administra la cartelera completa: guarda la lista
//             de peliculas y decide como se agregan.
//
//  POR QUE
//  EXISTE:    Para separar responsabilidades, que es la regla de oro de la
//             POO. Cada clase tiene UN trabajo:
//                 Pelicula          representa una pelicula
//                 NodoPelicula      encadena una pelicula con la siguiente
//                 ListaPeliculas    administra la estructura
//                 Cartelera         administra el negocio (ids, datos iniciales)
//                 las paginas       solo muestran
//
//             Usa el patron SINGLETON: existe una sola Cartelera en toda la
//             aplicacion y se llega a ella con Cartelera.Instancia. Asi todas
//             las paginas ven los mismos datos sin pasarselos entre si.
//
//  QUE SE
//  HACE AQUI: Crear las peliculas de ejemplo, asignar los Id y ofrecer los
//             metodos que usan las paginas.
//
//  OJO:       Los datos viven en memoria, asi que al detener el servidor se
//             pierden. En el Proyecto 2 el origen de los datos sera el XML.
// ===========================================================================
public class Cartelera
{
    // La unica instancia. Es privada y estatica: le pertenece a la clase, no a un objeto.
    private static Cartelera _instancia;

    // Puerta de entrada: la primera vez la crea, despues devuelve siempre la misma.
    public static Cartelera Instancia
    {
        get
        {
            if (_instancia == null)
            {
                _instancia = new Cartelera();   // aqui se ejecuta el constructor privado de abajo
            }
            return _instancia;
        }
    }

    private readonly ListaPeliculas _peliculas = new ListaPeliculas();  // el TDA propio
    private int _siguienteId = 1;                                       // contador para asignar Id unicos

    // Constructor PRIVADO: impide escribir new Cartelera() desde fuera.
    // Es lo que garantiza que solo exista una. Se ejecuta una sola vez.
    private Cartelera()
    {
        Agregar("El ultimo compilador", "Suspenso", 118, "B",
                "Un estudiante descubre que su programa se compila solo a las tres de la manana.",
                "poster-1.png");
        Agregar("Noches de Xela", "Drama", 96, "A",
                "Tres historias que se cruzan una noche fria en el altiplano.",
                "poster-2.png");
        Agregar("La ruta del quetzal", "Aventura", 132, "A",
                "Una expedicion sigue el rastro de un ave que nadie ha logrado fotografiar.",
                "poster-3.png");
    }

    // Las paginas leen la lista por aqui. Solo get: pueden recorrerla, no reemplazarla.
    public ListaPeliculas Peliculas => _peliculas;

    // Agregar una pelicula. El Id NO lo manda quien llama: lo asigna la
    // cartelera, que es la responsable de que no se repita.
    public Pelicula Agregar(string titulo, string genero, int duracion,
                            string clasificacion, string sinopsis, string poster = "poster-generico.png")
    {
        var pelicula = new Pelicula
        {
            Id = _siguienteId,
            Titulo = titulo,
            Genero = genero,
            Duracion = duracion,
            Clasificacion = clasificacion,
            Sinopsis = sinopsis,
            Poster = poster
        };

        _siguienteId++;              // el proximo que entre usara el numero siguiente
        _peliculas.Agregar(pelicula);// se delega en el TDA: la cartelera no sabe de nodos

        return pelicula;
    }

    // Se delega en la lista. La pagina no tiene por que saber como se busca.
    public Pelicula Buscar(int id)
    {
        return _peliculas.Buscar(id);
    }
}
