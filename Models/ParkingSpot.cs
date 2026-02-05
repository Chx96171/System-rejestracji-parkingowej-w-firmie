using System.ComponentModel.DataAnnotations;

namespace SystemRejestracjiParkingowej.Models
{
    public class ParkingSpot
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Numer miejsca parkingowego jest wymagany.")]
        public string SpotNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Typ miejsca parkingowego jest wymagany.")]
        public string SpotType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Status miejsca parkingowego jest wymagany.")]
        public string Status { get; set; } = "Available";

        [Range(1, int.MaxValue, ErrorMessage = "Strefa parkingowa jest wymagana.")]
        public int ParkingZoneId { get; set; }

        public ParkingZone? ParkingZone { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Automatyczne przypisanie bieżącej daty dla nowych rekordów
    }
}