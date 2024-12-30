

using Microsoft.AspNetCore.Identity;

namespace sistema_soportemp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cargo { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}
