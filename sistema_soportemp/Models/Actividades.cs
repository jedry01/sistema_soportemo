// Models/Activity.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace sistema_soportemp.Models
{
    public class Actividades
    {
        public int Id { get; set; }

        [Required]
        public string Titulo { get; set; }

        [Required]
        public DateTime Fecha_programada { get; set; }

        [Required]
        public int TrabajadorId { get; set; }
        public Trabajadores Trabajador { get; set; }

        [Required]
        public int UbicacionId { get; set; }
        public Ubicaciones Ubicacion { get; set; }

        public string Description { get; set; }

        public bool Está_completado { get; set; }

        public DateTime Creado_el { get; set; } = DateTime.Now;
    }
}