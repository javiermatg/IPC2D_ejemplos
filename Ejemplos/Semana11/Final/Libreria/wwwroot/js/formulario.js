/* ===========================================================================
   ARCHIVO:   wwwroot/js/formulario.js

   QUE ES:    Validacion del formulario "Agregar libro" en el NAVEGADOR.
              Llega por la seccion Scripts de Views/Libros/Nueva.cshtml.

   POR QUE
   EXISTE:    Para ahorrarle al usuario un viaje al servidor: si falta el
              titulo, se le avisa al instante, sin recargar la pagina.

   OJO, MUY
   IMPORTANTE: Esta validacion es una CORTESIA, no una proteccion. Cualquiera
              puede desactivar JavaScript o enviar datos con otra herramienta.
              Por eso LibrosController.Nueva sigue revisando ModelState: esa
              es la validacion que cuenta.
   =========================================================================== */

const formulario   = document.querySelector("#form-libro");
const titulo       = document.querySelector("#Titulo");        // id que puso asp-for
const cuentaTitulo = document.querySelector("#cuenta-titulo");

// ----- Contador de caracteres del titulo -----------------------------------
// Funcion flecha: forma corta de escribir una funcion, como las lambdas de C#.
const actualizarCuenta = () => {
    cuentaTitulo.textContent = `${titulo.value.length} / 80`;
};

titulo.addEventListener("input", actualizarCuenta);
actualizarCuenta();

// ----- Revisar antes de enviar ---------------------------------------------
// Devuelve un texto con el problema, o una cadena vacia si todo esta bien.
function revisar(campo) {
    const valor = campo.value.trim();

    if (["Isbn", "Titulo", "Autor"].includes(campo.id) && valor === "") {
        return "Este campo es obligatorio.";
    }
    if (campo.id === "Precio" && (Number(valor) < 1 || Number(valor) > 3000)) {
        return "El precio debe estar entre Q1 y Q3000.";
    }
    if (campo.id === "Existencias" && Number(valor) < 0) {
        return "Las existencias no pueden ser negativas.";
    }
    return "";
}

// El evento submit ocurre justo antes de que el navegador envie el POST.
formulario.addEventListener("submit", (evento) => {
    let hayErrores = false;

    // Se limpian los mensajes de un intento anterior.
    document.querySelectorAll(".error-cliente").forEach(m => m.remove());

    for (const campo of formulario.querySelectorAll("input[id]")) {
        const problema = revisar(campo);
        if (problema !== "") {
            hayErrores = true;

            // Crear un elemento nuevo en el DOM y colocarlo despues del input.
            const mensaje = document.createElement("span");
            mensaje.className = "error-cliente";
            mensaje.textContent = problema;
            campo.insertAdjacentElement("afterend", mensaje);
        }
    }

    // preventDefault() CANCELA el envio: el POST no sale y la pagina no se
    // recarga. Solo se cancela si hubo errores.
    if (hayErrores) {
        evento.preventDefault();
    }
});
