using Microsoft.AspNetCore.Mvc;
using sistema_soportemp.Data;
using sistema_soportemp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;

namespace sistema_soportemp.Controllers
{
    
    public class ReparacionEquiposController : Controller
    {
        private readonly AppDbContext _context;

        public ReparacionEquiposController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string buscar, int page = 1)
        {
            ViewData["Buscar"] = buscar;
            int pageSize = 10;

            var query = _context.ReparacionEquipos
                .Include(r => r.Ubicacion)
                .Include(r => r.Activo)
                .Include(r => r.Marca)
                .Include(r => r.Modelo)
                .Include(r => r.Trabajador)
                .AsQueryable();

            if (!string.IsNullOrEmpty(buscar))
            {
                query = query.Where(r => r.Codigo.Contains(buscar) ||
                                         r.Ubicacion.Nombre.Contains(buscar) ||
                                         r.Trabajador.Nombre.Contains(buscar));
            }

            query = query.OrderByDescending(r => r.Fecha);
            int totalItems = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var paginatedResult = new PagedResult<ReparacionEquipos>
            {
                Items = items,
                Page = page,
                TotalItems = totalItems,
                PageSize = pageSize
            };

            return View(paginatedResult);
        }

        [HttpGet("Create")]
        [Route("ReparacionEquipos/Create")]
        public async Task<IActionResult> Create()
        {
            var viewModel = new ReparacionEquiposViewModel
            {
                Ubicaciones = await _context.Ubicaciones.OrderBy(u => u.Nombre).ToListAsync(),
                Activos = await _context.Activos.OrderBy(a => a.Nombre).ToListAsync(),
                Marcas = await _context.Marcas.OrderBy(m => m.Nombre).ToListAsync(),
                Modelos = await _context.Modelos.OrderBy(m => m.Nombre).ToListAsync(),
                Trabajadores = await _context.Trabajadores.OrderBy(t => t.Nombre).ToListAsync()
            };

            return View(viewModel);
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReparacionEquipos reparacionEquipos)
        {
            if (ModelState.IsValid)
            {
                if (await ValidateForeignKeys(reparacionEquipos))
                {
                    _context.Add(reparacionEquipos);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Uno o más elementos seleccionados no existen en la base de datos.");
            }

            PopulateViewModelLists(reparacionEquipos);
            return View(reparacionEquipos);
        }

        private void PopulateViewModelLists(ReparacionEquipos reparacionEquipos)
        {
            throw new NotImplementedException();
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var reparacion = await GetReparacionEquiposById(id);
            if (reparacion == null) return NotFound();

            var viewModel = MapToViewModel(reparacion);
            await PopulateViewModelLists(viewModel);
            return View(viewModel);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReparacionEquiposViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await PopulateViewModelLists(viewModel);
                return View(viewModel);
            }

            var reparacion = await _context.ReparacionEquipos.FindAsync(id);
            if (reparacion == null) return NotFound();

            UpdateReparacionFromViewModel(reparacion, viewModel);
            _context.ReparacionEquipos.Update(reparacion);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var reparacionEquipo = await GetReparacionEquiposById(id);
            if (reparacionEquipo == null) return NotFound();

            return View(reparacionEquipo);
        }

        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reparacionEquipo = await _context.ReparacionEquipos.FindAsync(id);
            if (reparacionEquipo != null)
            {
                _context.ReparacionEquipos.Remove(reparacionEquipo);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("GetMarcasPorActivo/{activoId}")]
        public async Task<JsonResult> GetMarcasPorActivo(int activoId)
        {
            var marcas = await _context.Marcas
                                       .Where(m => m.ActivoId == activoId)
                                       .Select(m => new { m.Id, m.Nombre })
                                       .ToListAsync();
            return Json(marcas);
        }

        [HttpGet("GetModelosPorMarcaYActivo")]
        public async Task<JsonResult> GetModelosPorMarcaYActivo(int marcaId, int activoId)
        {
            var modelos = await _context.Modelos
                                         .Where(m => m.MarcaId == marcaId && m.ActivoId == activoId)
                                         .Select(m => new { m.Id, m.Nombre })
                                         .ToListAsync();
            return Json(modelos);
        }

        private async Task<bool> ValidateForeignKeys(ReparacionEquipos reparacionEquipos)
        {
            return await _context.Ubicaciones.AnyAsync(u => u.Id == reparacionEquipos.UbicacionId) &&
                   await _context.Activos.AnyAsync(a => a.Id == reparacionEquipos.ActivoId) &&
                   await _context.Marcas.AnyAsync(m => m.Id == reparacionEquipos.MarcaId) &&
                   await _context.Modelos.AnyAsync(mo => mo.Id == reparacionEquipos.ModeloId) &&
                   await _context.Trabajadores.AnyAsync(t => t.Id == reparacionEquipos.TrabajadorId);
        }

        private async Task PopulateViewModelLists(ReparacionEquiposViewModel viewModel)
        {
            viewModel.Ubicaciones = await _context.Ubicaciones.OrderBy(u => u.Nombre).ToListAsync();
            viewModel.Activos = await _context.Activos.OrderBy(a => a.Nombre).ToListAsync();
            viewModel.Marcas = await _context.Marcas.OrderBy(m => m.Nombre).ToListAsync();
            viewModel.Modelos = await _context.Modelos.OrderBy(m => m.Nombre).ToListAsync();
            viewModel.Trabajadores = await _context.Trabajadores.OrderBy(t => t.Nombre).ToListAsync();
        }

        private async Task<ReparacionEquipos> GetReparacionEquiposById(int id)
        {
            return await _context.ReparacionEquipos
                .Include(r => r.Ubicacion)
                .Include(r => r.Activo)
                .Include(r => r.Marca)
                .Include(r => r.Modelo)
                .Include(r => r.Trabajador)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        private ReparacionEquiposViewModel MapToViewModel(ReparacionEquipos reparacion)
        {
            return new ReparacionEquiposViewModel
            {
                Id = reparacion.Id,
                Codigo = reparacion.Codigo,
                Serie = reparacion.Serie,
                TrabajoRealizado = reparacion.TrabajoRealizado,
                Observaciones = reparacion.Observaciones,
                Fecha = reparacion.Fecha,
                Entregado = reparacion.Entregado,
                UbicacionId = reparacion.UbicacionId,
                ActivoId = reparacion.ActivoId,
                MarcaId = reparacion.MarcaId,
                ModeloId = reparacion.ModeloId,
                TrabajadorId = reparacion.TrabajadorId
            };
        }

        private void UpdateReparacionFromViewModel(ReparacionEquipos reparacion, ReparacionEquiposViewModel viewModel)
        {
            reparacion.Codigo = viewModel.Codigo;
            reparacion.Serie = viewModel.Serie;
            reparacion.TrabajoRealizado = viewModel.TrabajoRealizado;
            reparacion.Observaciones = viewModel.Observaciones;
            reparacion.Fecha = viewModel.Fecha;
            reparacion.Entregado = viewModel.Entregado;
            reparacion.UbicacionId = viewModel.UbicacionId;
            reparacion.ActivoId = viewModel.ActivoId;
            reparacion.MarcaId = viewModel.MarcaId;
            reparacion.ModeloId = viewModel.ModeloId;
            reparacion.TrabajadorId = viewModel.TrabajadorId;
        }
    }
}

