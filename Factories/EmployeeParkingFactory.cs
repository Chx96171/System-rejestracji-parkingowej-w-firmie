using SystemRejestracjiParkingowej.Models;
using SystemRejestracjiParkingowej.Models.ParkingSpots;

namespace SystemRejestracjiParkingowej.Factories
{
    /// <summary>
    /// Konkretna fabryka dla parkingu pracowniczego
    /// Abstract Factory Pattern - Concrete Factory
    /// </summary>
    public class EmployeeParkingFactory : IParkingZoneFactory
    {
        /// <summary>
        /// Tworzy strefę parkingową dla pracowników
        /// </summary>
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

        /// <summary>
        /// Tworzy miejsce dla parkingu pracowniczego
        /// </summary>
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

        /// <summary>
        /// Tworzy reguły rezerwacji dla parkingu pracowniczego
        /// </summary>
        public IReservationRule CreateReservationRule()
        {
            return new ReservationRule
            {
                MaxReservationDays = 30, // Dłuższe rezerwacje dla pracowników
                CanCancel = true,
                MinCancellationHours = 24,
                CanExtend = true
            };
        }
    }
}
