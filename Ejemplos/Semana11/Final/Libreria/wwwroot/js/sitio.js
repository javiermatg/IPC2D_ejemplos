/* ===========================================================================
   ARCHIVO:   wwwroot/js/sitio.js

   QUE ES:    El JavaScript COMUN del sitio. Lo carga el _Layout en todas las
              paginas, con defer.

   POR QUE
   EXISTE:    Para el primer ejemplo de programacion del lado del CLIENTE:
              este codigo no lo ejecuta ASP.NET Core, lo ejecuta el
              navegador del usuario, despues de recibir el HTML.

   QUE SE
   HACE AQUI: Marcar en el menu el enlace de la seccion en la que esta el
              usuario. El servidor podria hacerlo con Razor; se hace aqui para
              ver el DOM en accion con muy poco codigo.

   COMO
   PROBARLO:  F12 > Consola. Debe aparecer el mensaje de abajo. Si no
              aparece, revise la pestana Red: el archivo debe responder 200.
   =========================================================================== */

console.log("sitio.js cargado desde wwwroot/js");

// location.pathname es la parte de la URL despues del dominio: "/Libros/Nueva"
const rutaActual = location.pathname.toLowerCase();

// querySelectorAll devuelve TODOS los elementos que cumplen el selector CSS.
// Es el mismo selector que se usaria en estilos.css.
const enlaces = document.querySelectorAll(".menu a");

for (const enlace of enlaces) {
    // getAttribute lee el href que genero el tag helper en el servidor.
    const destino = enlace.getAttribute("href").toLowerCase();

    // === compara valor y tipo. En JavaScript casi siempre se usa === y no ==.
    if (rutaActual === destino) {
        enlace.classList.add("activo");       // agrega una clase CSS; el estilo lo pone el CSS
    }
}
