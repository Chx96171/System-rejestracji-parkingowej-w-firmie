namespace SystemRejestracjiParkingowej.Factories
{
    /// <summary>
    /// Interfejs dla reguł rezerwacji
    /// Produkt Abstract Factory Pattern
    /// </summary>
    public interface IReservationRule
    {
        /// <summary>
        /// Maksymalna liczba dni rezerwacji
        /// </summary>
        int MaxReservationDays { get; set; }

        /// <summary>
        /// Czy można anulować rezerwację
        /// </summary>
        bool CanCancel { get; set; }

        /// <summary>
        /// Minimalny czas przed rozpoczęciem, aby móc anulować (w godzinach)
        /// </summary>
        int MinCancellationHours { get; set; }

        /// <summary>
        /// Czy można przedłużyć rezerwację
        /// </summary>
        bool CanExtend { get; set; }

        /// <summary>
        /// Waliduje czy rezerwacja jest zgodna z regułami
        /// </summary>
        /// <param name="days">Liczba dni rezerwacji</param>
        /// <returns>True jeśli rezerwacja jest zgodna z regułami</returns>
        bool ValidateReservation(int days);
    }

    /// <summary>
    /// Bazowa implementacja reguł rezerwacji
    /// </summary>
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
