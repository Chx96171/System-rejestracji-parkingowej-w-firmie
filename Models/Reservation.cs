using System.ComponentModel.DataAnnotations;

namespace SystemRejestracjiParkingowej.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Data rozpoczęcia rezerwacji jest wymagana.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Data zakończenia rezerwacji jest wymagana.")]
        public DateTime EndDate { get; set; }

        public string Status { get; set; } = "Pending"; // Domyślnie przypisany Status

        public int ParkingSpotId { get; set; }
        public ParkingSpot? ParkingSpot { get; set; } // Pole nawigacyjne (opcjonalne)

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; } // Pole nawigacyjne (opcjonalne)

        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; } // Pole nawigacyjne (opcjonalne)
    }
}