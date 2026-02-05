using SystemRejestracjiParkingowej.Models.ParkingSpots;

namespace SystemRejestracjiParkingowej.Factories
{
    /// <summary>
    /// Klasa abstrakcyjna dla Factory Method Pattern
    /// Definiuje metodę fabrykującą do tworzenia miejsc parkingowych
    /// Wzorzec projektowy: Factory Method Pattern
    /// </summary>
    public abstract class ParkingSpotCreator
    {
        /// <summary>
        /// Factory Method - metoda abstrakcyjna do tworzenia miejsc parkingowych
        /// Każda konkretna implementacja zwraca inny typ miejsca
        /// </summary>
        /// <param name="spotNumber">Numer miejsca parkingowego</param>
        /// <param name="zoneId">ID strefy parkingowej</param>
        /// <returns>Nowe miejsce parkingowe</returns>
        public abstract ParkingSpotBase CreateParkingSpot(string spotNumber, int zoneId);

        /// <summary>
        /// Metoda szablonowa - wykorzystuje Factory Method do utworzenia i skonfigurowania miejsca
        /// </summary>
        /// <param name="spotNumber">Numer miejsca</param>
        /// <param name="zoneId">ID strefy</param>
        /// <returns>Skonfigurowane miejsce parkingowe</returns>
        public ParkingSpotBase CreateAndConfigureSpot(string spotNumber, int zoneId)
        {
            var spot = CreateParkingSpot(spotNumber, zoneId);
            spot.Status = "Available";
            return spot;
        }
    }
}
