// Interpreter - Context
using SystemRejestracjiParkingowej.Models;
using SystemRejestracjiParkingowej.Models.Vehicles;

namespace SystemRejestracjiParkingowej.Interpreters
{
    public class ParkingContext
    {
        public ApplicationUser? User { get; set; }

        public string? UserRole { get; set; }

        public VehicleBase? Vehicle { get; set; }

        public string? VehicleType { get; set; }

        public ParkingZone? Zone { get; set; }

        public string? ZoneType { get; set; }

        public DateTime DateTime { get; set; }

        public ParkingContext()
        {
            DateTime = DateTime.Now;
        }

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
