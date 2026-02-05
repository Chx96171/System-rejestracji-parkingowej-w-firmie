using SystemRejestracjiParkingowej.Models;

namespace SystemRejestracjiParkingowej.Services
{
    /// <summary>
    /// Interfejs serwisu rezerwacji
    /// </summary>
    public interface IReservationService
    {
        /// <summary>
        /// Tworzy nową rezerwację
        /// </summary>
        /// <param name="reservation">Dane rezerwacji</param>
        /// <returns>True jeśli rezerwacja została utworzona pomyślnie</returns>
        Task<bool> CreateReservation(Reservation reservation);

        /// <summary>
        /// Anuluje rezerwację
        /// </summary>
        /// <param name="reservationId">ID rezerwacji</param>
        /// <param name="userId">ID użytkownika</param>
        /// <returns>True jeśli anulowanie powiodło się</returns>
        Task<bool> CancelReservation(int reservationId, string userId);

        /// <summary>
        /// Pobiera rezerwacje użytkownika
        /// </summary>
        /// <param name="userId">ID użytkownika</param>
        /// <returns>Lista rezerwacji</returns>
        Task<List<Reservation>> GetUserReservations(string userId);

        /// <summary>
        /// Sprawdza czy użytkownik może utworzyć nową rezerwację
        /// </summary>
        /// <param name="userId">ID użytkownika</param>
        /// <returns>True jeśli użytkownik może utworzyć rezerwację</returns>
        Task<bool> CanUserReserve(string userId);

        /// <summary>
        /// Waliduje dane rezerwacji
        /// </summary>
        /// <param name="reservation">Rezerwacja do walidacji</param>
        /// <returns>True jeśli dane są poprawne</returns>
        bool ValidateReservation(Reservation reservation);
    }
}
