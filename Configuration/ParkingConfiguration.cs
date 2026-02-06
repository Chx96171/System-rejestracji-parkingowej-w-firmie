// Singleton - Configuration
namespace SystemRejestracjiParkingowej.Configuration
{
    public sealed class ParkingConfiguration
    {
        private static readonly Lazy<ParkingConfiguration> _instance = 
            new Lazy<ParkingConfiguration>(() => new ParkingConfiguration());

        public int MaxReservationDays { get; set; }

        public int DefaultReservationHours { get; set; }

        public bool AllowMultipleReservations { get; set; }

        public TimeSpan ParkingOpenHour { get; set; }

        public TimeSpan ParkingCloseHour { get; set; }

        public bool Is24Hours { get; set; }

        public int MaxVehiclesPerUser { get; set; }

        private ParkingConfiguration()
        {
            MaxReservationDays = 30;
            DefaultReservationHours = 2;
            AllowMultipleReservations = false;
            ParkingOpenHour = new TimeSpan(6, 0, 0);
            ParkingCloseHour = new TimeSpan(22, 0, 0);
            Is24Hours = false;
            MaxVehiclesPerUser = 5;
        }

        public static ParkingConfiguration Instance => _instance.Value;

        public static ParkingConfiguration GetInstance()
        {
            return _instance.Value;
        }

        public bool IsParkingOpen(TimeSpan time)
        {
            if (Is24Hours)
                return true;

            return time >= ParkingOpenHour && time <= ParkingCloseHour;
        }

        public bool ValidateReservationDays(int days)
        {
            return days > 0 && days <= MaxReservationDays;
        }

        public void ResetToDefaults()
        {
            MaxReservationDays = 30;
            DefaultReservationHours = 2;
            AllowMultipleReservations = false;
            ParkingOpenHour = new TimeSpan(6, 0, 0);
            ParkingCloseHour = new TimeSpan(22, 0, 0);
            Is24Hours = false;
            MaxVehiclesPerUser = 5;
        }

        public override string ToString()
        {
            return $"Parking Configuration: Max Days: {MaxReservationDays}, " +
                   $"Open: {ParkingOpenHour}-{ParkingCloseHour}, " +
                   $"24h: {Is24Hours}, Multiple Reservations: {AllowMultipleReservations}";
        }
    }
}
