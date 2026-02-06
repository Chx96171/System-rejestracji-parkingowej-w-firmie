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

        /// <summary>
        /// Waliduje dane rezerwacji (asynchronicznie) - sprawdza także limit strefy
        /// </summary>
        /// <param name="reservation">Rezerwacja do walidacji</param>
        /// <returns>True jeśli dane są poprawne</returns>
        Task<bool> ValidateReservationAsync(Reservation reservation);
    }
}
