// Service interface - Vehicles
using SystemRejestracjiParkingowej.Models;
using SystemRejestracjiParkingowej.Models.Vehicles;

namespace SystemRejestracjiParkingowej.Services
{
    public interface IVehicleService
    {
        Task<Vehicle> CreateVehicle(string type, string registrationNumber, string brand, string model, string color, string userId);
        Task<List<Vehicle>> GetUserVehicles(string userId);
        Task<bool> CanAddVehicle(string userId);
        VehicleBase ConvertToVehicleBase(Vehicle vehicle);
    }
}
