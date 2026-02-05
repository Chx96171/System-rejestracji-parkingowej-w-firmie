using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SystemRejestracjiParkingowej.Data;
using SystemRejestracjiParkingowej.Models;

namespace SystemRejestracjiParkingowej.Controllers
{
    [Authorize]
    public class VehiclesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public VehiclesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Lista pojazdów użytkownika
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var vehicles = await _context.Vehicles
                .Where(v => v.UserId == user.Id)
                .ToListAsync();

            return View(vehicles);
        }

        // Szczegóły pojazdu
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var vehicle = await _context.Vehicles
                .Include(v => v.User)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vehicle == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (vehicle.UserId != user.Id)
                return Forbid();

            return View(vehicle);
        }

        // Formularz dodawania pojazdu
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Obsługa dodawania nowego pojazdu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VehicleType,RegistrationNumber,Brand,Model,Color")] Vehicle vehicle)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            // Przypisanie ID użytkownika do pojazdu
            vehicle.UserId = user.Id;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(vehicle);
                    await _context.SaveChangesAsync();

                    Console.WriteLine($"Pojazd {vehicle.RegistrationNumber} został pomyślnie dodany użytkownikowi {user.UserName}.");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Błąd dodawania pojazdu: {ex.Message}");
                    ModelState.AddModelError("", $"Wystąpił błąd: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("ModelState jest niepoprawny:");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Błąd walidacji: {error.ErrorMessage}");
                }
            }

            return View(vehicle);
        }

        // Formularz edycji pojazdu
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (vehicle.UserId != user.Id)
                return Forbid();

            return View(vehicle);
        }

        // Obsługa edycji pojazdu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VehicleType,RegistrationNumber,Brand,Model,Color")] Vehicle vehicle)
        {
            if (id != vehicle.Id)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            // Walidacja zgodności pojazdu z zalogowanym użytkownikiem
            var existingVehicle = await _context.Vehicles.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);
            if (existingVehicle?.UserId != user.Id)
                return Forbid();

            if (ModelState.IsValid)
            {
                try
                {
                    vehicle.UserId = user.Id; // Aktualizacja UserId
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();

                    Console.WriteLine($"Pojazd {vehicle.RegistrationNumber} został zaktualizowany.");
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Vehicles.Any(v => v.Id == vehicle.Id))
                        return NotFound();

                    throw;
                }
            }
            else
            {
                Console.WriteLine("ModelState jest niepoprawny:");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Błąd walidacji: {error.ErrorMessage}");
                }
            }

            return View(vehicle);
        }

        // Formularz potwierdzenia usunięcia
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == id);

            if (vehicle == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (vehicle.UserId != user.Id)
                return Forbid();

            return View(vehicle);
        }

        // Obsługa usuwania pojazdu
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync();

                Console.WriteLine($"Pojazd o ID {vehicle.Id} został usunięty.");
            }
            else
            {
                Console.WriteLine($"Nie znaleziono pojazdu o ID: {id}.");
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}