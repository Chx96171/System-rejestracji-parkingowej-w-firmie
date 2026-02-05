using SystemRejestracjiParkingowej.Models;
using SystemRejestracjiParkingowej.Models.ParkingSpots;

namespace SystemRejestracjiParkingowej.Factories
{
    /// <summary>
    /// Abstract Factory - interfejs dla fabryk stref parkingowych
    /// Wzorzec projektowy: Abstract Factory Pattern
    /// </summary>
    public interface IParkingZoneFactory
    {
        /// <summary>
        /// Tworzy strefę parkingową
        /// </summary>
        /// <param name="name">Nazwa strefy</param>
        /// <param name="description">Opis strefy</param>
        /// <returns>Strefa parkingowa</returns>
        ParkingZone CreateZone(string name, string description);

        /// <summary>
        /// Tworzy miejsce parkingowe dla tej strefy
        /// </summary>
        /// <param name="spotNumber">Numer miejsca</param>
        /// <param name="zoneId">ID strefy</param>
        /// <returns>Miejsce parkingowe</returns>
        ParkingSpotBase CreateSpot(string spotNumber, int zoneId);

        /// <summary>
        /// Tworzy reguły rezerwacji dla tej strefy
        /// </summary>
        /// <returns>Reguły rezerwacji</returns>
        IReservationRule CreateReservationRule();
    }
}
