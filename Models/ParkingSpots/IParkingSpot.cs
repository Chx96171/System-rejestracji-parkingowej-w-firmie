namespace SystemRejestracjiParkingowej.Models.ParkingSpots
{
    /// <summary>
    /// Interfejs dla miejsc parkingowych
    /// </summary>
    public interface IParkingSpot
    {
        /// <summary>
        /// Numer miejsca parkingowego
        /// </summary>
        string SpotNumber { get; set; }

        /// <summary>
        /// Typ miejsca parkingowego
        /// </summary>
        string SpotType { get; set; }

        /// <summary>
        /// Status miejsca (Available, Occupied, Reserved)
        /// </summary>
        string Status { get; set; }

        /// <summary>
        /// Rozmiar miejsca (Small, Medium, Large, XLarge)
        /// </summary>
        string Size { get; set; }

        /// <summary>
        /// Czy miejsce ma dostęp do ładowania elektrycznego
        /// </summary>
        bool HasElectricCharging { get; set; }

        /// <summary>
        /// Zwraca szczegółowy opis miejsca parkingowego
        /// </summary>
        /// <returns>Opis miejsca</returns>
        string GetSpotDescription();

        /// <summary>
        /// Zwraca szczegółową charakterystykę miejsca parkingowego
        /// </summary>
        /// <returns>Charakterystyka miejsca</returns>
        string GetSpotCharacteristics();
    }
}
