using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SystemRejestracjiParkingowej.Data;
using SystemRejestracjiParkingowej.Models;

namespace SystemRejestracjiParkingowej.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ParkingZonesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ParkingZonesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var zones = await _context.ParkingZones
                .Include(z => z.ParkingSpots)
                .ToListAsync();
            return View(zones);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var zone = await _context.ParkingZones
                .Include(z => z.ParkingSpots)
                .FirstOrDefaultAsync(z => z.Id == id);

            if (zone == null) return NotFound();

            return View(zone);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,ZoneType")] ParkingZone zone)
        {
            if (ModelState.IsValid)
            {
                zone.TotalSpots = 0;
                _context.Add(zone);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Strefa parkingowa została utworzona pomyślnie!";
                return RedirectToAction(nameof(Index));
            }

            return View(zone);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var zone = await _context.ParkingZones.FindAsync(id);
            if (zone == null) return NotFound();

            return View(zone);
        }

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

                    existingZone.Name = zone.Name;
                    existingZone.Description = zone.Description;
                    existingZone.ZoneType = zone.ZoneType;

                    _context.Update(existingZone);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Strefa parkingowa została zaktualizowana pomyślnie!";
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zone = await _context.ParkingZones
                .Include(z => z.ParkingSpots)
                .FirstOrDefaultAsync(z => z.Id == id);

            if (zone != null)
            {
                _context.ParkingSpots.RemoveRange(zone.ParkingSpots);
                _context.ParkingZones.Remove(zone);

                await _context.SaveChangesAsync();
                TempData["Success"] = "Strefa parkingowa została usunięta pomyślnie!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}