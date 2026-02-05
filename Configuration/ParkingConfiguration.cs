namespace SystemRejestracjiParkingowej.Configuration
{
    /// <summary>
    /// Singleton - zarządza globalną konfiguracją systemu parkingowego
    /// Wzorzec projektowy: Singleton Pattern (thread-safe z lazy initialization)
    /// </summary>
    public sealed class ParkingConfiguration
    {
        // Lazy<T> zapewnia thread-safety i lazy initialization
        private static readonly Lazy<ParkingConfiguration> _instance = 
            new Lazy<ParkingConfiguration>(() => new ParkingConfiguration());

        /// <summary>
        /// Maksymalna liczba dni rezerwacji
        /// </summary>
        public int MaxReservationDays { get; set; }

        /// <summary>
        /// Domyślna liczba godzin rezerwacji
        /// </summary>
        public int DefaultReservationHours { get; set; }

        /// <summary>
        /// Czy użytkownik może mieć wiele aktywnych rezerwacji jednocześnie
        /// </summary>
        public bool AllowMultipleReservations { get; set; }

        /// <summary>
        /// Godzina otwarcia parkingu
        /// </summary>
        public TimeSpan ParkingOpenHour { get; set; }

        /// <summary>
        /// Godzina zamknięcia parkingu
        /// </summary>
        public TimeSpan ParkingCloseHour { get; set; }

        /// <summary>
        /// Czy parking jest czynny 24/7
        /// </summary>
        public bool Is24Hours { get; set; }

        /// <summary>
        /// Maksymalna liczba pojazdów na użytkownika
        /// </summary>
        public int MaxVehiclesPerUser { get; set; }

        /// <summary>
        /// Prywatny konstruktor - zapobiega tworzeniu instancji z zewnątrz
        /// </summary>
        private ParkingConfiguration()
        {
            // Wartości domyślne
            MaxReservationDays = 30;
            DefaultReservationHours = 2;
            AllowMultipleReservations = false;
            ParkingOpenHour = new TimeSpan(6, 0, 0); // 6:00
            ParkingCloseHour = new TimeSpan(22, 0, 0); // 22:00
            Is24Hours = false;
            MaxVehiclesPerUser = 5;
        }

        /// <summary>
        /// Publiczna właściwość dostępu do instancji Singleton
        /// </summary>
        public static ParkingConfiguration Instance => _instance.Value;

        /// <summary>
        /// Metoda do pobierania instancji (alternatywa dla property)
        /// </summary>
        /// <returns>Instancja ParkingConfiguration</returns>
        public static ParkingConfiguration GetInstance()
        {
            return _instance.Value;
        }

        /// <summary>
        /// Sprawdza czy parking jest otwarty w danej godzinie
        /// </summary>
        /// <param name="time">Czas do sprawdzenia</param>
        /// <returns>True jeśli parking jest otwarty</returns>
        public bool IsParkingOpen(TimeSpan time)
        {
            if (Is24Hours)
                return true;

            return time >= ParkingOpenHour && time <= ParkingCloseHour;
        }

        /// <summary>
        /// Waliduje liczbę dni rezerwacji
        /// </summary>
        /// <param name="days">Liczba dni</param>
        /// <returns>True jeśli liczba dni jest prawidłowa</returns>
        public bool ValidateReservationDays(int days)
        {
            return days > 0 && days <= MaxReservationDays;
        }

        /// <summary>
        /// Resetuje konfigurację do wartości domyślnych
        /// </summary>
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

        /// <summary>
        /// Zwraca informację o konfiguracji
        /// </summary>
        /// <returns>String z konfiguracją</returns>
        public override string ToString()
        {
            return $"Parking Configuration: Max Days: {MaxReservationDays}, " +
                   $"Open: {ParkingOpenHour}-{ParkingCloseHour}, " +
                   $"24h: {Is24Hours}, Multiple Reservations: {AllowMultipleReservations}";
        }
    }
}
