// Parking spot base
using System.ComponentModel.DataAnnotations;

namespace SystemRejestracjiParkingowej.Models.ParkingSpots
{
    public class ParkingSpotBase : IParkingSpot
    {
        [Required(ErrorMessage = "Numer miejsca parkingowego jest wymagany.")]
        public string SpotNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Typ miejsca parkingowego jest wymagany.")]
        public string SpotType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Status miejsca parkingowego jest wymagany.")]
        public string Status { get; set; } = "Available";

        public string Size { get; set; } = "Normal";
        public bool HasElectricCharging { get; set; } = false;
        public int Priority { get; set; } = 3;
        public string? Description { get; set; }
        public int ParkingZoneId { get; set; }

        public ParkingSpotBase()
        {
        }

        public virtual string GetSpotDescription()
        {
            string chargingInfo = HasElectricCharging ? "z ładowaniem elektrycznym" : "bez ładowania";
            return $"Miejsce {SpotType} nr {SpotNumber}, rozmiar: {Size}, {chargingInfo}";
        }

        public virtual string GetSpotCharacteristics()
        {
            string chargingInfo = HasElectricCharging ? "z ładowarką 22kW" : "bez ładowania";
            return $"Typ: {SpotType}, Rozmiar: {Size}, {chargingInfo}";
        }
    }
}
