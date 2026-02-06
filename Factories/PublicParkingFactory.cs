// Concrete factory - Public
using SystemRejestracjiParkingowej.Models;
using SystemRejestracjiParkingowej.Models.ParkingSpots;

namespace SystemRejestracjiParkingowej.Factories
{
    public class PublicParkingFactory : IParkingZoneFactory
    {
        public ParkingZone CreateZone(string name, string description)
        {
            return new ParkingZone
            {
                Name = name,
                Description = description,
                ZoneType = "Public",
                TotalSpots = 100
            };
        }

        public ParkingSpotBase CreateSpot(string spotNumber, int zoneId)
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
                Description = "Standardowe miejsce dla odwiedzających"
            };
        }

        public IReservationRule CreateReservationRule()
        {
            return new ReservationRule
            {
                MaxReservationDays = 7,
                CanCancel = true,
                MinCancellationHours = 2,
                CanExtend = true
            };
        }
    }
}
