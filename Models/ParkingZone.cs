using System.ComponentModel.DataAnnotations;

namespace SystemRejestracjiParkingowej.Models
{
    public class ParkingZone
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa strefy jest wymagana.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Opis strefy parkingowej jest wymagany.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Typ strefy parkingowej jest wymagany.")]
        public string ZoneType { get; set; } = string.Empty;

        public int TotalSpots { get; set; }

        public ICollection<ParkingSpot> ParkingSpots { get; set; } = new List<ParkingSpot>();
    }
}