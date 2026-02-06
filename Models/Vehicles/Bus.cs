// Vehicle - Bus
namespace SystemRejestracjiParkingowej.Models.Vehicles
{
    public class Bus : VehicleBase
    {
        public int SeatingCapacity { get; set; } = 50;
        public string BusType { get; set; } = "City";
        public bool HasDisabledAccess { get; set; } = true;

        public Bus()
        {
            Type = "Bus";
        }

        public override string GetDescription()
        {
            string accessInfo = HasDisabledAccess ? "z dostępem dla niepełnosprawnych" : "bez dostępu dla niepełnosprawnych";
            return $"Autobus {Brand} {Model}, typ: {BusType}, {SeatingCapacity} miejsc, {accessInfo}, kolor: {Color}";
        }
    }
}
