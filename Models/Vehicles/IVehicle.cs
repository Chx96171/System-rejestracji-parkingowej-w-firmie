// Vehicle interface
namespace SystemRejestracjiParkingowej.Models.Vehicles
{
    public interface IVehicle
    {
        string Type { get; set; }
        string RegistrationNumber { get; set; }
        string Brand { get; set; }
        string Model { get; set; }
        string Color { get; set; }
        string GetDescription();
    }
}
