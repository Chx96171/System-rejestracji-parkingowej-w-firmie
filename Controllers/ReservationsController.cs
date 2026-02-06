// Controller - Reservations
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SystemRejestracjiParkingowej.Data;
using SystemRejestracjiParkingowej.Models;

namespace SystemRejestracjiParkingowej.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReservationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var reservations = await _context.Reservations
                .Where(r => r.UserId == user.Id)
                .Include(r => r.ParkingSpot)
                .Include(r => r.Vehicle)
                .ToListAsync();

            return View(reservations);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var reservation = await _context.Reservations
                .Include(r => r.ParkingSpot)
                .Include(r => r.Vehicle)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (reservation.UserId != user.Id && !User.IsInRole("Admin"))
                return Forbid();

            return View(reservation);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            ViewData["VehicleId"] = new SelectList(
                await _context.Vehicles.Where(v => v.UserId == user.Id).ToListAsync(),
                "Id",
                "RegistrationNumber"
            );

            ViewData["ParkingSpotId"] = new SelectList(
                await _context.ParkingSpots.Where(s => s.Status == "Available").ToListAsync(),
                "Id",
                "SpotNumber"
            );

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ParkingSpotId,VehicleId,StartDate,EndDate")] Reservation reservation)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            reservation.UserId = user.Id;
            reservation.Status = "Confirmed";

            if (reservation.StartDate >= reservation.EndDate)
            {
                ModelState.AddModelError(nameof(reservation.StartDate), "Data rozpoczęcia musi być wcześniejsza niż data zakończenia");
            }

            var spot = await _context.ParkingSpots.FindAsync(reservation.ParkingSpotId);
            if (spot == null)
            {
                ModelState.AddModelError(nameof(reservation.ParkingSpotId), "Wybrane miejsce parkingowe nie istnieje");
            }
            else if (spot.Status != "Available")
            {
                ModelState.AddModelError(nameof(reservation.ParkingSpotId), "Wybrane miejsce parkingowe jest już zajęte");
            }

            var vehicle = await _context.Vehicles.FindAsync(reservation.VehicleId);
            if (vehicle == null)
            {
                ModelState.AddModelError(nameof(reservation.VehicleId), "Wybrany pojazd nie istnieje");
            }
            else if (vehicle.UserId != user.Id)
            {
                ModelState.AddModelError(nameof(reservation.VehicleId), "Wybrany pojazd nie należy do Ciebie");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    spot.Status = "Reserved";
                    _context.Update(spot);

                    _context.Add(reservation);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Rezerwacja została utworzona pomyślnie!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Wystąpił błąd podczas zapisywania: {ex.Message}");
                }
            }

            ViewData["VehicleId"] = new SelectList(
                await _context.Vehicles.Where(v => v.UserId == user.Id).ToListAsync(),
                "Id",
                "RegistrationNumber",
                reservation.VehicleId
            );

            ViewData["ParkingSpotId"] = new SelectList(
                await _context.ParkingSpots.Where(s => s.Status == "Available").ToListAsync(),
                "Id",
                "SpotNumber",
                reservation.ParkingSpotId
            );

            return View(reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.ParkingSpot)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (reservation.UserId != user.Id)
                return Forbid();

            reservation.Status = "Cancelled";
            reservation.ParkingSpot.Status = "Available";

            _context.Update(reservation.ParkingSpot);
            _context.Update(reservation);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Rezerwacja została anulowana pomyślnie!";
            return RedirectToAction(nameof(Index));
        }
    }
}