using Microsoft.AspNetCore.Identity;

namespace SystemRejestracjiParkingowej.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumberForParking { get; set; } = string.Empty;

        public DateTime RegisteredAt { get; set; } = DateTime.Now;

        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}