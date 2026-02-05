using System.ComponentModel.DataAnnotations;

namespace SystemRejestracjiParkingowej.Models.ParkingSpots
{
    /// <summary>
    /// Klasa bazowa dla miejsc parkingowych
    /// Używana w Factory Method Pattern
    /// </summary>
    public class ParkingSpotBase : IParkingSpot
    {
        [Required(ErrorMessage = "Numer miejsca parkingowego jest wymagany.")]
        public string SpotNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Typ miejsca parkingowego jest wymagany.")]
        public string SpotType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Status miejsca parkingowego jest wymagany.")]
        public string Status { get; set; } = "Available";

        /// <summary>
        /// Cena za godzinę parkowania
        /// </summary>
        public decimal PricePerHour { get; set; }

        /// <summary>
        /// Rozmiar miejsca parkingowego
        /// </summary>
        public string Size { get; set; } = "Medium";

        /// <summary>
        /// Czy miejsce posiada ładowanie elektryczne
        /// </summary>
        public bool HasElectricCharging { get; set; } = false;

        /// <summary>
        /// ID strefy parkingowej
        /// </summary>
        public int ParkingZoneId { get; set; }

        /// <summary>
        /// Konstruktor domyślny
        /// </summary>
        public ParkingSpotBase()
        {
        }

        /// <summary>
        /// Oblicza koszt parkowania za określoną liczbę godzin
        /// </summary>
        /// <param name="hours">Liczba godzin parkowania</param>
        /// <returns>Całkowity koszt</returns>
        public virtual decimal CalculateCost(int hours)
        {
            return PricePerHour * hours;
        }

        /// <summary>
        /// Zwraca szczegółowy opis miejsca parkingowego
        /// </summary>
        /// <returns>Opis miejsca</returns>
        public virtual string GetSpotDescription()
        {
            string chargingInfo = HasElectricCharging ? "z ładowaniem elektrycznym" : "bez ładowania";
            return $"Miejsce {SpotType} nr {SpotNumber}, rozmiar: {Size}, {PricePerHour} zł/h, {chargingInfo}";
        }
    }
}
