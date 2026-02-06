// Factory Method - Spots
using SystemRejestracjiParkingowej.Models.ParkingSpots;

namespace SystemRejestracjiParkingowej.Factories
{
    public abstract class ParkingSpotCreator
    {
        public abstract ParkingSpotBase CreateParkingSpot(string spotNumber, int zoneId);

        public ParkingSpotBase CreateAndConfigureSpot(string spotNumber, int zoneId)
        {
            var spot = CreateParkingSpot(spotNumber, zoneId);
            spot.Status = "Available";
            return spot;
        }
    }
}
