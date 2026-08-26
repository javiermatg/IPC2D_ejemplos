namespace ChapinCine.Modelo
{
    /// <summary>
    /// Asiento ya vendido, correspondiente al carácter 'O' del archivo
    /// de configuración. No es reservable, pero ocupa una posición en la
    /// malla y participa de los enlaces como cualquier otro nodo.
    /// </summary>
    public class AsientoOcupado : Asiento
    {
        public override char Simbolo
        {
            get { return 'O'; }
        }

        public override string ObtenerTipo()
        {
            return "Ocupado";
        }

        public override bool EsReservablePor(Cliente cliente)
        {
            return false;
        }
    }
}
