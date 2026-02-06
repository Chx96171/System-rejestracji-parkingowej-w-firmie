// Mediator - Reservations
using Microsoft.EntityFrameworkCore;
using SystemRejestracjiParkingowej.Configuration;
using SystemRejestracjiParkingowej.Data;
using SystemRejestracjiParkingowej.Models;

namespace SystemRejestracjiParkingowej.Mediators
{
    public class ParkingMediator : IParkingMediator
    {
        private readonly ApplicationDbContext _context;
        private readonly ParkingConfiguration _config;

        public ParkingMediator(ApplicationDbContext context)
        {
            _context = context;
            _config = ParkingConfiguration.Instance;
        }

        public async Task<bool> RegisterReservation(Reservation reservation)
        {
            try
            {
                bool hasConflicts = await CheckConflicts(
                    reservation.ParkingSpotId, 
                    reservation.StartDate, 
                    reservation.EndDate);

                if (hasConflicts)
                {
                    return false;
                }

                if (reservation.UserId != null && !_config.AllowMultipleReservations)
                {
                    int activeReservations = await GetUserActiveReservationsCount(reservation.UserId);
                    if (activeReservations > 0)
                    {
                        return false;
                    }
                }

                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();

                await NotifyAvailability(reservation.ParkingSpotId, "Reserved");

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> CancelReservation(int reservationId, string userId)
        {
            try
            {
                var reservation = await _context.Reservations
                    .FirstOrDefaultAsync(r => r.Id == reservationId);

                if (reservation == null || reservation.UserId != userId)
                {
                    return false;
                }

                var now = DateTime.UtcNow;
                var hoursUntilStart = (reservation.StartDate - now).TotalHours;

                if (hoursUntilStart < 2)
                {
                    return false;
                }

                reservation.Status = "Cancelled";
                await _context.SaveChangesAsync();

                await NotifyAvailability(reservation.ParkingSpotId, "Available");

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task NotifyAvailability(int spotId, string newStatus)
        {
            try
            {
                var spot = await _context.ParkingSpots.FindAsync(spotId);
                if (spot != null)
                {
                    spot.Status = newStatus;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas aktualizacji dostępności miejsca {spotId}: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> CheckConflicts(int spotId, DateTime startDate, DateTime endDate)
        {
            var conflicts = await _context.Reservations
                .Where(r => r.ParkingSpotId == spotId 
                    && r.Status != "Cancelled"
                    && r.Status != "Completed"
                    && ((r.StartDate <= startDate && r.EndDate >= startDate) ||
                        (r.StartDate <= endDate && r.EndDate >= endDate) ||
                        (r.StartDate >= startDate && r.EndDate <= endDate)))
                .AnyAsync();

            return conflicts;
        }

        public async Task<bool> CheckSpotAvailability(int spotId, DateTime startDate, DateTime endDate)
        {
            var spot = await _context.ParkingSpots.FindAsync(spotId);
            if (spot == null)
            {
                return false;
            }

            bool hasConflicts = await CheckConflicts(spotId, startDate, endDate);
            
            return !hasConflicts;
        }

        public async Task<int> GetUserActiveReservationsCount(string userId)
        {
            var count = await _context.Reservations
                .Where(r => r.UserId == userId 
                    && r.Status != "Cancelled" 
                    && r.Status != "Completed"
                    && r.EndDate >= DateTime.UtcNow)
                .CountAsync();

            return count;
        }
    }
}
