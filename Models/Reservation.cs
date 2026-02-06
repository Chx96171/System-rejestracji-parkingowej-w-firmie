// Model - Reservation
using System.ComponentModel.DataAnnotations;

namespace SystemRejestracjiParkingowej.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Data rozpoczęcia jest wymagana")]
        [Display(Name = "Data rozpoczęcia")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Data zakończenia jest wymagana")]
        [Display(Name = "Data zakończenia")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; } = "Pending";

        [Display(Name = "Miejsce parkingowe")]
        public int ParkingSpotId { get; set; }
        public ParkingSpot? ParkingSpot { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        [Display(Name = "Pojazd")]
        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }
    }
}