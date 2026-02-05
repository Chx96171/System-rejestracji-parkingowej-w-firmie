using SystemRejestracjiParkingowej.Models;
using SystemRejestracjiParkingowej.Models.Vehicles;

namespace SystemRejestracjiParkingowej.Services
{
    /// <summary>
    /// Interfejs serwisu pojazdów
    /// </summary>
    public interface IVehicleService
    {
        /// <summary>
        /// Tworzy pojazd przy użyciu fabryki
        /// </summary>
        /// <param name="type">Typ pojazdu</param>
        /// <param name="registrationNumber">Numer rejestracyjny</param>
        /// <param name="brand">Marka</param>
        /// <param name="model">Model</param>
        /// <param name="color">Kolor</param>
        /// <param name="userId">ID użytkownika</param>
        /// <returns>Utworzony pojazd bazodanowy</returns>
        Task<Vehicle> CreateVehicle(string type, string registrationNumber, string brand, string model, string color, string userId);

        /// <summary>
        /// Pobiera pojazdy użytkownika
        /// </summary>
        /// <param name="userId">ID użytkownika</param>
        /// <returns>Lista pojazdów</returns>
        Task<List<Vehicle>> GetUserVehicles(string userId);

        /// <summary>
        /// Waliduje czy użytkownik może dodać nowy pojazd
        /// </summary>
        /// <param name="userId">ID użytkownika</param>
        /// <returns>True jeśli może dodać</returns>
        Task<bool> CanAddVehicle(string userId);

        /// <summary>
        /// Konwertuje pojazd bazodanowy na obiekt fabryczny
        /// </summary>
        /// <param name="vehicle">Pojazd z bazy danych</param>
        /// <returns>Obiekt pojazdu z fabryki</returns>
        VehicleBase ConvertToVehicleBase(Vehicle vehicle);
    }
}
