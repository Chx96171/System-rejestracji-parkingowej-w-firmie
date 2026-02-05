using Microsoft.EntityFrameworkCore;
using SystemRejestracjiParkingowej.Configuration;
using SystemRejestracjiParkingowej.Data;
using SystemRejestracjiParkingowej.Mediators;
using SystemRejestracjiParkingowej.Models;

namespace SystemRejestracjiParkingowej.Services
{
    /// <summary>
    /// Serwis zarządzania rezerwacjami
    /// Wykorzystuje Mediator Pattern do koordynacji operacji
    /// </summary>
    public class ReservationService : IReservationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IParkingMediator _mediator;
        private readonly ParkingConfiguration _config;

        public ReservationService(ApplicationDbContext context, IParkingMediator mediator)
        {
            _context = context;
            _mediator = mediator;
            _config = ParkingConfiguration.Instance;
        }

        /// <summary>
        /// Tworzy nową rezerwację przy użyciu mediatora
        /// </summary>
        public async Task<bool> CreateReservation(Reservation reservation)
        {
            if (!ValidateReservation(reservation))
                return false;

            // Użyj mediatora do koordynacji procesu rezerwacji
            return await _mediator.RegisterReservation(reservation);
        }

        /// <summary>
        /// Anuluje rezerwację przy użyciu mediatora
        /// </summary>
        public async Task<bool> CancelReservation(int reservationId, string userId)
        {
            return await _mediator.CancelReservation(reservationId, userId);
        }

        /// <summary>
        /// Pobiera rezerwacje użytkownika
        /// </summary>
        public async Task<List<Reservation>> GetUserReservations(string userId)
        {
            return await _context.Reservations
                .Where(r => r.UserId == userId)
                .Include(r => r.ParkingSpot)
                .Include(r => r.Vehicle)
                .OrderByDescending(r => r.StartDate)
                .ToListAsync();
        }

        /// <summary>
        /// Sprawdza czy użytkownik może utworzyć nową rezerwację
        /// </summary>
        public async Task<bool> CanUserReserve(string userId)
        {
            if (_config.AllowMultipleReservations)
                return true;

            var activeCount = await _mediator.GetUserActiveReservationsCount(userId);
            return activeCount == 0;
        }

        /// <summary>
        /// Waliduje dane rezerwacji
        /// </summary>
        public bool ValidateReservation(Reservation reservation)
        {
            // Sprawdź czy data zakończenia jest późniejsza niż rozpoczęcia
            if (reservation.EndDate <= reservation.StartDate)
                return false;

            // Sprawdź czy rezerwacja nie jest w przeszłości
            if (reservation.StartDate < DateTime.UtcNow)
                return false;

            // Sprawdź długość rezerwacji
            var days = (reservation.EndDate - reservation.StartDate).Days;
            if (!_config.ValidateReservationDays(days))
                return false;

            return true;
        }
    }
}
