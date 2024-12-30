namespace sistema_soportemp.Models
{
    public class LineChartDataModel
    {
        public List<string> Labels { get; set; } = new List<string>(); // Etiquetas (deben ser strings)
        public List<int> Values { get; set; } = new List<int>();       // Valores (deben ser enteros)
    }

}