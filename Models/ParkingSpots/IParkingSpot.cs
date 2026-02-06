// Parking spot interface
namespace SystemRejestracjiParkingowej.Models.ParkingSpots
{
    public interface IParkingSpot
    {
        string SpotNumber { get; set; }
        string SpotType { get; set; }
        string Status { get; set; }
        string Size { get; set; }
        bool HasElectricCharging { get; set; }
        string GetSpotDescription();
        string GetSpotCharacteristics();
    }
}
