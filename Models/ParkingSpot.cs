// Model - Parking spot
using System.ComponentModel.DataAnnotations;

namespace SystemRejestracjiParkingowej.Models
{
    public class ParkingSpot
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Numer miejsca jest wymagany")]
        [Display(Name = "Numer miejsca")]
        public string SpotNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Typ miejsca jest wymagany")]
        [Display(Name = "Typ miejsca")]
        public string SpotType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Status jest wymagany")]
        [Display(Name = "Status")]
        public string Status { get; set; } = "Available";

        [Range(1, int.MaxValue, ErrorMessage = "Strefa parkingowa jest wymagana")]
        [Display(Name = "Strefa parkingowa")]
        public int ParkingZoneId { get; set; }

        public ParkingZone? ParkingZone { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}