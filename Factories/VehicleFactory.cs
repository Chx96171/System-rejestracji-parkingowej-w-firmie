// Simple Factory - Vehicles
using SystemRejestracjiParkingowej.Models.Vehicles;

namespace SystemRejestracjiParkingowej.Factories
{
    public class VehicleFactory
    {
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

        public static VehicleBase CreateVehicleWithParams(string type, string registrationNumber, string brand, string model, string color, Dictionary<string, object>? additionalParams = null)
        {
            var vehicle = CreateVehicle(type, registrationNumber, brand, model, color);

            if (additionalParams != null)
            {
                ApplyAdditionalParameters(vehicle, additionalParams);
            }

            return vehicle;
        }

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

        public static List<string> GetAvailableVehicleTypes()
        {
            return new List<string> { "Car", "Motorcycle", "Truck", "Bus" };
        }
    }
}
