// Concrete factory - Employee
using SystemRejestracjiParkingowej.Models;
using SystemRejestracjiParkingowej.Models.ParkingSpots;

namespace SystemRejestracjiParkingowej.Factories
{
    public class EmployeeParkingFactory : IParkingZoneFactory
    {
        public ParkingZone CreateZone(string name, string description)
        {
            return new ParkingZone
            {
                Name = name,
                Description = description,
                ZoneType = "Employee",
                TotalSpots = 50
            };
        }

        public ParkingSpotBase CreateSpot(string spotNumber, int zoneId)
        {
            return new ParkingSpotBase
            {
                SpotNumber = spotNumber,
                SpotType = "Employee",
                ParkingZoneId = zoneId,
                Size = "Normal",
                HasElectricCharging = false,
                Status = "Available",
                Priority = 3,
                Description = "Standardowe miejsce dla pracowników"
            };
        }

        public IReservationRule CreateReservationRule()
        {
            return new ReservationRule
            {
                MaxReservationDays = 30,
                CanCancel = true,
                MinCancellationHours = 24,
                CanExtend = true
            };
        }
    }
}
