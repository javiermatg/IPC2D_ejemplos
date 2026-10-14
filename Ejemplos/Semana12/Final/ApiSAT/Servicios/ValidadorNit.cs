// ===========================================================================
//  ARCHIVO:   Servicios/ValidadorNit.cs  (ApiSAT)
//
//  QUE ES:    La regla que decide si un NIT es valido.
//
//  POR QUE
//  EXISTE:    El RTU solo acepta NITs validos, y en las autorizaciones hay que
//             contar "NIT emisor invalido" y "NIT receptor invalido". La misma
//             regla se usa en dos lugares: por eso vive en su propia clase y
//             no copiada dentro de cada controlador.
//
//  QUE SE
//  HACE AQUI: 1) Revisar la FORMA con una expresion regular.
//             2) Revisar el DIGITO VERIFICADOR con el algoritmo modulo 11.
//
//  EL ALGORITMO (del enunciado), con el NIT 7378106:
//
//      NIT:       7   3   7   8   1   0   6   <- el ultimo es el verificador
//      posicion:  7   6   5   4   3   2   (1) <- la 1 es la de mas a la derecha
//      producto: 49  18  35  32   3   0       <- la posicion 1 no se multiplica
//
//      suma = 137     137 % 11 = 5     11 - 5 = 6     6 % 11 = 6
//      el verificador calculado es 6 y el NIT termina en 6  -> VALIDO
//
//      Si el resultado final es 10, el verificador debe ser la letra K.
//
//  OJO:       5123456, que viene en el ejemplo del enunciado, NO es valido:
//             su verificador deberia ser 3. Es un buen caso para probar.
// ===========================================================================

using System.Text.RegularExpressions;

namespace ApiSAT.Servicios
{
    public static class ValidadorNit
    {
        // De 1 a 20 digitos y al final un digito o la letra K.
        //   ^         inicio del texto
        //   [0-9]{1,20}  entre 1 y 20 digitos
        //   [0-9K]    un caracter: digito o K
        //   $         fin del texto (no puede sobrar nada)
        private static readonly Regex Formato = new Regex(@"^[0-9]{1,20}[0-9K]$");

        // Deja el NIT en una forma unica para comparar y guardar:
        // sin espacios, sin guion y con la K en mayuscula.
        // Asi "737810-6", " 7378106 " y "7378106" son el mismo NIT.
        public static string Normalizar(string nit)
        {
            if (nit == null)
            {
                return "";
            }
            return nit.Trim().Replace("-", "").ToUpper();
        }

        // Calcula el verificador que DEBERIA tener un NIT, a partir de los
        // digitos sin el verificador. Ejemplo: "737810" -> '6'.
        public static char CalcularVerificador(string digitos)
        {
            int suma = 0;
            int posicion = 2;    // la posicion 1 es el verificador, que no entra en la suma

            // Se recorre de derecha a izquierda.
            for (int i = digitos.Length - 1; i >= 0; i--)
            {
                int digito = digitos[i] - '0';    // '7' - '0' = 7
                suma += digito * posicion;
                posicion++;
            }

            int resultado = (11 - (suma % 11)) % 11;

            if (resultado == 10)
            {
                return 'K';
            }
            return (char)('0' + resultado);       // 6 -> '6'
        }

        public static bool EsValido(string nit)
        {
            string limpio = Normalizar(nit);

            // 1) La forma: si no cumple, ni siquiera se calcula.
            if (!Formato.IsMatch(limpio))
            {
                return false;
            }

            // 2) El verificador.
            string digitos = limpio.Substring(0, limpio.Length - 1);
            char verificador = limpio[limpio.Length - 1];

            return CalcularVerificador(digitos) == verificador;
        }
    }
}
