using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace sistema_soportemp.Models
{
    public class RepuestosEquipos
    {
        public int Id { get; set; }
        public int UbicacionId { get; set; }  // Llave foránea
        public Ubicaciones Ubicacion { get; set; }  // Propiedad de navegación

        public int ActivoId { get; set; }
        public Activos Activo { get; set; }


        public int MarcaId { get; set; }
        [ForeignKey("MarcaId")]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public Marcas Marca { get; set; }

        public int ModeloId { get; set; }
        [ForeignKey("ModeloId")]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public Modelos Modelo { get; set; }

        public int? TrabajadorId { get; set; }
        public Trabajadores Trabajador { get; set; }

        public string Codigo { get; set; }
      
        public string Repuesto { get; set; }
        public string Observaciones { get; set; }
        public DateTime Fecha { get; set; }
        public bool Comprado { get; set; }
    }
}
