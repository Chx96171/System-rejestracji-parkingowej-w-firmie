using SystemRejestracjiParkingowej.Models.ParkingSpots;

namespace SystemRejestracjiParkingowej.Factories
{
    /// <summary>
    /// Konkretny Creator dla miejsc VIP
    /// Factory Method Pattern - Concrete Creator
    /// </summary>
    public class VipSpotCreator : ParkingSpotCreator
    {
        /// <summary>
        /// Tworzy miejsce parkingowe VIP
        /// </summary>
        /// <param name="spotNumber">Numer miejsca</param>
        /// <param name="zoneId">ID strefy parkingowej</param>
        /// <returns>Miejsce parkingowe VIP</returns>
        public override ParkingSpotBase CreateParkingSpot(string spotNumber, int zoneId)
        {
            return new ParkingSpotBase
            {
                SpotNumber = spotNumber,
                SpotType = "VIP",
                ParkingZoneId = zoneId,
                Size = "Large",
                HasElectricCharging = true,
                Status = "Available",
                Priority = 1,
                Description = "Miejsce VIP - duże, z ładowarką, dla kadry zarządzającej"
            };
        }
    }
}
