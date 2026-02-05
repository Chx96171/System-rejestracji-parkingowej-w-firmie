namespace SystemRejestracjiParkingowej.Models.Vehicles
{
    /// <summary>
    /// Klasa reprezentująca samochód osobowy
    /// Produkt Simple Factory Pattern
    /// </summary>
    public class Car : VehicleBase
    {
        /// <summary>
        /// Liczba drzwi w samochodzie
        /// </summary>
        public int NumberOfDoors { get; set; } = 4;

        /// <summary>
        /// Typ nadwozia (Sedan, SUV, Hatchback, Coupe)
        /// </summary>
        public string BodyType { get; set; } = "Sedan";

        /// <summary>
        /// Konstruktor domyślny
        /// </summary>
        public Car()
        {
            Type = "Car";
        }

        /// <summary>
        /// Zwraca szczegółowy opis samochodu osobowego
        /// </summary>
        /// <returns>Opis samochodu</returns>
        public override string GetDescription()
        {
            return $"Samochód osobowy {Brand} {Model}, nadwozie: {BodyType}, {NumberOfDoors} drzwi, kolor: {Color}";
        }
    }
}
