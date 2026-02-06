// Vehicle - Truck
namespace SystemRejestracjiParkingowej.Models.Vehicles
{
    public class Truck : VehicleBase
    {
        public double LoadCapacity { get; set; } = 3.5;
        public int NumberOfAxles { get; set; } = 2;
        public bool HasTrailer { get; set; } = false;

        public Truck()
        {
            Type = "Truck";
        }

        public override string GetDescription()
        {
            string trailerInfo = HasTrailer ? "z przyczepą" : "bez przyczepy";
            return $"Ciężarówka {Brand} {Model}, ładowność: {LoadCapacity}t, {NumberOfAxles} osie, {trailerInfo}, kolor: {Color}";
        }
    }
}
