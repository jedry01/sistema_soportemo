namespace sistema_soportemp.Models
{
    public class ReparacionEquiposViewModel
    {
        public int Id { get; set; }
        public int UbicacionId { get; set; }
        public IEnumerable<Ubicaciones> Ubicaciones { get; set; }

        public int ActivoId { get; set; }
        public IEnumerable<Activos> Activos { get; set; }

        public int MarcaId { get; set; }
        public IEnumerable<Marcas> Marcas { get; set; }

        public int ModeloId { get; set; }
        public IEnumerable<Modelos> Modelos { get; set; }

        public int TrabajadorId { get; set; }
        public IEnumerable<Trabajadores> Trabajadores { get; set; }

        public string Codigo { get; set; }
        public string Serie { get; set; }
        public string TrabajoRealizado { get; set; }
        public string Observaciones { get; set; }
        public DateTime Fecha { get; set; }
        public bool Entregado { get; set; }
    }

}
