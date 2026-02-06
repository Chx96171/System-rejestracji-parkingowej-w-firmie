// Concrete creator - Disabled
using SystemRejestracjiParkingowej.Models.ParkingSpots;

namespace SystemRejestracjiParkingowej.Factories
{
    public class DisabledSpotCreator : ParkingSpotCreator
    {
        public override ParkingSpotBase CreateParkingSpot(string spotNumber, int zoneId)
        {
            return new ParkingSpotBase
            {
                SpotNumber = spotNumber,
                SpotType = "Disabled",
                ParkingZoneId = zoneId,
                Size = "Large",
                HasElectricCharging = false,
                Status = "Available",
                Priority = 1,
                Description = "Miejsce dla osób niepełnosprawnych - poszerzone, priorytetowe"
            };
        }
    }
}
