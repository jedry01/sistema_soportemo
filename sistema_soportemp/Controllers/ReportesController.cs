using Microsoft.AspNetCore.Mvc;
using sistema_soportemp.Data;
using sistema_soportemp.Models;
using Microsoft.EntityFrameworkCore;
using iTextSharp.text.pdf;
using iTextSharp.text;
using OfficeOpenXml;

namespace sistema_soportemp.Controllers
{
    public class ReportesController : Controller
    {
        private readonly AppDbContext _context;

        public ReportesController(AppDbContext context)
        {
            _context = context;
        }

        // Acción para el reporte de equipos reparados
        public IActionResult ReparacionEquipos()
        {
            // Obtener los datos necesarios para el reporte
            var equiposReparados = _context.ReparacionEquipos
                .Include(r => r.Activo) // Si necesitas incluir relaciones
                .Include(r => r.Marca)
                .Include(r => r.Ubicacion)
                .Include(r => r.Modelo)
                .Include(r => r.Trabajador)
                .ToList();

            return View(equiposReparados);
        }

        // Acción para el reporte de repuestos de equipos
        public IActionResult RepuestosEquipos()
        {
            // Obtener los datos necesarios para el reporte
            var repuestosEquipos = _context.RepuestosEquipos
                .Include(r => r.Activo) // Si necesitas incluir relaciones
                .Include(r => r.Marca)
                .Include(r => r.Ubicacion)
                .Include(r => r.Modelo)
                .Include(r => r.Trabajador)
                .ToList();

            return View(repuestosEquipos);
        }

