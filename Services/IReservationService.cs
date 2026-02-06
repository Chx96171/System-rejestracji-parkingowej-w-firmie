// Service interface - Reservations
using SystemRejestracjiParkingowej.Models;

namespace SystemRejestracjiParkingowej.Services
{
    public interface IReservationService
    {
        Task<bool> CreateReservation(Reservation reservation);
        Task<bool> CancelReservation(int reservationId, string userId);
        Task<List<Reservation>> GetUserReservations(string userId);
        Task<bool> CanUserReserve(string userId);
        bool ValidateReservation(Reservation reservation);
    }
}
