using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SystemRejestracjiParkingowej.Data;
using SystemRejestracjiParkingowej.Models;

namespace SystemRejestracjiParkingowej.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ParkingSpotsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ParkingSpotsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var spots = await _context.ParkingSpots
                .Include(s => s.ParkingZone)
                .ToListAsync();
            return View(spots);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var spot = await _context.ParkingSpots
                .Include(s => s.ParkingZone)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (spot == null) return NotFound();

            return View(spot);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["ParkingZoneId"] = new SelectList(_context.ParkingZones, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SpotNumber,Status,SpotType,ParkingZoneId")] ParkingSpot spot)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    spot.CreatedAt = DateTime.Now;
                    _context.Add(spot);

                    if (spot.Status == "Available")
                    {
                        var zone = await _context.ParkingZones.FindAsync(spot.ParkingZoneId);
                        if (zone != null)
                        {
                            zone.TotalSpots++;
                            _context.Update(zone);
                        }
                    }

                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Miejsce parkingowe zostało dodane pomyślnie!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Wystąpił błąd podczas zapisywania: {ex.Message}");
                }
            }

            ViewData["ParkingZoneId"] = new SelectList(_context.ParkingZones, "Id", "Name", spot.ParkingZoneId);
            return View(spot);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var spot = await _context.ParkingSpots.FindAsync(id);
            if (spot == null) return NotFound();

            ViewData["ParkingZoneId"] = new SelectList(_context.ParkingZones, "Id", "Name", spot.ParkingZoneId);
            return View(spot);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SpotNumber,Status,SpotType,ParkingZoneId")] ParkingSpot spot)
        {
            if (id != spot.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingSpot = await _context.ParkingSpots.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
                    if (existingSpot == null)
                    {
                        return NotFound();
                    }

                    spot.CreatedAt = existingSpot.CreatedAt;

                    if (existingSpot.Status != spot.Status)
                    {
                        var zone = await _context.ParkingZones.FirstOrDefaultAsync(z => z.Id == spot.ParkingZoneId);
                        if (zone != null)
                        {
                            if (existingSpot.Status == "Available") zone.TotalSpots--;
                            if (spot.Status == "Available") zone.TotalSpots++;
                            _context.Update(zone);
                        }
                    }

                    _context.Update(spot);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Miejsce parkingowe zostało zaktualizowane pomyślnie!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.ParkingSpots.Any(s => s.Id == spot.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["ParkingZoneId"] = new SelectList(_context.ParkingZones, "Id", "Name", spot.ParkingZoneId);
            return View(spot);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spot = await _context.ParkingSpots
                .Include(s => s.ParkingZone)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (spot == null)
            {
                return NotFound();
            }

            return View(spot);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var spot = await _context.ParkingSpots.FindAsync(id);
                if (spot != null)
                {
                    if (spot.Status == "Available")
                    {
                        var zone = await _context.ParkingZones.FindAsync(spot.ParkingZoneId);
                        if (zone != null)
                        {
                            zone.TotalSpots--;
                            _context.Update(zone);
                        }
                    }

                    _context.ParkingSpots.Remove(spot);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Miejsce parkingowe zostało usunięte pomyślnie!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Błąd podczas usuwania: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}