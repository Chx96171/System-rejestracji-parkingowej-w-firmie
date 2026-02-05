using Microsoft.EntityFrameworkCore;
using SystemRejestracjiParkingowej.Configuration;
using SystemRejestracjiParkingowej.Data;
using SystemRejestracjiParkingowej.Models;

namespace SystemRejestracjiParkingowej.Mediators
{
    /// <summary>
    /// Implementacja Mediatora dla systemu parkingowego
    /// Koordynuje komunikację między rezerwacjami, miejscami i użytkownikami
    /// Wzorzec projektowy: Mediator Pattern
    /// </summary>
    public class ParkingMediator : IParkingMediator
    {
        private readonly ApplicationDbContext _context;
        private readonly ParkingConfiguration _config;

        /// <summary>
        /// Konstruktor mediatora
        /// </summary>
        /// <param name="context">Kontekst bazy danych</param>
        public ParkingMediator(ApplicationDbContext context)
        {
            _context = context;
            _config = ParkingConfiguration.Instance;
        }

        /// <summary>
        /// Rejestruje nową rezerwację - koordynuje sprawdzenie dostępności, walidację i zapis
        /// </summary>
        public async Task<bool> RegisterReservation(Reservation reservation)
        {
            try
            {
                // Sprawdź konflikty
                bool hasConflicts = await CheckConflicts(
                    reservation.ParkingSpotId, 
                    reservation.StartDate, 
                    reservation.EndDate);

                if (hasConflicts)
                {
                    return false;
                }

                // Sprawdź limity użytkownika
                if (reservation.UserId != null && !_config.AllowMultipleReservations)
                {
                    int activeReservations = await GetUserActiveReservationsCount(reservation.UserId);
                    if (activeReservations > 0)
                    {
                        return false;
                    }
                }

                // Zarejestruj rezerwację
                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();

                // Powiadom o zmianie dostępności
                await NotifyAvailability(reservation.ParkingSpotId, "Reserved");

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Anuluje rezerwację - koordynuje walidację uprawnień i aktualizację statusu
        /// </summary>
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

                // Sprawdź czy można anulować (zgodnie z regułami czasowymi)
                var now = DateTime.UtcNow;
                var hoursUntilStart = (reservation.StartDate - now).TotalHours;

                if (hoursUntilStart < 2) // Minimalne 2 godziny przed rozpoczęciem
                {
                    return false;
                }

                reservation.Status = "Cancelled";
                await _context.SaveChangesAsync();

                // Powiadom o dostępności miejsca
                await NotifyAvailability(reservation.ParkingSpotId, "Available");

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Powiadamia o zmianie dostępności miejsca - aktualizuje status w bazie
        /// </summary>
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
                // W produkcji należy użyć ILogger do logowania błędu
                Console.WriteLine($"Błąd podczas aktualizacji dostępności miejsca {spotId}: {ex.Message}");
                // Rzuć wyjątek dalej, aby nie ukrywać problemu
                throw;
            }
        }

        /// <summary>
        /// Sprawdza czy istnieją konflikty rezerwacji dla danego miejsca i czasu
        /// </summary>
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

        /// <summary>
        /// Sprawdza dostępność miejsca w określonym czasie
        /// </summary>
        public async Task<bool> CheckSpotAvailability(int spotId, DateTime startDate, DateTime endDate)
        {
            // Sprawdź czy miejsce istnieje
            var spot = await _context.ParkingSpots.FindAsync(spotId);
            if (spot == null)
            {
                return false;
            }

            // Sprawdź konflikty rezerwacji
            bool hasConflicts = await CheckConflicts(spotId, startDate, endDate);
            
            return !hasConflicts;
        }

        /// <summary>
        /// Zwraca liczbę aktywnych rezerwacji użytkownika
        /// </summary>
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
