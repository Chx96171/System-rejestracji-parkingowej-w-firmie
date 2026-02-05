using SystemRejestracjiParkingowej.Models;

namespace SystemRejestracjiParkingowej.Mediators
{
    /// <summary>
    /// Interfejs Mediatora dla systemu parkingowego
    /// Wzorzec projektowy: Mediator Pattern
    /// </summary>
    public interface IParkingMediator
    {
        /// <summary>
        /// Rejestruje nową rezerwację w systemie
        /// </summary>
        /// <param name="reservation">Rezerwacja do zarejestrowania</param>
        /// <returns>True jeśli rezerwacja została zarejestrowana pomyślnie</returns>
        Task<bool> RegisterReservation(Reservation reservation);

        /// <summary>
        /// Anuluje rezerwację
        /// </summary>
        /// <param name="reservationId">ID rezerwacji do anulowania</param>
        /// <param name="userId">ID użytkownika anulującego</param>
        /// <returns>True jeśli anulowanie powiodło się</returns>
        Task<bool> CancelReservation(int reservationId, string userId);

        /// <summary>
        /// Powiadamia o zmianie dostępności miejsca parkingowego
        /// </summary>
        /// <param name="spotId">ID miejsca parkingowego</param>
        /// <param name="newStatus">Nowy status miejsca</param>
        Task NotifyAvailability(int spotId, string newStatus);

        /// <summary>
        /// Sprawdza konflikty rezerwacji dla danego miejsca
        /// </summary>
        /// <param name="spotId">ID miejsca parkingowego</param>
        /// <param name="startDate">Data rozpoczęcia</param>
        /// <param name="endDate">Data zakończenia</param>
        /// <returns>True jeśli istnieją konflikty</returns>
        Task<bool> CheckConflicts(int spotId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Sprawdza dostępność miejsca w danym okresie
        /// </summary>
        /// <param name="spotId">ID miejsca parkingowego</param>
        /// <param name="startDate">Data rozpoczęcia</param>
        /// <param name="endDate">Data zakończenia</param>
        /// <returns>True jeśli miejsce jest dostępne</returns>
        Task<bool> CheckSpotAvailability(int spotId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Zwraca liczbę aktywnych rezerwacji użytkownika
        /// </summary>
        /// <param name="userId">ID użytkownika</param>
        /// <returns>Liczba aktywnych rezerwacji</returns>
        Task<int> GetUserActiveReservationsCount(string userId);
    }
}
