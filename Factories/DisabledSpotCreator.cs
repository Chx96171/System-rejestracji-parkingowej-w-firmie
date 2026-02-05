using SystemRejestracjiParkingowej.Models.ParkingSpots;

namespace SystemRejestracjiParkingowej.Factories
{
    /// <summary>
    /// Konkretny Creator dla miejsc dla osób niepełnosprawnych
    /// Factory Method Pattern - Concrete Creator
    /// </summary>
    public class DisabledSpotCreator : ParkingSpotCreator
    {
        /// <summary>
        /// Tworzy miejsce parkingowe dla osób niepełnosprawnych
        /// </summary>
        /// <param name="spotNumber">Numer miejsca</param>
        /// <param name="zoneId">ID strefy parkingowej</param>
        /// <returns>Miejsce dla osób niepełnosprawnych</returns>
        public override ParkingSpotBase CreateParkingSpot(string spotNumber, int zoneId)
        {
            return new ParkingSpotBase
            {
                SpotNumber = spotNumber,
                SpotType = "Disabled",
                ParkingZoneId = zoneId,
                PricePerHour = 0.00m, // Darmowe dla osób niepełnosprawnych
                Size = "Large",
                HasElectricCharging = false,
                Status = "Available"
            };
        }
    }
}
