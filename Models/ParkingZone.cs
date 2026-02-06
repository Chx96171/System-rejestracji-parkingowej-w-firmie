// Model - Parking zone
using System.ComponentModel.DataAnnotations;

namespace SystemRejestracjiParkingowej.Models
{
    public class ParkingZone
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa strefy jest wymagana")]
        [Display(Name = "Nazwa strefy")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Opis strefy jest wymagany")]
        [Display(Name = "Opis")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Typ strefy jest wymagany")]
        [Display(Name = "Typ strefy")]
        public string ZoneType { get; set; } = string.Empty;

        [Display(Name = "Liczba miejsc")]
        public int TotalSpots { get; set; }

        public ICollection<ParkingSpot> ParkingSpots { get; set; } = new List<ParkingSpot>();
    }
}