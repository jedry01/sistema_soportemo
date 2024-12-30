namespace sistema_soportemp.Models


{
    public class DashboardViewModel
    {
        public int TotalEquiposReparados { get; set; }
        public int TotalEquiposEnEspera { get; set; }
        public int TotalEquiposEntregados { get; set; }
        public int TotalEquipos { get; set; }
        public DateTime? EndDate { get; internal set; }
        public DateTime? StartDate { get; internal set; }

        // Filtros
        public List<Trabajadores> Tecnicos { get; set; }  // Lista de técnicos para el filtro
        public int? SelectedTecnicoId { get; set; }     // Técnico seleccionado

        public ChartDataModel ChartData { get; set; }
        public LineChartDataModel LineChartData { get; set; } // Usando el modelo desde sistema_soportemp.Models

    }
}

