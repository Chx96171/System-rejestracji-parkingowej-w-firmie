using System.ComponentModel.DataAnnotations;

namespace SystemRejestracjiParkingowej.Models
{
    public class Vehicle
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Typ pojazdu jest wymagany")]
        [Display(Name = "Typ pojazdu")]
        public string VehicleType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Numer rejestracyjny jest wymagany")]
        [Display(Name = "Numer rejestracyjny")]
        public string RegistrationNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Marka pojazdu jest wymagana")]
        [Display(Name = "Marka")]
        public string Brand { get; set; } = string.Empty;

        [Required(ErrorMessage = "Model pojazdu jest wymagany")]
        [Display(Name = "Model")]
        public string Model { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kolor pojazdu jest wymagany")]
        [Display(Name = "Kolor")]
        public string Color { get; set; } = string.Empty;

        public string? UserId { get; set; }

        public ApplicationUser? User { get; set; }
    }
}