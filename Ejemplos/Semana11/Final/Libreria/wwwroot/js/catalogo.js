/* ===========================================================================
   ARCHIVO:   wwwroot/js/catalogo.js

   QUE ES:    El JavaScript propio del listado (/Libros). Llega a la pagina
              por la seccion Scripts de Views/Libros/Index.cshtml.

   POR QUE
   EXISTE:    Para comparar las dos formas de buscar:
                - el formulario "Buscar" viaja al SERVIDOR y recarga la pagina
                - este archivo filtra en el CLIENTE, al instante, sin pedir
                  nada al servidor
              Las dos sirven; la diferencia es DONDE se ejecuta el codigo.

   QUE SE
   HACE AQUI: Escuchar lo que se escribe en #filtro y esconder las tarjetas
              cuyo titulo o autor no coinciden. Los datos se leen de los
              atributos data-titulo y data-autor que Razor escribio en cada
              tarjeta (Views/Shared/_TarjetaLibro.cshtml).

   OJO:       Este filtro solo ve los libros que YA llegaron en el HTML. Si
              el catalogo tuviera 10 000 libros, no se mandarian todos: ahi
              conviene el buscador del servidor.
   =========================================================================== */

// 1. Buscar en el DOM los elementos con los que se va a trabajar.
const filtro   = document.querySelector("#filtro");
const contador = document.querySelector("#contador");
const tarjetas = document.querySelectorAll(".tarjeta");

// 2. Una funcion con nombre, igual que un metodo de C# pero sin tipos.
function aplicarFiltro() {
    const texto = filtro.value.trim().toLowerCase();
    let visibles = 0;                                  // let: variable que va a cambiar

    for (const tarjeta of tarjetas) {
        // dataset lee los atributos data-*: data-titulo -> dataset.titulo
        const coincide = tarjeta.dataset.titulo.includes(texto)
                      || tarjeta.dataset.autor.includes(texto);

        // toggle(clase, condicion): pone la clase si la condicion es true y
        // la quita si es false. Se OCULTA la tarjeta cuando NO coincide.
        tarjeta.classList.toggle("oculto", !coincide);

        if (coincide) visibles++;
    }

    // Template string: comillas invertidas y ${ } para insertar valores.
    // Es el equivalente de la interpolacion $"..." de C#.
    contador.textContent = `${visibles} de ${tarjetas.length} visibles`;
}

// 3. Evento: cada vez que cambia el texto del input, se vuelve a filtrar.
//    Si no hay filtro en la pagina (no hay libros), no se hace nada.
if (filtro) {
    filtro.addEventListener("input", aplicarFiltro);
    aplicarFiltro();                                   // estado inicial del contador
}
