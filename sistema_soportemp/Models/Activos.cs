using System.Text.RegularExpressions;

namespace sistema_soportemp.Models
{
    public class Activos
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        // Relación uno a muchos con Marca
        public virtual ICollection<Marcas> Marcas { get; set; }

        // Relación uno a muchos con Modelo
        public virtual ICollection<Modelos> Modelos { get; set; }
    }

}