        // Exportar Reparación de Equipos a Excel
        public IActionResult ExportarReparacionEquiposExcel()
        {
            var equiposReparados = _context.ReparacionEquipos
                .Include(r => r.Activo)
                .Include(r => r.Marca)
                .Include(r => r.Ubicacion)
                .Include(r => r.Modelo)
                .Include(r => r.Trabajador)
                .ToList();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Configurar licencia

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Equipos Reparados");
                worksheet.Cells[1, 1].Value = "Código";
                worksheet.Cells[1, 2].Value = "Marca";
                worksheet.Cells[1, 3].Value = "Modelo";
                worksheet.Cells[1, 4].Value = "Ubicación";
                worksheet.Cells[1, 5].Value = "Activo";
                worksheet.Cells[1, 6].Value = "Trabajador";
                worksheet.Cells[1, 7].Value = "Trabajo Realizado";
                worksheet.Cells[1, 8].Value = "Fecha Reparación";
                worksheet.Cells[1, 9].Value = "Entregado";

                int row = 2;
                foreach (var equipo in equiposReparados)
                {
                    worksheet.Cells[row, 1].Value = equipo.Codigo;
                    worksheet.Cells[row, 2].Value = equipo.Marca?.Nombre;
                    worksheet.Cells[row, 3].Value = equipo.Modelo?.Nombre;
                    worksheet.Cells[row, 4].Value = equipo.Ubicacion?.Nombre;
                    worksheet.Cells[row, 5].Value = equipo.Activo?.Nombre;
                    worksheet.Cells[row, 6].Value = equipo.Trabajador?.Nombre;
                    worksheet.Cells[row, 7].Value = equipo.TrabajoRealizado;
                    worksheet.Cells[row, 8].Value = equipo.Fecha.ToString("dd/MM/yyyy");
                    worksheet.Cells[row, 9].Value = equipo.Entregado? "SI" : "NO";
                    row++;
                }

                var excelData = package.GetAsByteArray();
                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "EquiposReparados.xlsx");
            }
        }

        // Exportar Reparación de Equipos a PDF
        public IActionResult ExportarReparacionEquiposPdf()
        {
            var equiposReparados = _context.ReparacionEquipos
                .Include(r => r.Activo)
                .Include(r => r.Marca)
                .Include(r => r.Ubicacion)
                .Include(r => r.Modelo)
                .Include(r => r.Trabajador)
                .ToList();

            using (var stream = new MemoryStream())
            {
                var document = new Document(PageSize.A3.Rotate(), 25f, 25f, 25f, 25f);
                var writer = PdfWriter.GetInstance(document, stream);
                document.Open();
                
                // Configurar la fuente del título
                Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20, BaseColor.BLACK);

                // Crear el título
                Paragraph title = new Paragraph("Reporte de Reparación de Equipos", titleFont)
                {
                    Alignment = Element.ALIGN_CENTER, // Centrar el título
                    SpacingAfter = 20f // Espacio después del título
                };

                // Añadir el título al documento
                document.Add(title);

                var table = new PdfPTable(9); // 9 columnas
                table.WidthPercentage = 100;

                // Ajustar anchos proporcionales de las columnas
                float[] columnWidths = { 12f, 8f, 15f, 15f, 8f, 15f, 20f, 10f, 8f};
                table.SetWidths(columnWidths);

            
                Font cellFont = FontFactory.GetFont("Arial", 10, Font.BOLD);

                // Estilo del encabezado
                Font headerFont = FontFactory.GetFont("Arial", 12, Font.BOLD); // Contenido de las celdas (reducido)
                foreach (string header in new[] { "Código", "Marca", "Modelo", "Ubicación", "Activo", "Trabajador","Trabajo Realizado","Fecha Reparación", "Entregado" })
                {
                    PdfPCell headerCell = new PdfPCell(new Phrase(header, headerFont))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        BackgroundColor = new BaseColor(200, 200, 200), // Color gris claro
                        Padding = 5
                    };
                    table.AddCell(headerCell);
                }

                foreach (var equipo in equiposReparados)
                {
                    table.AddCell(equipo.Codigo);
                    table.AddCell(equipo.Marca?.Nombre);
                    table.AddCell(equipo.Modelo?.Nombre);
                    table.AddCell(equipo.Ubicacion?.Nombre);
                    table.AddCell(equipo.Activo?.Nombre);
                    table.AddCell(equipo.Trabajador?.Nombre);
                    table.AddCell(equipo.TrabajoRealizado);
                    table.AddCell(equipo.Fecha.ToString("dd/MM/yyyy"));
                    table.AddCell(equipo.Entregado? "SI" : "NO");
                }

                document.Add(table);
                document.Close();

                return File(stream.ToArray(), "application/pdf", "EquiposReparados.pdf");
            }
        }

        // Exportar Repuestos de Equipos a Excel
        public IActionResult ExportarRepuestosEquiposExcel()
        {
            var equiposReparados = _context.RepuestosEquipos
                .Include(r => r.Activo)
                .Include(r => r.Marca)
                .Include(r => r.Ubicacion)
                .Include(r => r.Modelo)
                .ToList();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Configurar licencia

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Repuestos de Equipos");
                worksheet.Cells[1, 1].Value = "Activo";
                worksheet.Cells[1, 2].Value = "Código";
                worksheet.Cells[1, 3].Value = "Marca";
                worksheet.Cells[1, 4].Value = "Modelo";
                worksheet.Cells[1, 5].Value = "Ubicación";
                worksheet.Cells[1, 6].Value = "Repuesto Solicitado";
                worksheet.Cells[1, 7].Value = "Fecha Solicitud";
                worksheet.Cells[1, 8].Value = "Entregado";

                int row = 2;
                foreach (var equipo in equiposReparados)
                {
                    worksheet.Cells[row, 1].Value = equipo.Activo?.Nombre;
                    worksheet.Cells[row, 2].Value = equipo.Codigo;
                    worksheet.Cells[row, 3].Value = equipo.Marca?.Nombre;
                    worksheet.Cells[row, 4].Value = equipo.Modelo?.Nombre;
                    worksheet.Cells[row, 5].Value = equipo.Ubicacion?.Nombre;
                    worksheet.Cells[row, 6].Value = equipo.Repuesto;
                    worksheet.Cells[row, 7].Value = equipo.Fecha.ToString("dd/MM/yyyy");
                    worksheet.Cells[row, 8].Value = equipo.Comprado ? "SI" : "NO";
                    row++;
                }

                var excelData = package.GetAsByteArray();
                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "RepuestosdeEquipos.xlsx");
            }
        }

        // Exportar Repuestos de Equipos a PDF
        public IActionResult ExportarRepuestosEquiposPdf()
        {
            var equiposReparados = _context.RepuestosEquipos
                .Include(r => r.Activo)
                .Include(r => r.Marca)
                .Include(r => r.Ubicacion)
                .Include(r => r.Modelo)               
                .ToList();

            using (var stream = new MemoryStream())
            {
                var document = new Document(PageSize.A3.Rotate(), 25f, 25f, 25f, 25f);
                var writer = PdfWriter.GetInstance(document, stream);
                document.Open();

                // Configurar la fuente del título
                Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20, BaseColor.BLACK);

                // Crear el título
                Paragraph title = new Paragraph("Reporte de Reparación de Equipos", titleFont)
                {
                    Alignment = Element.ALIGN_CENTER, // Centrar el título
                    SpacingAfter = 20f // Espacio después del título
                };

                // Añadir el título al documento
                document.Add(title);

                var table = new PdfPTable(8); // 9 columnas
                table.WidthPercentage = 100;

                // Ajustar anchos proporcionales de las columnas
                float[] columnWidths = { 10f, 10f, 8f, 10f, 20f, 20f, 9f, 7f };
                table.SetWidths(columnWidths);


                Font cellFont = FontFactory.GetFont("Arial", 10, Font.BOLD);

                // Estilo del encabezado
                Font headerFont = FontFactory.GetFont("Arial", 12, Font.BOLD); // Contenido de las celdas (reducido)
                foreach (string header in new[] { "Activo", "Código", "Marca", "Modelo", "Ubicación", "Repuesto Solicitado", "Fecha Solicitud", "Comprado" })
                {
                    PdfPCell headerCell = new PdfPCell(new Phrase(header, headerFont))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        BackgroundColor = new BaseColor(200, 200, 200), // Color gris claro
                        Padding = 5
                    };
                    table.AddCell(headerCell);
                }

                foreach (var equipo in equiposReparados)
                {
                    table.AddCell(equipo.Activo?.Nombre);
                    table.AddCell(equipo.Codigo);
                    table.AddCell(equipo.Marca?.Nombre);
                    table.AddCell(equipo.Modelo?.Nombre);
                    table.AddCell(equipo.Ubicacion?.Nombre);
                    table.AddCell(equipo.Repuesto);
                    table.AddCell(equipo.Fecha.ToString("dd/MM/yyyy"));
                    table.AddCell(equipo.Comprado ? "SI" : "NO");
                }

                document.Add(table);
                document.Close();

                return File(stream.ToArray(), "application/pdf", "RepuestosdeEquipos.pdf");
            }
        }
    }
}
