// ===========================================================================
//  ARCHIVO:   Servicios/RegistroTributario.cs  (ApiSAT)
//
//  QUE ES:    El TDA del RTU: la coleccion de contribuyentes en memoria y las
//             operaciones que tiene sentido hacerle.
//
//  POR QUE
//  EXISTE:    El enunciado pide TDAs propios. En este proyecto SI se permite
//             List<T>, pero el resto del programa no toca la lista
//             directamente: solo usa Buscar, AgregarOActualizar, Todos y
//             Limpiar. Si manana se cambia la List por un arbol o un
//             Dictionary, solo cambia esta clase.
//
//  QUE SE
//  HACE AQUI: Nada de HTTP ni de archivos. Solo la estructura de datos.
// ===========================================================================

using ApiSAT.Models;

namespace ApiSAT.Servicios
{
    public class RegistroTributario
    {
        private readonly List<Contribuyente> contribuyentes;

        public RegistroTributario(List<Contribuyente> iniciales)
        {
            // Si el archivo no existia, se empieza con un RTU vacio.
            contribuyentes = iniciales ?? new List<Contribuyente>();
        }

        public int Cantidad
        {
            get { return contribuyentes.Count; }
        }

        // Busqueda lineal por NIT. Devuelve null si no existe.
        public Contribuyente Buscar(string nit)
        {
            foreach (Contribuyente c in contribuyentes)
            {
                if (c.Nit == nit)
                {
                    return c;
                }
            }
            return null;
        }

        // Regla del enunciado:
        //   NIT valido que NO existe -> se crea      (devuelve true)
        //   NIT valido que YA existe -> se actualiza (devuelve false)
        // La validacion del NIT se hace ANTES de llamar a este metodo.
        public bool AgregarOActualizar(Contribuyente nuevo)
        {
            Contribuyente existente = Buscar(nuevo.Nit);

            if (existente == null)
            {
                contribuyentes.Add(nuevo);
                return true;
            }

            existente.Nombre = nuevo.Nombre;
            return false;
        }

        // Devuelve una COPIA de la lista: quien la reciba no puede
        // modificar el RTU por fuera de este TDA.
        public List<Contribuyente> Todos()
        {
            return new List<Contribuyente>(contribuyentes);
        }

        public void Limpiar()
        {
            contribuyentes.Clear();
        }
    }
}
