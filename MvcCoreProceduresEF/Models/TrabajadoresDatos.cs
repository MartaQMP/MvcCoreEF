namespace MvcCoreProceduresEF.Models
{
    public class TrabajadoresDatos
    {
        public int Personas { get; set; }
        public int MediaSalarial { get; set; }
        public int SumaSalarial { get; set; }
        public List<Trabajador> Trabajadores { get; set; }
    }
}
