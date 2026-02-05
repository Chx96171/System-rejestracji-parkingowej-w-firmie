namespace SystemRejestracjiParkingowej.Models.Vehicles
{
    /// <summary>
    /// Interfejs dla wszystkich typów pojazdów w systemie parkingowym
    /// </summary>
    public interface IVehicle
    {
        /// <summary>
        /// Typ pojazdu (Car, Motorcycle, Truck, Bus)
        /// </summary>
        string Type { get; set; }

        /// <summary>
        /// Numer rejestracyjny pojazdu
        /// </summary>
        string RegistrationNumber { get; set; }

        /// <summary>
        /// Marka pojazdu
        /// </summary>
        string Brand { get; set; }

        /// <summary>
        /// Model pojazdu
        /// </summary>
        string Model { get; set; }

        /// <summary>
        /// Kolor pojazdu
        /// </summary>
        string Color { get; set; }

        /// <summary>
        /// Pobiera szczegółowy opis pojazdu
        /// </summary>
        /// <returns>Opis pojazdu</returns>
        string GetDescription();
    }
}
