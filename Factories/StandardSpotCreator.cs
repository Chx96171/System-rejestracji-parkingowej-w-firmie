// Concrete creator - Standard
using SystemRejestracjiParkingowej.Models.ParkingSpots;

namespace SystemRejestracjiParkingowej.Factories
{
    public class StandardSpotCreator : ParkingSpotCreator
    {
        public override ParkingSpotBase CreateParkingSpot(string spotNumber, int zoneId)
        {
            return new ParkingSpotBase
            {
                SpotNumber = spotNumber,
                SpotType = "Standard",
                ParkingZoneId = zoneId,
                Size = "Normal",
                HasElectricCharging = false,
                Status = "Available",
                Priority = 3,
                Description = "Standardowe miejsce parkingowe dla pracowników"
            };
        }
    }
}
