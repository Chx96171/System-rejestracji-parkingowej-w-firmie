using SystemRejestracjiParkingowej.Models.ParkingSpots;

namespace SystemRejestracjiParkingowej.Factories
{
    /// <summary>
    /// Konkretny Creator dla standardowych miejsc parkingowych
    /// Factory Method Pattern - Concrete Creator
    /// </summary>
    public class StandardSpotCreator : ParkingSpotCreator
    {
        /// <summary>
        /// Tworzy standardowe miejsce parkingowe
        /// </summary>
        /// <param name="spotNumber">Numer miejsca</param>
        /// <param name="zoneId">ID strefy parkingowej</param>
        /// <returns>Standardowe miejsce parkingowe</returns>
        public override ParkingSpotBase CreateParkingSpot(string spotNumber, int zoneId)
        {
            return new ParkingSpotBase
            {
                SpotNumber = spotNumber,
                SpotType = "Standard",
                ParkingZoneId = zoneId,
                PricePerHour = 5.00m,
                Size = "Medium",
                HasElectricCharging = false,
                Status = "Available"
            };
        }
    }
}
