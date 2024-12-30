using Microsoft.AspNetCore.Mvc;
using sistema_soportemp.Models;
using sistema_soportemp.Data;
using Microsoft.EntityFrameworkCore;

namespace sistema_soportemp.Controllers
{
    public class RepuestosEquiposController : Controller
    {
        private readonly AppDbContext _context;

        public RepuestosEquiposController(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index(string buscar, int page = 1)
        {
            // Guardar el valor de búsqueda en ViewData
            ViewData["Buscar"] = buscar;

            // Definir el número de elementos por página
            int pageSize = 10;

            // Crear la consulta inicial, incluyendo las relaciones necesarias
            var query = _context.RepuestosEquipos
                .Include(r => r.Ubicacion)  // Relación con Ubicación
                .Include(r => r.Marca)      // Relación con Marca
                .Include(r => r.Modelo)     // Relación con Modelo
                .Include(r => r.Activo)     // Relación con Activo
                .OrderByDescending(r => r.Fecha) // Ordenar por fecha descendente
                .AsQueryable();

            // Filtro opcional: Buscar por campos específicos
            if (!string.IsNullOrEmpty(buscar))
            {
                query = query.Where(r => r.Codigo.Contains(buscar) ||
                                         r.Ubicacion.Nombre.Contains(buscar) ||
                                         r.Marca.Nombre.Contains(buscar));
            }

            // Obtener el número total de elementos después de aplicar el filtro
            int totalItems = await query.CountAsync();

            // Obtener los elementos de la página actual
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Crear el modelo de paginación
            var paginatedResult = new PagedResult<RepuestosEquipos>
            {
                Items = items,
                Page = page,
                TotalItems = totalItems,
                PageSize = pageSize
            };

            // Pasar los datos a la vista
            return View(paginatedResult);
        }

        // GET: RepuestosEquipos/Create
        public IActionResult Create()
        {
            var viewModel = new RepuestosEquiposViewModel
            {
                Marcas = _context.Marcas.ToList(),
                Modelos = _context.Modelos.ToList(),
                Ubicaciones = _context.Ubicaciones.ToList(),
                Activos = _context.Activos.ToList(),
                //Trabajadores = _context.Trabajadores.ToList()
            };

            return View(viewModel);
        }

        // POST: RepuestosEquipos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RepuestosEquiposViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var repuesto = new RepuestosEquipos
                {
                    Codigo = viewModel.Codigo,                   
                    Repuesto = viewModel.Repuesto,
                    Observaciones = viewModel.Observaciones ?? "",
                    Fecha = viewModel.Fecha,
                    Comprado = viewModel.Comprado,
                    UbicacionId = viewModel.UbicacionId,
                    ActivoId = viewModel.ActivoId,
                    MarcaId = viewModel.MarcaId,
                    ModeloId = viewModel.ModeloId,
                    TrabajadorId = viewModel.TrabajadorId 
                };

                _context.Add(repuesto);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index)); // Redirigir a la vista Index después de guardar
            }

            // Si no es válido, volver a mostrar el formulario con los datos actuales
            viewModel.Marcas = _context.Marcas.ToList();
            viewModel.Modelos = _context.Modelos.ToList();
            viewModel.Ubicaciones = _context.Ubicaciones.ToList();
            viewModel.Activos = _context.Activos.ToList();
            //viewModel.Trabajadores = _context.Trabajadores.ToList();

            return View(viewModel);
        }

        // Acción GET: Edit
        public async Task<IActionResult> Edit(int id)
        {
            var repuestoEquipos = await _context.RepuestosEquipos
                .Include(r => r.Ubicacion)
                .Include(r => r.Activo)
                .Include(r => r.Marca)
                .Include(r => r.Modelo)
                .Include(r => r.Trabajador)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (repuestoEquipos == null)
            {
                return NotFound();
            }

            var viewModel = new RepuestosEquiposViewModel
            {
                Id = repuestoEquipos.Id,
                Codigo = repuestoEquipos.Codigo,
                //Serie = repuestoEquipos.Serie,
                Repuesto = repuestoEquipos.Repuesto,
                //Observaciones = repuestoEquipos.Observaciones,
                Fecha = repuestoEquipos.Fecha,
                Comprado = repuestoEquipos.Comprado,
                UbicacionId = repuestoEquipos.UbicacionId,
                ActivoId = repuestoEquipos.ActivoId,
                MarcaId = repuestoEquipos.MarcaId,
                ModeloId = repuestoEquipos.ModeloId,
                //TrabajadorId = repuestoEquipos.TrabajadorId,
                Ubicaciones = await _context.Ubicaciones.ToListAsync(),
                Activos = await _context.Activos.ToListAsync(),
                Marcas = await _context.Marcas.ToListAsync(),
                Modelos = await _context.Modelos.ToListAsync(),
                //Trabajadores = await _context.Trabajadores.ToListAsync()
            };

            return View(viewModel);
        }

        // Acción POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RepuestosEquiposViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var repuestoEquipos = await _context.RepuestosEquipos.FindAsync(id);

                if (repuestoEquipos == null)
                {
                    return NotFound();
                }

                repuestoEquipos.Codigo = viewModel.Codigo;
                //repuestoEquipos.Serie = viewModel.Serie;
                repuestoEquipos.Repuesto = viewModel.Repuesto;
                //repuestoEquipos.Observaciones = viewModel.Observaciones;
                repuestoEquipos.Fecha = viewModel.Fecha;
                repuestoEquipos.Comprado = viewModel.Comprado;
                repuestoEquipos.UbicacionId = viewModel.UbicacionId;
                repuestoEquipos.ActivoId = viewModel.ActivoId;
                repuestoEquipos.MarcaId = viewModel.MarcaId;
                repuestoEquipos.ModeloId = viewModel.ModeloId;
                //repuestoEquipos.TrabajadorId = viewModel.TrabajadorId;

                try
                {
                    _context.Update(repuestoEquipos);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.RepuestosEquipos.Any(r => r.Id == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // Si llegamos aquí, significa que hubo un error en la validación, así que volvemos a cargar los datos.
            viewModel.Ubicaciones = await _context.Ubicaciones.ToListAsync();
            viewModel.Activos = await _context.Activos.ToListAsync();
            viewModel.Marcas = await _context.Marcas.ToListAsync();
            viewModel.Modelos = await _context.Modelos.ToListAsync();
            //viewModel.Trabajadores = await _context.Trabajadores.ToListAsync();

            return View(viewModel);
        }

        // GET: RepuestosEquipos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repuestoEquipo = await _context.RepuestosEquipos
                .Include(r => r.Ubicacion)
                .Include(r => r.Activo)
                .Include(r => r.Marca)
                .Include(r => r.Modelo)
                .Include(r => r.Trabajador)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (repuestoEquipo == null)
            {
                return NotFound();
            }

            return View(repuestoEquipo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var repuestoEquipo = await _context.RepuestosEquipos.FindAsync(id);
            if (repuestoEquipo != null)
            {
                _context.RepuestosEquipos.Remove(repuestoEquipo);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
