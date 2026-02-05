namespace SystemRejestracjiParkingowej.Models.Vehicles
{
    /// <summary>
    /// Klasa reprezentująca ciężarówkę
    /// Produkt Simple Factory Pattern
    /// </summary>
    public class Truck : VehicleBase
    {
        /// <summary>
        /// Ładowność w tonach
        /// </summary>
        public double LoadCapacity { get; set; } = 3.5;

        /// <summary>
        /// Liczba osi
        /// </summary>
        public int NumberOfAxles { get; set; } = 2;

        /// <summary>
        /// Czy posiada przyczepę
        /// </summary>
        public bool HasTrailer { get; set; } = false;

        /// <summary>
        /// Konstruktor domyślny
        /// </summary>
        public Truck()
        {
            Type = "Truck";
        }

        /// <summary>
        /// Zwraca szczegółowy opis ciężarówki
        /// </summary>
        /// <returns>Opis ciężarówki</returns>
        public override string GetDescription()
        {
            string trailerInfo = HasTrailer ? "z przyczepą" : "bez przyczepy";
            return $"Ciężarówka {Brand} {Model}, ładowność: {LoadCapacity}t, {NumberOfAxles} osie, {trailerInfo}, kolor: {Color}";
        }
    }
}
