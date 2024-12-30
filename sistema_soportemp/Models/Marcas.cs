using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace sistema_soportemp.Models
{
    public class Marcas
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int ActivoId { get; set; }

        // Propiedad de navegación y clave foránea para Activo
        [ForeignKey("ActivoId")]
        public virtual Activos Activo { get; set; }

        // Relación uno a muchos con ReparacionEquipos
        public virtual ICollection<ReparacionEquipos> ReparacionEquipos { get; set; }

        // Relación uno a muchos con Modelo
        public virtual ICollection<Modelos> Modelos { get; set; }
    }

}
