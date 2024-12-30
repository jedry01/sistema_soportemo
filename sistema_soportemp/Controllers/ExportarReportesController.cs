using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using iTextSharp.text;
using iTextSharp.text.pdf;
using sistema_soportemp.Data; // Cambia según tu espacio de nombres
using sistema_soportemp.Models; // Cambia según tus modelos
using Microsoft.EntityFrameworkCore;

namespace sistema_soportemp.Controllers
{
    public class ExportarReportesController : Controller
    {
        private readonly AppDbContext _context;

        public ExportarReportesController(AppDbContext context)
        {
            _context = context;
        }

        // Exportar a Excel usando EPPlus
        public async Task<IActionResult> ExportarReparacionEquiposExcel()
        {
            var equiposReparados = await _context.ReparacionEquipos
                .Include(r => r.Activo)
                .Include(r => r.Ubicacion)
                .ToListAsync();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("ReparacionEquipos");
                worksheet.Cells[1, 1].Value = "Código";
                worksheet.Cells[1, 2].Value = "Activo";
                worksheet.Cells[1, 3].Value = "Fecha";
                worksheet.Cells[1, 4].Value = "Ubicación";

                int row = 2;
                foreach (var equipo in equiposReparados)
                {
                    worksheet.Cells[row, 1].Value = equipo.Codigo;
                    worksheet.Cells[row, 2].Value = equipo.Activo;
                    worksheet.Cells[row, 3].Value = equipo.Fecha.ToString("dd/MM/yyyy");
                    worksheet.Cells[row, 4].Value = equipo.Ubicacion.Nombre; // Ajusta según tu modelo
                    row++;
                }

                var stream = new MemoryStream(package.GetAsByteArray());
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReparacionEquipos.xlsx");
            }
        }

        // Exportar a PDF usando iTextSharp
        public async Task<IActionResult> ExportarReparacionEquiposPdf()
        {
            var equiposReparados = await _context.ReparacionEquipos
                .Include(r => r.Activo)
                .Include(r => r.Ubicacion)
                .ToListAsync();

            using (var stream = new MemoryStream())
            {
                var document = new Document(PageSize.A4);
                var writer = PdfWriter.GetInstance(document, stream);
                document.Open();

                document.Add(new Paragraph("Reporte de Reparación de Equipos") { Alignment = Element.ALIGN_CENTER });
                document.Add(new Paragraph("\n"));

                foreach (var equipo in equiposReparados)
                {
                    document.Add(new Paragraph($"Código: {equipo.Codigo}"));
                    document.Add(new Paragraph($"Activo: {equipo.Activo}"));
                    document.Add(new Paragraph($"Fecha: {equipo.Fecha:dd/MM/yyyy}"));
                    document.Add(new Paragraph($"Ubicación: {equipo.Ubicacion.Nombre}"));
                    document.Add(new Paragraph("\n"));
                }

                document.Close();
                stream.Position = 0;

                return File(stream.ToArray(), "application/pdf", "ReparacionEquipos.pdf");
            }
        }
    }
}
