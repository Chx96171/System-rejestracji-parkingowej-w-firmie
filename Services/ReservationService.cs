// Service - Reservations
using Microsoft.EntityFrameworkCore;
using SystemRejestracjiParkingowej.Configuration;
using SystemRejestracjiParkingowej.Data;
using SystemRejestracjiParkingowej.Mediators;
using SystemRejestracjiParkingowej.Models;

namespace SystemRejestracjiParkingowej.Services
{
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

        public async Task<bool> CreateReservation(Reservation reservation)
        {
            if (!await ValidateReservationAsync(reservation))
                return false;

            return await _mediator.RegisterReservation(reservation);
        }

        public async Task<bool> CancelReservation(int reservationId, string userId)
        {
            return await _mediator.CancelReservation(reservationId, userId);
        }

        public async Task<List<Reservation>> GetUserReservations(string userId)
        {
            return await _context.Reservations
                .Where(r => r.UserId == userId)
                .Include(r => r.ParkingSpot)
                .Include(r => r.Vehicle)
                .OrderByDescending(r => r.StartDate)
                .ToListAsync();
        }

        public async Task<bool> CanUserReserve(string userId)
        {
            if (_config.AllowMultipleReservations)
                return true;

            var activeCount = await _mediator.GetUserActiveReservationsCount(userId);
            return activeCount == 0;
        }

        public bool ValidateReservation(Reservation reservation)
        {
            if (reservation.EndDate <= reservation.StartDate)
                return false;

            if (reservation.StartDate < DateTime.UtcNow)
                return false;

            var days = (reservation.EndDate - reservation.StartDate).Days;
            if (!_config.ValidateReservationDays(days))
                return false;

            return true;
        }

        /// <summary>
        /// Waliduje dane rezerwacji asynchronicznie - sprawdza także limit strefy
        /// </summary>
        public async Task<bool> ValidateReservationAsync(Reservation reservation)
        {
            // Sprawdź czy data zakończenia jest późniejsza niż rozpoczęcia
            if (reservation.EndDate <= reservation.StartDate)
                return false;

            // Sprawdź czy rezerwacja nie jest w przeszłości
            if (reservation.StartDate < DateTime.UtcNow)
                return false;

            var days = (reservation.EndDate - reservation.StartDate).Days;
            
            // Sprawdź limit konkretnej strefy parkingowej
            var spot = await _context.ParkingSpots
                .Include(s => s.ParkingZone)
                .FirstOrDefaultAsync(s => s.Id == reservation.ParkingSpotId);
            
            if (spot?.ParkingZone == null)
                return false;
            
            // Użyj limitu z konkretnej strefy zamiast globalnego
            if (days > spot.ParkingZone.MaxReservationDays)
                return false;

            return true;
        }
    }
}
