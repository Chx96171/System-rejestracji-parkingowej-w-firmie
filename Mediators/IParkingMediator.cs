// Mediator interface
using SystemRejestracjiParkingowej.Models;

namespace SystemRejestracjiParkingowej.Mediators
{
    public interface IParkingMediator
    {
        Task<bool> RegisterReservation(Reservation reservation);

        Task<bool> CancelReservation(int reservationId, string userId);

        Task NotifyAvailability(int spotId, string newStatus);

        Task<bool> CheckConflicts(int spotId, DateTime startDate, DateTime endDate);

        Task<bool> CheckSpotAvailability(int spotId, DateTime startDate, DateTime endDate);

        Task<int> GetUserActiveReservationsCount(string userId);
    }
}
