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

            // NOWY KOD - Sprawdź aktywne rezerwacje
            var activeReservationsCount = await _context.Reservations
                .CountAsync(r => r.ParkingSpotId == id && 
                            (r.Status == "Confirmed" || r.Status == "Pending" || r.Status == "Active"));

            if (activeReservationsCount > 0)
            {
                ViewBag.ActiveReservations = activeReservationsCount;
                ViewBag.Warning = $"UWAGA: To miejsce ma {activeReservationsCount} aktywnych rezerwacji. Zmiana statusu spowoduje ich anulowanie!";
            }

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

                    // NOWY KOD - Sprawdź czy status się zmienia z Reserved
                    if (existingSpot.Status == "Reserved" && spot.Status != "Reserved")
                    {
                        // Znajdź wszystkie aktywne rezerwacje dla tego miejsca
                        var activeReservations = await _context.Reservations
                            .Where(r => r.ParkingSpotId == id && 
                                   (r.Status == "Confirmed" || r.Status == "Pending" || r.Status == "Active"))
                            .ToListAsync();

                        if (activeReservations.Any())
                        {
                            // Anuluj rezerwacje
                            foreach (var reservation in activeReservations)
                            {
                                reservation.Status = "Cancelled";
                                _context.Update(reservation);
                            }

                            TempData["Warning"] = $"Uwaga: Anulowano {activeReservations.Count} aktywnych rezerwacji powiązanych z tym miejscem.";
                        }
                    }

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

            // NOWY KOD - Sprawdź rezerwacje
            var activeReservationsCount = await _context.Reservations
                .CountAsync(r => r.ParkingSpotId == id && 
                            (r.Status == "Confirmed" || r.Status == "Pending" || r.Status == "Active"));

            var totalReservationsCount = await _context.Reservations
                .CountAsync(r => r.ParkingSpotId == id);

            ViewBag.ActiveReservations = activeReservationsCount;
            ViewBag.TotalReservations = totalReservationsCount;

            if (activeReservationsCount > 0)
            {
                ViewBag.Error = $"To miejsce ma {activeReservationsCount} aktywnych rezerwacji i nie może zostać usunięte!";
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
                if (spot == null)
                {
                    return NotFound();
                }

                // NOWY KOD - Sprawdź rezerwacje
                var allReservations = await _context.Reservations
                    .Where(r => r.ParkingSpotId == id)
                    .ToListAsync();

                var activeReservations = allReservations
                    .Where(r => r.Status == "Confirmed" || r.Status == "Pending" || r.Status == "Active")
                    .ToList();

                if (activeReservations.Any())
                {
                    TempData["Error"] = $"Nie można usunąć miejsca parkingowego! Istnieją {activeReservations.Count} aktywne rezerwacje. Najpierw anuluj rezerwacje lub poczekaj aż się zakończą.";
                    return RedirectToAction(nameof(Index));
                }

                // Usuń wszystkie historyczne rezerwacje (zakończone/anulowane)
                if (allReservations.Any())
                {
                    _context.Reservations.RemoveRange(allReservations);
                }

                // Aktualizuj licznik w strefie jeśli było Available
                if (spot.Status == "Available")
                {
                    var zone = await _context.ParkingZones.FindAsync(spot.ParkingZoneId);
                    if (zone != null && zone.TotalSpots > 0)
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
            catch (Exception ex)
            {
                TempData["Error"] = $"Błąd podczas usuwania: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}