using AplicacionTacho.Data;
using AplicacionTacho.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionTacho.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        // 📌 Cargar página - YA NO cuenta vistas
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var info = await _context.AppInfos.FirstOrDefaultAsync();

            if (info == null)
            {
                info = new AppInfo
                {
                    Name = "CleanCity – Horarios de Basura",
                    TotalViews = 0,        // lo usaremos como contador de descargas
                    TotalRating = 0,
                    RatingCount = 0
                };

                _context.AppInfos.Add(info);
                await _context.SaveChangesAsync();
            }

            // ❌ IMPORTANTE:
            // YA NO CONTAMOS NINGUNA VISTA
            // NO PONER info.TotalViews++;

            var comments = await _context.AppComments
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            double avg = info.RatingCount == 0
                ? 0
                : (double)info.TotalRating / info.RatingCount;

            var vm = new AppViewModel
            {
                Info = info,
                Comments = comments,
                AverageRating = avg
            };

            return View(vm);
        }

        // 📌 Método para comentar
        [HttpPost]
        public async Task<IActionResult> AddComment(string userName, string comment, int rating)
        {
            if (string.IsNullOrWhiteSpace(comment) || rating < 1 || rating > 5)
                return RedirectToAction("Index");

            var info = await _context.AppInfos.FirstOrDefaultAsync();

            if (info == null)
            {
                info = new AppInfo
                {
                    Name = "Gestor de Residuos Urbanos – Horarios de Basura",
                    TotalViews = 0,
                    TotalRating = 0,
                    RatingCount = 0
                };

                _context.AppInfos.Add(info);
            }

            var newComment = new AppComment
            {
                UserName = string.IsNullOrWhiteSpace(userName) ? "Anónimo" : userName,
                Comment = comment,
                Rating = rating,
                CreatedAt = DateTime.Now
            };

            _context.AppComments.Add(newComment);

            info.TotalRating += rating;
            info.RatingCount++;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // 📌 Método que SÍ cuenta descargas
        [HttpGet]
        public async Task<IActionResult> Download()
        {
            var info = await _context.AppInfos.FirstOrDefaultAsync();

            if (info == null)
            {
                info = new AppInfo
                {
                    Name = "Gestor Residuos Urbanos – Horarios de Basura",
                    TotalViews = 0,
                    TotalRating = 0,
                    RatingCount = 0
                };

                _context.AppInfos.Add(info);
            }

            // 👉 Aquí SÍ aumentamos el contador (solo descargas)
            info.TotalViews++;
            await _context.SaveChangesAsync();

            // Ruta del APK
            var filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "downloads",
                "GestorResiduosUrbanos.apk" // Asegúrate que este nombre coincida con tu archivo
            );

            var contentType = "application/vnd.android.package-archive";
            var fileName = "GestorResiduosUrbanos.apk";

            return PhysicalFile(filePath, contentType, fileName);
        }
    }
}

