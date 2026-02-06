// Concrete creator - VIP
using SystemRejestracjiParkingowej.Models.ParkingSpots;

namespace SystemRejestracjiParkingowej.Factories
{
    public class VipSpotCreator : ParkingSpotCreator
    {
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
