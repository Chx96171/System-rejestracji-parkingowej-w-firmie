// Vehicle - Car
namespace SystemRejestracjiParkingowej.Models.Vehicles
{
    public class Car : VehicleBase
    {
        public int NumberOfDoors { get; set; } = 4;
        public string BodyType { get; set; } = "Sedan";

        public Car()
        {
            Type = "Car";
        }

        public override string GetDescription()
        {
            return $"Samochód osobowy {Brand} {Model}, nadwozie: {BodyType}, {NumberOfDoors} drzwi, kolor: {Color}";
        }
    }
}
