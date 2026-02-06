// Vehicle base class
using System.ComponentModel.DataAnnotations;

namespace SystemRejestracjiParkingowej.Models.Vehicles
{
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

        protected VehicleBase()
        {
        }

        public abstract string GetDescription();

        public override string ToString()
        {
            return $"{Type}: {Brand} {Model} ({RegistrationNumber}) - {Color}";
        }
    }
}
