using System.ComponentModel.DataAnnotations;

namespace SystemRejestracjiParkingowej.Models
{
    public class Vehicle
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Typ pojazdu jest wymagany.")]
        public string VehicleType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Numer rejestracyjny pojazdu jest wymagany.")]
        public string RegistrationNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Marka pojazdu jest wymagana.")]
        public string Brand { get; set; } = string.Empty;

        [Required(ErrorMessage = "Model pojazdu jest wymagany.")]
        public string Model { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kolor pojazdu jest wymagany.")]
        public string Color { get; set; } = string.Empty;

        // Usuń [Required] z UserId
        public string? UserId { get; set; } // Pole teraz opcjonalne

        public ApplicationUser? User { get; set; } // Ustanawia relację na podstawie UserId
    }
}