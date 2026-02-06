// Concrete creator - Electric
using SystemRejestracjiParkingowej.Models.ParkingSpots;

namespace SystemRejestracjiParkingowej.Factories
{
    public class ElectricSpotCreator : ParkingSpotCreator
    {
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
