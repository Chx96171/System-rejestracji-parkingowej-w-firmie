using System.ComponentModel.DataAnnotations;

namespace SystemRejestracjiParkingowej.Models.Vehicles
{
    /// <summary>
    /// Klasa abstrakcyjna bazowa dla wszystkich typów pojazdów
    /// Wykorzystywana w Simple Factory Pattern
    /// </summary>
    public abstract class VehicleBase : IVehicle
    {
        [Required(ErrorMessage = "Typ pojazdu jest wymagany.")]
        public string Type { get; set; } = string.Empty;

        [Required(ErrorMessage = "Numer rejestracyjny pojazdu jest wymagany.")]
        public string RegistrationNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Marka pojazdu jest wymagana.")]
        public string Brand { get; set; } = string.Empty;

        [Required(ErrorMessage = "Model pojazdu jest wymagany.")]
        public string Model { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kolor pojazdu jest wymagany.")]
        public string Color { get; set; } = string.Empty;

        /// <summary>
        /// Konstruktor chroniony - klasa może być tylko dziedziczona
        /// </summary>
        protected VehicleBase()
        {
        }

        /// <summary>
        /// Metoda abstrakcyjna - każdy typ pojazdu musi zaimplementować swój własny opis
        /// </summary>
        /// <returns>Opis pojazdu</returns>
        public abstract string GetDescription();

        /// <summary>
        /// Podstawowa informacja o pojeździe
        /// </summary>
        /// <returns>Formatowany string z podstawowymi informacjami</returns>
        public override string ToString()
        {
            return $"{Type}: {Brand} {Model} ({RegistrationNumber}) - {Color}";
        }
    }
}
