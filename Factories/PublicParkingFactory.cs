using SystemRejestracjiParkingowej.Models;
using SystemRejestracjiParkingowej.Models.ParkingSpots;

namespace SystemRejestracjiParkingowej.Factories
{
    /// <summary>
    /// Konkretna fabryka dla publicznego parkingu
    /// Abstract Factory Pattern - Concrete Factory
    /// </summary>
    public class PublicParkingFactory : IParkingZoneFactory
    {
        /// <summary>
        /// Tworzy publiczną strefę parkingową
        /// </summary>
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

        /// <summary>
        /// Tworzy standardowe miejsce dla publicznego parkingu
        /// </summary>
        public ParkingSpotBase CreateSpot(string spotNumber, int zoneId)
        {
            return new ParkingSpotBase
            {
                SpotNumber = spotNumber,
                SpotType = "Standard",
                ParkingZoneId = zoneId,
                PricePerHour = 6.00m,
                Size = "Medium",
                HasElectricCharging = false,
                Status = "Available"
            };
        }

        /// <summary>
        /// Tworzy reguły rezerwacji dla publicznego parkingu
        /// </summary>
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
