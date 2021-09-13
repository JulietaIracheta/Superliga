namespace SuperligaChallange.Models
{
    public class EquipoEstadistica
    {
        public string Nombre { get; set; }
        public int CantidadSocios { get; set; }
        public int SumaEdad { get; set; }
        public int MenorEdad { get; set; }
        public int MayorEdad { get; set; }

        public float PromedioDeEdad()
        {
            var calculo = SumaEdad / CantidadSocios;
            return calculo;
        }
    }
}