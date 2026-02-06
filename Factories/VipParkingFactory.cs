// Concrete factory - VIP
using SystemRejestracjiParkingowej.Models;
using SystemRejestracjiParkingowej.Models.ParkingSpots;

namespace SystemRejestracjiParkingowej.Factories
{
    public class VipParkingFactory : IParkingZoneFactory
    {
        public ParkingZone CreateZone(string name, string description)
        {
            return new ParkingZone
            {
                Name = name,
                Description = description,
                ZoneType = "VIP",
                TotalSpots = 20
            };
        }

        public ParkingSpotBase CreateSpot(string spotNumber, int zoneId)
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
                Description = "Miejsce VIP dla kadry zarządzającej - duże, z ładowarką elektryczną"
            };
        }

        public IReservationRule CreateReservationRule()
        {
            return new ReservationRule
            {
                MaxReservationDays = 90,
                CanCancel = true,
                MinCancellationHours = 1,
                CanExtend = true
            };
        }
    }
}
