// Controller - Parking zones
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
                // Ustaw TotalSpots na 0 - będzie aktualizowane przy dodawaniu miejsc
                zone.TotalSpots = 0;
                
                // Ustaw MaxReservationDays w zależności od typu strefy
                zone.MaxReservationDays = zone.ZoneType switch
                {
                    "VIP" => 90,        // VIP - 90 dni
                    "Employee" => 30,   // Pracownicy - 30 dni
                    "Public" => 7,      // Publiczny - 7 dni
                    "Underground" => 30,
                    "Outdoor" => 30,
                    "Covered" => 30,
                    _ => 30             // Domyślnie 30 dni
                };
                
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

            var spotsIds = zone.ParkingSpots.Select(s => s.Id).ToList();
            
            var activeReservationsCount = 0;
            var totalReservationsCount = 0;
            
            if (spotsIds.Any())
            {
                activeReservationsCount = await _context.Reservations
                    .CountAsync(r => spotsIds.Contains(r.ParkingSpotId) && 
                                (r.Status == "Confirmed" || r.Status == "Pending" || r.Status == "Active"));

                totalReservationsCount = await _context.Reservations
                    .CountAsync(r => spotsIds.Contains(r.ParkingSpotId));
            }

            ViewBag.ActiveReservations = activeReservationsCount;
            ViewBag.TotalReservations = totalReservationsCount;

            if (activeReservationsCount > 0)
            {
                ViewBag.Error = $"Nie można usunąć strefy! Miejsca w tej strefie mają {activeReservationsCount} aktywnych rezerwacji.";
            }

            return View(zone);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zone = await _context.ParkingZones
                .Include(z => z.ParkingSpots)
                .FirstOrDefaultAsync(z => z.Id == id);

            if (zone == null)
            {
                return NotFound();
            }

            var spotsIds = zone.ParkingSpots.Select(s => s.Id).ToList();
            
            var activeReservationsCount = await _context.Reservations
                .CountAsync(r => spotsIds.Contains(r.ParkingSpotId) && 
                            (r.Status == "Confirmed" || r.Status == "Pending" || r.Status == "Active"));

            if (activeReservationsCount > 0)
            {
                TempData["Error"] = $"Nie można usunąć strefy! Miejsca w tej strefie mają {activeReservationsCount} aktywnych rezerwacji. Najpierw anuluj lub poczekaj na zakończenie rezerwacji.";
                return RedirectToAction(nameof(Index));
            }

            var allReservations = await _context.Reservations
                .Where(r => spotsIds.Contains(r.ParkingSpotId))
                .ToListAsync();

            if (allReservations.Any())
            {
                _context.Reservations.RemoveRange(allReservations);
            }

            _context.ParkingSpots.RemoveRange(zone.ParkingSpots);

            _context.ParkingZones.Remove(zone);

            await _context.SaveChangesAsync();

            TempData["Success"] = $"Strefa parkingowa została usunięta pomyślnie wraz z {zone.ParkingSpots.Count} miejscami i {allReservations.Count} rezerwacjami.";
            return RedirectToAction(nameof(Index));
        }
    }
}