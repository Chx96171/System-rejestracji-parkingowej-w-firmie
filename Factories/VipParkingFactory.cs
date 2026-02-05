using SystemRejestracjiParkingowej.Models;
using SystemRejestracjiParkingowej.Models.ParkingSpots;

namespace SystemRejestracjiParkingowej.Factories
{
    /// <summary>
    /// Konkretna fabryka dla parkingu VIP
    /// Abstract Factory Pattern - Concrete Factory
    /// </summary>
    public class VipParkingFactory : IParkingZoneFactory
    {
        /// <summary>
        /// Tworzy strefę parkingową VIP
        /// </summary>
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

        /// <summary>
        /// Tworzy miejsce VIP
        /// </summary>
        public ParkingSpotBase CreateSpot(string spotNumber, int zoneId)
        {
            return new ParkingSpotBase
            {
                SpotNumber = spotNumber,
                SpotType = "VIP",
                ParkingZoneId = zoneId,
                PricePerHour = 20.00m, // Premium cena
                Size = "Large",
                HasElectricCharging = true,
                Status = "Available"
            };
        }

        /// <summary>
        /// Tworzy reguły rezerwacji dla parkingu VIP
        /// </summary>
        public IReservationRule CreateReservationRule()
        {
            return new ReservationRule
            {
                MaxReservationDays = 90, // Bardzo długie rezerwacje
                CanCancel = true,
                MinCancellationHours = 1, // Krótki czas anulowania
                CanExtend = true
            };
        }
    }
}
