// Abstract Factory - Zones
using SystemRejestracjiParkingowej.Models;
using SystemRejestracjiParkingowej.Models.ParkingSpots;

namespace SystemRejestracjiParkingowej.Factories
{
    public interface IParkingZoneFactory
    {
        ParkingZone CreateZone(string name, string description);

        ParkingSpotBase CreateSpot(string spotNumber, int zoneId);

        IReservationRule CreateReservationRule();
    }
}
