// Factory - Reservation rules
namespace SystemRejestracjiParkingowej.Factories
{
    public interface IReservationRule
    {
        int MaxReservationDays { get; set; }

        bool CanCancel { get; set; }

        int MinCancellationHours { get; set; }

        bool CanExtend { get; set; }

        bool ValidateReservation(int days);
    }

    public class ReservationRule : IReservationRule
    {
        public int MaxReservationDays { get; set; }
        public bool CanCancel { get; set; }
        public int MinCancellationHours { get; set; }
        public bool CanExtend { get; set; }

        public virtual bool ValidateReservation(int days)
        {
            return days > 0 && days <= MaxReservationDays;
        }
    }
}
