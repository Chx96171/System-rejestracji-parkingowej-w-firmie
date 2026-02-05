namespace SystemRejestracjiParkingowej.Models.Vehicles
{
    /// <summary>
    /// Klasa reprezentująca motocykl
    /// Produkt Simple Factory Pattern
    /// </summary>
    public class Motorcycle : VehicleBase
    {
        /// <summary>
        /// Pojemność silnika w cm3
        /// </summary>
        public int EngineCapacity { get; set; } = 600;

        /// <summary>
        /// Typ motocykla (Sport, Cruiser, Touring, Adventure)
        /// </summary>
        public string MotorcycleType { get; set; } = "Sport";

        /// <summary>
        /// Konstruktor domyślny
        /// </summary>
        public Motorcycle()
        {
            Type = "Motorcycle";
        }

        /// <summary>
        /// Zwraca szczegółowy opis motocykla
        /// </summary>
        /// <returns>Opis motocykla</returns>
        public override string GetDescription()
        {
            return $"Motocykl {Brand} {Model}, typ: {MotorcycleType}, pojemność: {EngineCapacity}cm3, kolor: {Color}";
        }
    }
}
