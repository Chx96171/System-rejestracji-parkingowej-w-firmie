using SystemRejestracjiParkingowej.Models.ParkingSpots;

namespace SystemRejestracjiParkingowej.Factories
{
    /// <summary>
    /// Konkretny Creator dla miejsc z ładowaniem elektrycznym
    /// Factory Method Pattern - Concrete Creator
    /// </summary>
    public class ElectricSpotCreator : ParkingSpotCreator
    {
        /// <summary>
        /// Tworzy miejsce parkingowe z ładowaniem elektrycznym
        /// </summary>
        /// <param name="spotNumber">Numer miejsca</param>
        /// <param name="zoneId">ID strefy parkingowej</param>
        /// <returns>Miejsce z ładowaniem elektrycznym</returns>
        public override ParkingSpotBase CreateParkingSpot(string spotNumber, int zoneId)
        {
            return new ParkingSpotBase
            {
                SpotNumber = spotNumber,
                SpotType = "Electric",
                ParkingZoneId = zoneId,
                Size = "Normal",
                HasElectricCharging = true,
                Status = "Available",
                Priority = 2,
                Description = "Miejsce z ładowarką dla pojazdów elektrycznych - 22kW"
            };
        }
    }
}
