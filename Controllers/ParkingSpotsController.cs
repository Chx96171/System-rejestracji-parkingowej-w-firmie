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
            Console.WriteLine("-----> Wywołano akcję CREATE");
            Console.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");
            Console.WriteLine($"SpotNumber: {spot.SpotNumber}, Status: {spot.Status}, SpotType: {spot.SpotType}, ParkingZoneId: {spot.ParkingZoneId}");

            if (ModelState.IsValid)
            {
                try
                {
                    spot.CreatedAt = DateTime.Now; // Przypisanie daty utworzenia
                    _context.Add(spot);

                    // Jeśli miejsce jest dostępne, zwiększamy licznik w strefie
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
                    Console.WriteLine($"Miejsce parkingowe {spot.SpotNumber} zostało pomyślnie dodane.");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Wystąpił błąd: {ex.Message}");
                    ModelState.AddModelError("", $"Wystąpił błąd: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("-----> ModelState jest niepoprawny:");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Walidacja błędu: {error.ErrorMessage}");
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

                    // Przypisanie daty utworzenia z istniejącego rekordu
                    spot.CreatedAt = existingSpot.CreatedAt;

                    // Aktualizacja liczników miejsc w strefie
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
                    Console.WriteLine($"Miejsce parkingowe {spot.SpotNumber} zostało pomyślnie zaktualizowane.");
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

            Console.WriteLine("-----> ModelState jest niepoprawny:");
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($"Walidacja błędu: {error.ErrorMessage}");
            }

            ViewData["ParkingZoneId"] = new SelectList(_context.ParkingZones, "Id", "Name", spot.ParkingZoneId);
            return View(spot);
        }

        // GET: ParkingSpots/Delete/{id}
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

            return View(spot); // Wyświetlenie widoku Delete.cshtml w celu potwierdzenia
        }

        // POST: ParkingSpots/DeleteConfirmed/{id}
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
                    Console.WriteLine($"Miejsce parkingowe o ID {id} zostało usunięte.");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    Console.WriteLine($"Nie znaleziono miejsca parkingowego o ID: {id}.");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas usuwania miejsca parkingowego: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}