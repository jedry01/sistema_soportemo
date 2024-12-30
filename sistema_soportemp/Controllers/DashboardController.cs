using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using sistema_soportemp.Data;
using Microsoft.EntityFrameworkCore;
using sistema_soportemp.Models;
using System.Text.RegularExpressions;

namespace sistema_soporte.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate, int? tecnicoId)
        {
            // Consultar las estadísticas para el rango de fechas proporcionado y el técnico seleccionado
            var queryReparacionEquipos = _context.ReparacionEquipos.AsQueryable();
            var queryRepuestosEquipos = _context.RepuestosEquipos.AsQueryable();

            // Filtros por fechas
            if (startDate.HasValue)
                queryReparacionEquipos = queryReparacionEquipos.Where(e => e.Fecha >= startDate.Value);

            if (endDate.HasValue)
                queryReparacionEquipos = queryReparacionEquipos.Where(e => e.Fecha <= endDate.Value);

            // Filtro por técnico
            if (tecnicoId.HasValue)
            {
                queryReparacionEquipos = queryReparacionEquipos.Where(e => e.TrabajadorId == tecnicoId.Value);
            }

            // Obtener estadísticas
            var totalEquiposReparados = await queryReparacionEquipos.CountAsync(e => e.Entregado == true);
            var totalEquiposEnEspera = await queryRepuestosEquipos.CountAsync(e => e.Comprado == true);
            var totalEquiposEntregados = await queryReparacionEquipos.CountAsync(e => e.Entregado == true);
            var totalEquipos = await queryReparacionEquipos.CountAsync();

            // Preparar los datos para el gráfico
            var chartData = new ChartDataModel
            {
                Labels = new[] { "Equipos Reparados", "Equipos en Espera", "Equipos Entregados", "Equipos Totales" },
                Values = new[] { totalEquiposReparados, totalEquiposEnEspera, totalEquiposEntregados, totalEquipos }
            };

            var dashboardViewModel = new DashboardViewModel
            {
                TotalEquiposReparados = totalEquiposReparados,
                TotalEquiposEnEspera = totalEquiposEnEspera,
                TotalEquiposEntregados = totalEquiposEntregados,
                TotalEquipos = totalEquipos,
                StartDate = startDate,
                EndDate = endDate,
                Tecnicos = await _context.Trabajadores.ToListAsync(),  // Lista de técnicos
                SelectedTecnicoId = tecnicoId,
                ChartData = chartData // Asignación correcta del tipo ChartDataModel
            };

            //hasta aqui bueno//


            // Obtener el número de reparaciones por mes
            var reparacionesPorMes = await _context.ReparacionEquipos
                .Where(r => r.Entregado == true)
                .GroupBy(r => new { r.Fecha.Year, r.Fecha.Month })
                .Select(g => new
                {
                    Año = g.Key.Year,
                    Mes = g.Key.Month,
                    TotalReparaciones = g.Count()
                })
                .OrderBy(g => g.Año).ThenBy(g => g.Mes)
                .ToListAsync();

            var totalAcumuladoReparaciones = reparacionesPorMes
                .Select((r, index) => reparacionesPorMes.Take(index + 1).Sum(x => x.TotalReparaciones))
                .ToList();

            // Convertir los resultados a un formato adecuado para el gráfico
            var meses = reparacionesPorMes.Select(r => $"{r.Año}-{r.Mes:00}").ToList();
            var totalReparaciones = reparacionesPorMes.Select(r => r.TotalReparaciones).ToList();

            // Pasar los datos para el gráfico a la vista
            ViewData["Meses"] = string.Join(",", meses);
            ViewData["TotalReparaciones"] = string.Join(",", totalReparaciones);

            // Reparaciones por trabajador
            var reparacionesPorTrabajador = await _context.ReparacionEquipos
                .Where(r => r.Entregado == true)
                .GroupBy(r => r.Trabajador.Nombre)
                .Select(g => new
                {
                    NombreTrabajador = g.Key,
                    TotalReparaciones = g.Count()
                })
                .OrderByDescending(g => g.TotalReparaciones)
                .Take(10)  // Get top 10 workers
                .ToListAsync();

            ViewData["NombresTrabajadores"] = string.Join(",", reparacionesPorTrabajador.Select(r => r.NombreTrabajador));
            ViewData["ReparacionesPorTrabajador"] = string.Join(",", reparacionesPorTrabajador.Select(r => r.TotalReparaciones));

            // Reparaciones por Ubicacion
            var reparacionesPorUbicacion = await _context.ReparacionEquipos
                .Where(r => r.Entregado == true)
                .GroupBy(r => r.Ubicacion.Nombre) // Asumiendo que Ubicacion tiene una propiedad Nombre
                .Select(g => new
                {
                    Ubicacion = g.Key,
                    TotalReparaciones = g.Count()
                })
                .OrderByDescending(g => g.TotalReparaciones)
                .Take(10)  // Get top 10 locations
                .ToListAsync();

            ViewData["Ubicaciones"] = string.Join(",", reparacionesPorUbicacion.Select(r => r.Ubicacion));
            ViewData["ReparacionesPorUbicacion"] = string.Join(",", reparacionesPorUbicacion.Select(r => r.TotalReparaciones));

            return View(dashboardViewModel);
        }

    }
}
