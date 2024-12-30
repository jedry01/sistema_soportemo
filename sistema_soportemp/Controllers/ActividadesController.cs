// Controllers/ActividadesController.cs
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using sistema_soportemp.Models;
using Microsoft.EntityFrameworkCore;
using sistema_soportemp.Data;

public class ActividadesController : Controller
{
    private readonly AppDbContext _context;

    public ActividadesController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var actividades = _context.Actividades
            .Include(a => a.Trabajador)
            .Include(a => a.Ubicacion)
            .OrderBy(a => a.Fecha_programada)
            .ToList();
        return View(actividades);
    }

    public IActionResult Calendar()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Trabajadores = new SelectList(_context.Trabajadores, "Id", "Nombre");
        ViewBag.Ubicaciones = new SelectList(_context.Ubicaciones, "Id", "Nombre");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Actividades actividad)
    {
        if (ModelState.IsValid)
        {
            actividad.Creado_el = DateTime.Now;
            _context.Add(actividad);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Trabajadores = new SelectList(_context.Trabajadores, "Id", "Nombre");
        ViewBag.Ubicaciones = new SelectList(_context.Ubicaciones, "Id", "Nombre");
        return View(actividad);
    }

    [HttpPost]
    public async Task<IActionResult> ToggleComplete(int id)
    {
        var actividad = await _context.Actividades.FindAsync(id);
        if (actividad == null)
        {
            return NotFound();
        }

        actividad.Está_completado = !actividad.Está_completado;
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpGet]
    public JsonResult GetActividades()
    {
        var actividades = _context.Actividades
            .Include(a => a.Trabajador)
            .Include(a => a.Ubicacion)
            .Select(a => new
            {
                id = a.Id,
                title = a.Titulo,
                start = a.Fecha_programada,
                description = a.Description,
                trabajador = a.Trabajador.Nombre,
                ubicacion = a.Ubicacion.Nombre,
                completed = a.Está_completado
            })
            .ToList();
        return Json(actividades);
    }
}
