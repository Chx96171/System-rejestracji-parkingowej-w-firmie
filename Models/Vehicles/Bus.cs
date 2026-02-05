namespace SystemRejestracjiParkingowej.Models.Vehicles
{
    /// <summary>
    /// Klasa reprezentująca autobus
    /// Produkt Simple Factory Pattern
    /// </summary>
    public class Bus : VehicleBase
    {
        /// <summary>
        /// Liczba miejsc siedzących
        /// </summary>
        public int SeatingCapacity { get; set; } = 50;

        /// <summary>
        /// Typ autobusu (City, Intercity, Tourist, School)
        /// </summary>
        public string BusType { get; set; } = "City";

        /// <summary>
        /// Czy posiada miejsce dla osób niepełnosprawnych
        /// </summary>
        public bool HasDisabledAccess { get; set; } = true;

        /// <summary>
        /// Konstruktor domyślny
        /// </summary>
        public Bus()
        {
            Type = "Bus";
        }

        /// <summary>
        /// Zwraca szczegółowy opis autobusu
        /// </summary>
        /// <returns>Opis autobusu</returns>
        public override string GetDescription()
        {
            string accessInfo = HasDisabledAccess ? "z dostępem dla niepełnosprawnych" : "bez dostępu dla niepełnosprawnych";
            return $"Autobus {Brand} {Model}, typ: {BusType}, {SeatingCapacity} miejsc, {accessInfo}, kolor: {Color}";
        }
    }
}
