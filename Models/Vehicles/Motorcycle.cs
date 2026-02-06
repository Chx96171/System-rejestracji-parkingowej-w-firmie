// Vehicle - Motorcycle
namespace SystemRejestracjiParkingowej.Models.Vehicles
{
    public class Motorcycle : VehicleBase
    {
        public int EngineCapacity { get; set; } = 600;
        public string MotorcycleType { get; set; } = "Sport";

        public Motorcycle()
        {
            Type = "Motorcycle";
        }

        public override string GetDescription()
        {
            return $"Motocykl {Brand} {Model}, typ: {MotorcycleType}, pojemność: {EngineCapacity}cm3, kolor: {Color}";
        }
    }
}
