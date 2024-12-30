using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using sistema_soportemp.Models;

namespace sistema_soportemp.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Ubicaciones> Ubicaciones { get; set; }
        public DbSet<Activos> Activos { get; set; }
        public DbSet<Trabajadores> Trabajadores { get; set; }
        public DbSet<Marcas> Marcas { get; set; }
        public DbSet<Modelos> Modelos { get; set; }
        public DbSet<ReparacionEquipos> ReparacionEquipos { get; set; }
        public DbSet<RepuestosEquipos> RepuestosEquipos { get; set; }
        public DbSet<Actividades> Actividades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relación entre Activo y Marca
            modelBuilder.Entity<Marcas>()
                .HasOne(m => m.Activo)
                .WithMany(a => a.Marcas)
                .HasForeignKey(m => m.ActivoId)
                .OnDelete(DeleteBehavior.Restrict);  // Evitar eliminación en cascada

            // Relación entre Marca y Modelo
            modelBuilder.Entity<Modelos>()
                .HasOne(mo => mo.Marca)
                .WithMany(m => m.Modelos)
                .HasForeignKey(mo => mo.MarcaId)
                .OnDelete(DeleteBehavior.Restrict);  // Evitar eliminación en cascada

            // Relación entre Modelo y Activo
            modelBuilder.Entity<Modelos>()
                .HasOne(mo => mo.Activo)
                .WithMany(a => a.Modelos)
                .HasForeignKey(mo => mo.ActivoId)
                .OnDelete(DeleteBehavior.Restrict);  // Evitar eliminación en cascada
        }
    }
}
