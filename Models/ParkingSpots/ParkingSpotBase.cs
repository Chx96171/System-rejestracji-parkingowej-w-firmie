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
        /// Rozmiar miejsca parkingowego
        /// </summary>
        public string Size { get; set; } = "Normal";

        /// <summary>
        /// Czy miejsce posiada ładowanie elektryczne
        /// </summary>
        public bool HasElectricCharging { get; set; } = false;

        /// <summary>
        /// Priorytet miejsca (1 = najwyższy priorytet rezerwacji, 5 = najniższy)
        /// Określa kolejność dostępu do miejsc i maksymalny czas rezerwacji
        /// </summary>
        public int Priority { get; set; } = 3;

        /// <summary>
        /// Opis przeznaczenia miejsca
        /// </summary>
        public string? Description { get; set; }

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
        /// Zwraca szczegółowy opis miejsca parkingowego
        /// </summary>
        /// <returns>Opis miejsca</returns>
        public virtual string GetSpotDescription()
        {
            string chargingInfo = HasElectricCharging ? "z ładowaniem elektrycznym" : "bez ładowania";
            return $"Miejsce {SpotType} nr {SpotNumber}, rozmiar: {Size}, {chargingInfo}";
        }

        /// <summary>
        /// Zwraca szczegółową charakterystykę miejsca
        /// </summary>
        public virtual string GetSpotCharacteristics()
        {
            string chargingInfo = HasElectricCharging ? "z ładowarką 22kW" : "bez ładowania";
            return $"Typ: {SpotType}, Rozmiar: {Size}, {chargingInfo}";
        }
    }
}
