using SystemRejestracjiParkingowej.Models.Vehicles;

namespace SystemRejestracjiParkingowej.Factories
{
    /// <summary>
    /// Simple Factory - tworzy różne typy pojazdów w zależności od przekazanego typu
    /// Wzorzec projektowy: Simple Factory Pattern
    /// </summary>
    public class VehicleFactory
    {
        /// <summary>
        /// Tworzy pojazd na podstawie typu
        /// </summary>
        /// <param name="type">Typ pojazdu: Car, Motorcycle, Truck, Bus</param>
        /// <param name="registrationNumber">Numer rejestracyjny</param>
        /// <param name="brand">Marka pojazdu</param>
        /// <param name="model">Model pojazdu</param>
        /// <param name="color">Kolor pojazdu</param>
        /// <returns>Nowa instancja pojazdu odpowiedniego typu</returns>
        /// <exception cref="ArgumentException">Gdy podano nieznany typ pojazdu</exception>
        public static VehicleBase CreateVehicle(string type, string registrationNumber, string brand, string model, string color)
        {
            VehicleBase vehicle = type.ToLower() switch
            {
                "car" => new Car(),
                "motorcycle" => new Motorcycle(),
                "truck" => new Truck(),
                "bus" => new Bus(),
                _ => throw new ArgumentException($"Nieznany typ pojazdu: {type}")
            };

            vehicle.Type = type;
            vehicle.RegistrationNumber = registrationNumber;
            vehicle.Brand = brand;
            vehicle.Model = model;
            vehicle.Color = color;

            return vehicle;
        }

        /// <summary>
        /// Tworzy pojazd z dodatkowymi parametrami specyficznymi dla typu
        /// </summary>
        /// <param name="type">Typ pojazdu</param>
        /// <param name="registrationNumber">Numer rejestracyjny</param>
        /// <param name="brand">Marka pojazdu</param>
        /// <param name="model">Model pojazdu</param>
        /// <param name="color">Kolor pojazdu</param>
        /// <param name="additionalParams">Dodatkowe parametry specyficzne dla typu pojazdu</param>
        /// <returns>Nowa instancja pojazdu z ustawionymi dodatkowymi parametrami</returns>
        public static VehicleBase CreateVehicleWithParams(string type, string registrationNumber, string brand, string model, string color, Dictionary<string, object>? additionalParams = null)
        {
            var vehicle = CreateVehicle(type, registrationNumber, brand, model, color);

            if (additionalParams != null)
            {
                ApplyAdditionalParameters(vehicle, additionalParams);
            }

            return vehicle;
        }

        /// <summary>
        /// Stosuje dodatkowe parametry specyficzne dla typu pojazdu
        /// </summary>
        /// <param name="vehicle">Pojazd do konfiguracji</param>
        /// <param name="parameters">Parametry do zastosowania</param>
        private static void ApplyAdditionalParameters(VehicleBase vehicle, Dictionary<string, object> parameters)
        {
            switch (vehicle)
            {
                case Car car:
                    if (parameters.ContainsKey("NumberOfDoors"))
                        car.NumberOfDoors = Convert.ToInt32(parameters["NumberOfDoors"]);
                    if (parameters.ContainsKey("BodyType"))
                        car.BodyType = parameters["BodyType"].ToString() ?? "Sedan";
                    break;

                case Motorcycle motorcycle:
                    if (parameters.ContainsKey("EngineCapacity"))
                        motorcycle.EngineCapacity = Convert.ToInt32(parameters["EngineCapacity"]);
                    if (parameters.ContainsKey("MotorcycleType"))
                        motorcycle.MotorcycleType = parameters["MotorcycleType"].ToString() ?? "Sport";
                    break;

                case Truck truck:
                    if (parameters.ContainsKey("LoadCapacity"))
                        truck.LoadCapacity = Convert.ToDouble(parameters["LoadCapacity"]);
                    if (parameters.ContainsKey("NumberOfAxles"))
                        truck.NumberOfAxles = Convert.ToInt32(parameters["NumberOfAxles"]);
                    if (parameters.ContainsKey("HasTrailer"))
                        truck.HasTrailer = Convert.ToBoolean(parameters["HasTrailer"]);
                    break;

                case Bus bus:
                    if (parameters.ContainsKey("SeatingCapacity"))
                        bus.SeatingCapacity = Convert.ToInt32(parameters["SeatingCapacity"]);
                    if (parameters.ContainsKey("BusType"))
                        bus.BusType = parameters["BusType"].ToString() ?? "City";
                    if (parameters.ContainsKey("HasDisabledAccess"))
                        bus.HasDisabledAccess = Convert.ToBoolean(parameters["HasDisabledAccess"]);
                    break;
            }
        }

        /// <summary>
        /// Zwraca listę dostępnych typów pojazdów
        /// </summary>
        /// <returns>Lista nazw typów pojazdów</returns>
        public static List<string> GetAvailableVehicleTypes()
        {
            return new List<string> { "Car", "Motorcycle", "Truck", "Bus" };
        }
    }
}
