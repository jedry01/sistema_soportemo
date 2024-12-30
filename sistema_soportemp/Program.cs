using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using sistema_soportemp.Data;
using sistema_soportemp.Models;

namespace sistema_soportemp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Agregar servicios al contenedor
            builder.Services.AddControllersWithViews();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Configuración de Identity para ApplicationUser
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
            })
               .AddEntityFrameworkStores<AppDbContext>()
               .AddDefaultTokenProviders();

            // Definir políticas de autorización
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrador", policy => policy.RequireRole("Administrador"));
                options.AddPolicy("Supervisor", policy => policy.RequireRole("Supervisor"));
                options.AddPolicy("Tecnico", policy => policy.RequireRole("Técnico"));
                options.AddPolicy("Operador", policy => policy.RequireRole("Operador"));
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // Crear roles
                foreach (var rol in Roles.TodosLosRoles)
                {
                    if (!await roleManager.RoleExistsAsync(rol))
                    {
                        await roleManager.CreateAsync(new IdentityRole(rol));
                    }
                }

                // Crear usuario administrador
                var userName = "admin";
                if (await userManager.FindByNameAsync(userName) == null)
                {
                    var usuario = new ApplicationUser
                    {
                        UserName = userName,
                        Email = "admin@ejemplo.com",
                        Nombre = "Admin",
                        Apellido = "Sistema",
                        Cargo = "Administrador del Sistema",
                        EmailConfirmed = true
                    };

                    var resultado = await userManager.CreateAsync(usuario, "Admin123!");
                    if (resultado.Succeeded)
                    {
                        await userManager.AddToRoleAsync(usuario, Roles.Administrador);
                    }
                }
            }

            // Configurar el pipeline de solicitudes HTTP
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
