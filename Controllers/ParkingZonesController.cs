using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SystemRejestracjiParkingowej.Data;
using SystemRejestracjiParkingowej.Models;

namespace SystemRejestracjiParkingowej.Controllers
{
    /// <summary>
    /// Kontroler do zarządzania strefami parkingowymi
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class ParkingZonesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ParkingZonesController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Wyświetla listę wszystkich stref parkingowych
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var zones = await _context.ParkingZones
                .Include(z => z.ParkingSpots)
                .ToListAsync();
            return View(zones);
        }

        /// <summary>
        /// Wyświetla szczegóły strefy parkingowej
        /// </summary>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var zone = await _context.ParkingZones
                .Include(z => z.ParkingSpots)
                .FirstOrDefaultAsync(z => z.Id == id);

            if (zone == null) return NotFound();

            return View(zone);
        }

        /// <summary>
        /// Wyświetla formularz do tworzenia nowej strefy
        /// </summary>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Obsługuje tworzenie nowej strefy
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,ZoneType")] ParkingZone zone)
        {
            if (ModelState.IsValid)
            {
                zone.TotalSpots = 0; // Ustawienie początkowej liczby miejsc na 0
                _context.Add(zone);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(zone);
        }

        /// <summary>
        /// Wyświetla formularz do edycji strefy
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var zone = await _context.ParkingZones.FindAsync(id);
            if (zone == null) return NotFound();

            return View(zone);
        }

        /// <summary>
        /// Obsługuje edycję strefy
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ZoneType")] ParkingZone zone)
        {
            if (id != zone.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingZone = await _context.ParkingZones
                        .Include(z => z.ParkingSpots)
                        .FirstOrDefaultAsync(z => z.Id == id);

                    if (existingZone == null) return NotFound();

                    // Aktualizacja właściwości strefy
                    existingZone.Name = zone.Name;
                    existingZone.Description = zone.Description;
                    existingZone.ZoneType = zone.ZoneType;

                    _context.Update(existingZone);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.ParkingZones.Any(z => z.Id == zone.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(zone);
        }

        /// <summary>
        /// Wyświetla stronę potwierdzenia usunięcia
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var zone = await _context.ParkingZones
                .Include(z => z.ParkingSpots)
                .FirstOrDefaultAsync(z => z.Id == id);

            if (zone == null) return NotFound();

            return View(zone);
        }

        /// <summary>
        /// Obsługuje usunięcie strefy
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zone = await _context.ParkingZones
                .Include(z => z.ParkingSpots)
                .FirstOrDefaultAsync(z => z.Id == id);

            if (zone != null)
            {
                // Usunięcie miejsc parkingowych powiązanych z tą strefą
                _context.ParkingSpots.RemoveRange(zone.ParkingSpots);

                // Usunięcie samej strefy
                _context.ParkingZones.Remove(zone);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}