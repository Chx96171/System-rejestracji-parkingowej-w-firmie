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

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VehicleType,RegistrationNumber,Brand,Model,Color")] Vehicle vehicle)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            vehicle.UserId = user.Id;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(vehicle);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Pojazd został dodany pomyślnie!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Wystąpił błąd podczas zapisywania: {ex.Message}");
                }
            }

            return View(vehicle);
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VehicleType,RegistrationNumber,Brand,Model,Color")] Vehicle vehicle)
        {
            if (id != vehicle.Id)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var existingVehicle = await _context.Vehicles.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);
            if (existingVehicle?.UserId != user.Id)
                return Forbid();

            if (ModelState.IsValid)
            {
                try
                {
                    vehicle.UserId = user.Id;
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Pojazd został zaktualizowany pomyślnie!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Vehicles.Any(v => v.Id == vehicle.Id))
                        return NotFound();

                    throw;
                }
            }

            return View(vehicle);
        }

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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Pojazd został usunięty pomyślnie!";
            }
            else
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}