namespace CarteleraCine.Modelos;

// ===========================================================================
//  ARCHIVO:   Modelos/Pelicula.cs
//
//  QUE ES:    La clase que representa UNA pelicula. Es la "entidad" del
//             ejemplo: el objeto del mundo real que el sistema administra.
//
//  POR QUE
//  EXISTE:    Programacion orientada a objetos significa que cada cosa del
//             problema se modela como una clase con sus propios datos y su
//             propio comportamiento. Una pelicula no es un monton de
//             variables sueltas: es un objeto.
//
//  QUE SE
//  HACE AQUI: Solo se declara que datos tiene una pelicula y que sabe hacer
//             por si misma. Nada de HTML, nada de pantallas: esta clase no
//             sabe que existe una pagina web, y esa separacion es a proposito.
// ===========================================================================
public class Pelicula
{
    // ----- Datos (estado del objeto) --------------------------------------
    // Son propiedades, no campos publicos: la propiedad permite controlar
    // despues como se lee o se escribe el valor sin cambiar quien la usa.
    public int Id { get; set; }               // identificador unico, se usa para armar el enlace al detalle
    public string Titulo { get; set; }        // lo que se muestra grande en la tarjeta
    public string Genero { get; set; }        // se muestra como etiqueta debajo del titulo
    public int Duracion { get; set; }         // en minutos: es int porque se hacen cuentas con el
    public string Clasificacion { get; set; } // A, B o C: viene de un <select> del formulario
    public string Poster { get; set; }        // nombre del archivo dentro de wwwroot/img
    public string Sinopsis { get; set; }      // texto largo, viene de un <textarea>

    // ----- Comportamiento (lo que el objeto sabe hacer) -------------------
    // Propiedad calculada: no se guarda en ningun lado, se deduce cada vez
    // que alguien la lee. La cuenta vive DENTRO del objeto, que es de quien
    // es el dato; la pagina solo la imprime con @pelicula.DuracionEnTexto
    public string DuracionEnTexto => (Duracion / 60) + " h " + (Duracion % 60) + " min";
}
