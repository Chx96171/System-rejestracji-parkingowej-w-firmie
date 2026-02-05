using SystemRejestracjiParkingowej.Models;
using SystemRejestracjiParkingowej.Models.Vehicles;

namespace SystemRejestracjiParkingowej.Interpreters
{
    /// <summary>
    /// Kontekst dla interpretera reguł parkingowych
    /// Zawiera wszystkie dane potrzebne do walidacji reguł
    /// </summary>
    public class ParkingContext
    {
        /// <summary>
        /// Użytkownik wykonujący operację
        /// </summary>
        public ApplicationUser? User { get; set; }

        /// <summary>
        /// Rola użytkownika
        /// </summary>
        public string? UserRole { get; set; }

        /// <summary>
        /// Pojazd
        /// </summary>
        public VehicleBase? Vehicle { get; set; }

        /// <summary>
        /// Typ pojazdu (dla backward compatibility z istniejącym modelem)
        /// </summary>
        public string? VehicleType { get; set; }

        /// <summary>
        /// Strefa parkingowa
        /// </summary>
        public ParkingZone? Zone { get; set; }

        /// <summary>
        /// Typ strefy
        /// </summary>
        public string? ZoneType { get; set; }

        /// <summary>
        /// Data i czas operacji
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Konstruktor domyślny
        /// </summary>
        public ParkingContext()
        {
            DateTime = DateTime.Now;
        }

        /// <summary>
        /// Konstruktor z parametrami
        /// </summary>
        public ParkingContext(ApplicationUser? user, string? userRole, VehicleBase? vehicle, ParkingZone? zone)
        {
            User = user;
            UserRole = userRole;
            Vehicle = vehicle;
            VehicleType = vehicle?.Type;
            Zone = zone;
            ZoneType = zone?.ZoneType;
            DateTime = DateTime.Now;
        }
    }
}
