using Microsoft.EntityFrameworkCore;
using SystemRejestracjiParkingowej.Configuration;
using SystemRejestracjiParkingowej.Data;
using SystemRejestracjiParkingowej.Factories;
using SystemRejestracjiParkingowej.Models;
using SystemRejestracjiParkingowej.Models.Vehicles;

namespace SystemRejestracjiParkingowej.Services
{
    /// <summary>
    /// Serwis zarządzania pojazdami
    /// Wykorzystuje Simple Factory Pattern do tworzenia pojazdów
    /// </summary>
    public class VehicleService : IVehicleService
    {
        private readonly ApplicationDbContext _context;
        private readonly ParkingConfiguration _config;

        public VehicleService(ApplicationDbContext context)
        {
            _context = context;
            _config = ParkingConfiguration.Instance;
        }

        /// <summary>
        /// Tworzy pojazd przy użyciu VehicleFactory i zapisuje do bazy
        /// </summary>
        public async Task<Vehicle> CreateVehicle(string type, string registrationNumber, string brand, string model, string color, string userId)
        {
            // Użyj Simple Factory do walidacji typu i utworzenia obiektu
            var vehicleBase = VehicleFactory.CreateVehicle(type, registrationNumber, brand, model, color);

            // Konwertuj na model bazodanowy
            var vehicle = new Vehicle
            {
                VehicleType = vehicleBase.Type,
                RegistrationNumber = vehicleBase.RegistrationNumber,
                Brand = vehicleBase.Brand,
                Model = vehicleBase.Model,
                Color = vehicleBase.Color,
                UserId = userId
            };

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return vehicle;
        }

        /// <summary>
        /// Pobiera pojazdy użytkownika
        /// </summary>
        public async Task<List<Vehicle>> GetUserVehicles(string userId)
        {
            return await _context.Vehicles
                .Where(v => v.UserId == userId)
                .ToListAsync();
        }

        /// <summary>
        /// Sprawdza czy użytkownik może dodać nowy pojazd
        /// </summary>
        public async Task<bool> CanAddVehicle(string userId)
        {
            var vehicleCount = await _context.Vehicles
                .Where(v => v.UserId == userId)
                .CountAsync();

            return vehicleCount < _config.MaxVehiclesPerUser;
        }

        /// <summary>
        /// Konwertuje pojazd z bazy danych na obiekt VehicleBase z fabryki
        /// </summary>
        public VehicleBase ConvertToVehicleBase(Vehicle vehicle)
        {
            return VehicleFactory.CreateVehicle(
                vehicle.VehicleType,
                vehicle.RegistrationNumber,
                vehicle.Brand,
                vehicle.Model,
                vehicle.Color
            );
        }
    }
}
