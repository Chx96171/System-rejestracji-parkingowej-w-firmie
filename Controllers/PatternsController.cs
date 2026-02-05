using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SystemRejestracjiParkingowej.Configuration;
using SystemRejestracjiParkingowej.Factories;
using SystemRejestracjiParkingowej.Interpreters;
using SystemRejestracjiParkingowej.Models;
using SystemRejestracjiParkingowej.Models.Vehicles;
using System.Text;

namespace SystemRejestracjiParkingowej.Controllers
{
    /// <summary>
    /// Kontroler demonstracyjny pokazujący użycie wszystkich wzorców projektowych
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class PatternsController : Controller
    {
        /// <summary>
        /// Strona główna demonstracji wzorców
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Demonstracja Simple Factory Pattern
        /// </summary>
        public IActionResult SimpleFactory()
        {
            var results = new StringBuilder();
            results.AppendLine("=== SIMPLE FACTORY PATTERN ===");
            results.AppendLine();
            results.AppendLine("Wzorzec: Simple Factory służy do tworzenia różnych typów obiektów bez określania ich dokładnej klasy.");
            results.AppendLine();

            try
            {
                // Tworzenie różnych typów pojazdów
                var car = VehicleFactory.CreateVehicle("Car", "WA12345", "Toyota", "Corolla", "Srebrny");
                results.AppendLine($"1. Samochód osobowy: {car.GetDescription()}");

                var motorcycle = VehicleFactory.CreateVehicle("Motorcycle", "WB67890", "Honda", "CBR600", "Czerwony");
                results.AppendLine($"2. Motocykl: {motorcycle.GetDescription()}");

                var truck = VehicleFactory.CreateVehicle("Truck", "WC11111", "Volvo", "FH16", "Biały");
                results.AppendLine($"3. Ciężarówka: {truck.GetDescription()}");

                var bus = VehicleFactory.CreateVehicle("Bus", "WD22222", "Mercedes", "Sprinter", "Żółty");
                results.AppendLine($"4. Autobus: {bus.GetDescription()}");

                results.AppendLine();
                results.AppendLine("Dostępne typy pojazdów:");
                foreach (var type in VehicleFactory.GetAvailableVehicleTypes())
                {
                    results.AppendLine($"  - {type}");
                }
            }
            catch (Exception ex)
            {
                results.AppendLine($"Błąd: {ex.Message}");
            }

            ViewBag.Results = results.ToString();
            return View();
        }

        /// <summary>
        /// Demonstracja Factory Method Pattern
        /// </summary>
        public IActionResult FactoryMethod()
        {
            var results = new StringBuilder();
            results.AppendLine("=== FACTORY METHOD PATTERN ===");
            results.AppendLine();
            results.AppendLine("Wzorzec: Factory Method definiuje interfejs do tworzenia obiektów, ale pozwala podklasom decydować o konkretnej klasie.");
            results.AppendLine();

            try
            {
                // Różne typy kreatorów miejsc parkingowych
                ParkingSpotCreator standardCreator = new StandardSpotCreator();
                var standardSpot = standardCreator.CreateAndConfigureSpot("A1", 1);
                results.AppendLine($"1. Miejsce standardowe: {standardSpot.GetSpotDescription()}");

                ParkingSpotCreator vipCreator = new VipSpotCreator();
                var vipSpot = vipCreator.CreateAndConfigureSpot("V1", 2);
                results.AppendLine($"2. Miejsce VIP: {vipSpot.GetSpotDescription()}");

                ParkingSpotCreator disabledCreator = new DisabledSpotCreator();
                var disabledSpot = disabledCreator.CreateAndConfigureSpot("D1", 3);
                results.AppendLine($"3. Miejsce dla niepełnosprawnych: {disabledSpot.GetSpotDescription()}");

                ParkingSpotCreator electricCreator = new ElectricSpotCreator();
                var electricSpot = electricCreator.CreateAndConfigureSpot("E1", 4);
                results.AppendLine($"4. Miejsce z ładowaniem elektrycznym: {electricSpot.GetSpotDescription()}");

                results.AppendLine();
                results.AppendLine("Koszt parkowania przez 4 godziny:");
                results.AppendLine($"  - Standard: {standardSpot.CalculateCost(4)} zł");
                results.AppendLine($"  - VIP: {vipSpot.CalculateCost(4)} zł");
                results.AppendLine($"  - Dla niepełnosprawnych: {disabledSpot.CalculateCost(4)} zł");
                results.AppendLine($"  - Elektryczne: {electricSpot.CalculateCost(4)} zł");
            }
            catch (Exception ex)
            {
                results.AppendLine($"Błąd: {ex.Message}");
            }

            ViewBag.Results = results.ToString();
            return View();
        }

        /// <summary>
        /// Demonstracja Abstract Factory Pattern
        /// </summary>
        public IActionResult AbstractFactory()
        {
            var results = new StringBuilder();
            results.AppendLine("=== ABSTRACT FACTORY PATTERN ===");
            results.AppendLine();
            results.AppendLine("Wzorzec: Abstract Factory dostarcza interfejs do tworzenia rodzin powiązanych obiektów bez określania ich konkretnych klas.");
            results.AppendLine();

            try
            {
                // Publiczny parking
                IParkingZoneFactory publicFactory = new PublicParkingFactory();
                var publicZone = publicFactory.CreateZone("Strefa Publiczna A", "Parking ogólnodostępny");
                var publicSpot = publicFactory.CreateSpot("P-A1", publicZone.Id);
                var publicRule = publicFactory.CreateReservationRule();

                results.AppendLine($"1. PARKING PUBLICZNY:");
                results.AppendLine($"   Strefa: {publicZone.Name} ({publicZone.ZoneType})");
                results.AppendLine($"   Miejsce: {publicSpot.GetSpotDescription()}");
                results.AppendLine($"   Reguły: Max {publicRule.MaxReservationDays} dni, anulowanie: {publicRule.MinCancellationHours}h wcześniej");
                results.AppendLine();

                // Parking pracowniczy
                IParkingZoneFactory employeeFactory = new EmployeeParkingFactory();
                var employeeZone = employeeFactory.CreateZone("Strefa Pracownicza", "Tylko dla pracowników");
                var employeeSpot = employeeFactory.CreateSpot("E-A1", employeeZone.Id);
                var employeeRule = employeeFactory.CreateReservationRule();

                results.AppendLine($"2. PARKING PRACOWNICZY:");
                results.AppendLine($"   Strefa: {employeeZone.Name} ({employeeZone.ZoneType})");
                results.AppendLine($"   Miejsce: {employeeSpot.GetSpotDescription()}");
                results.AppendLine($"   Reguły: Max {employeeRule.MaxReservationDays} dni, anulowanie: {employeeRule.MinCancellationHours}h wcześniej");
                results.AppendLine();

                // Parking VIP
                IParkingZoneFactory vipFactory = new VipParkingFactory();
                var vipZone = vipFactory.CreateZone("Strefa VIP", "Parking premium");
                var vipSpot = vipFactory.CreateSpot("V-A1", vipZone.Id);
                var vipRule = vipFactory.CreateReservationRule();

                results.AppendLine($"3. PARKING VIP:");
                results.AppendLine($"   Strefa: {vipZone.Name} ({vipZone.ZoneType})");
                results.AppendLine($"   Miejsce: {vipSpot.GetSpotDescription()}");
                results.AppendLine($"   Reguły: Max {vipRule.MaxReservationDays} dni, anulowanie: {vipRule.MinCancellationHours}h wcześniej");
            }
            catch (Exception ex)
            {
                results.AppendLine($"Błąd: {ex.Message}");
            }

            ViewBag.Results = results.ToString();
            return View();
        }

        /// <summary>
        /// Demonstracja Singleton Pattern
        /// </summary>
        public IActionResult Singleton()
        {
            var results = new StringBuilder();
            results.AppendLine("=== SINGLETON PATTERN ===");
            results.AppendLine();
            results.AppendLine("Wzorzec: Singleton zapewnia, że klasa ma tylko jedną instancję i zapewnia globalny punkt dostępu do niej.");
            results.AppendLine();

            try
            {
                // Pobierz instancję Singleton
                var config1 = ParkingConfiguration.Instance;
                var config2 = ParkingConfiguration.GetInstance();

                results.AppendLine($"Czy obie referencje wskazują na ten sam obiekt? {ReferenceEquals(config1, config2)}");
                results.AppendLine();

                results.AppendLine("Aktualna konfiguracja parkingu:");
                results.AppendLine($"  - Maksymalna liczba dni rezerwacji: {config1.MaxReservationDays}");
                results.AppendLine($"  - Domyślna liczba godzin rezerwacji: {config1.DefaultReservationHours}");
                results.AppendLine($"  - Wiele rezerwacji jednocześnie: {config1.AllowMultipleReservations}");
                results.AppendLine($"  - Godziny otwarcia: {config1.ParkingOpenHour} - {config1.ParkingCloseHour}");
                results.AppendLine($"  - Parking 24/7: {config1.Is24Hours}");
                results.AppendLine($"  - Maksymalna liczba pojazdów na użytkownika: {config1.MaxVehiclesPerUser}");
                results.AppendLine();

                // Test walidacji
                var currentTime = DateTime.Now.TimeOfDay;
                results.AppendLine($"Czy parking jest otwarty teraz ({currentTime:hh\\:mm})? {config1.IsParkingOpen(currentTime)}");
                results.AppendLine($"Czy można zarezerwować na 15 dni? {config1.ValidateReservationDays(15)}");
                results.AppendLine($"Czy można zarezerwować na 40 dni? {config1.ValidateReservationDays(40)}");
            }
            catch (Exception ex)
            {
                results.AppendLine($"Błąd: {ex.Message}");
            }

            ViewBag.Results = results.ToString();
            return View();
        }

        /// <summary>
        /// Demonstracja Interpreter Pattern
        /// </summary>
        public IActionResult Interpreter()
        {
            var results = new StringBuilder();
            results.AppendLine("=== INTERPRETER PATTERN ===");
            results.AppendLine();
            results.AppendLine("Wzorzec: Interpreter definiuje reprezentację gramatyki języka i interpreter, który używa tej reprezentacji do interpretacji zdań w języku.");
            results.AppendLine();

            try
            {
                // Przykładowe konteksty
                var adminContext = new ParkingContext
                {
                    UserRole = "Admin",
                    VehicleType = "Car",
                    ZoneType = "VIP",
                    DateTime = DateTime.Now.Date.AddHours(10) // 10:00
                };

                var userTruckContext = new ParkingContext
                {
                    UserRole = "User",
                    VehicleType = "Truck",
                    ZoneType = "Employee",
                    DateTime = DateTime.Now.Date.AddHours(14) // 14:00
                };

                var userCarContext = new ParkingContext
                {
                    UserRole = "User",
                    VehicleType = "Car",
                    ZoneType = "Public",
                    DateTime = DateTime.Now.Date.AddHours(8) // 08:00
                };

                // Test reguł
                results.AppendLine("KONTEKST 1: Admin z samochodem w strefie VIP o 10:00");
                results.AppendLine($"  - WorkersOnly: {ParkingRuleInterpreter.ValidateAccess("WorkersOnly", adminContext)}");
                results.AppendLine($"  - VIPAccess: {ParkingRuleInterpreter.ValidateAccess("VIPAccess", adminContext)}");
                results.AppendLine($"  - NoTrucks: {ParkingRuleInterpreter.ValidateAccess("NoTrucks", adminContext)}");
                results.AppendLine($"  - EmployeeParking: {ParkingRuleInterpreter.ValidateAccess("EmployeeParking", adminContext)}");
                results.AppendLine();

                results.AppendLine("KONTEKST 2: Użytkownik z ciężarówką w strefie pracowniczej o 14:00");
                results.AppendLine($"  - WorkersOnly: {ParkingRuleInterpreter.ValidateAccess("WorkersOnly", userTruckContext)}");
                results.AppendLine($"  - VIPAccess: {ParkingRuleInterpreter.ValidateAccess("VIPAccess", userTruckContext)}");
                results.AppendLine($"  - NoTrucks: {ParkingRuleInterpreter.ValidateAccess("NoTrucks", userTruckContext)}");
                results.AppendLine($"  - EmployeeParking: {ParkingRuleInterpreter.ValidateAccess("EmployeeParking", userTruckContext)}");
                results.AppendLine();

                results.AppendLine("KONTEKST 3: Użytkownik z samochodem w strefie publicznej o 08:00");
                results.AppendLine($"  - WorkersOnly: {ParkingRuleInterpreter.ValidateAccess("WorkersOnly", userCarContext)}");
                results.AppendLine($"  - PublicParking: {ParkingRuleInterpreter.ValidateAccess("PublicParking", userCarContext)}");
                results.AppendLine($"  - NoTrucks: {ParkingRuleInterpreter.ValidateAccess("NoTrucks", userCarContext)}");

                results.AppendLine();
                results.AppendLine("Dostępne reguły:");
                results.AppendLine("  - WorkersOnly: Admin lub User");
                results.AppendLine("  - VIPAccess: tylko Admin");
                results.AppendLine("  - NoTrucks: wszystkie pojazdy oprócz Truck");
                results.AppendLine("  - PublicParking: 6:00-22:00 i nie Truck/Bus");
                results.AppendLine("  - EmployeeParking: (Admin lub User) i nie Truck");
            }
            catch (Exception ex)
            {
                results.AppendLine($"Błąd: {ex.Message}");
            }

            ViewBag.Results = results.ToString();
            return View();
        }

        /// <summary>
        /// Demonstracja Mediator Pattern
        /// </summary>
        public IActionResult Mediator()
        {
            var results = new StringBuilder();
            results.AppendLine("=== MEDIATOR PATTERN ===");
            results.AppendLine();
            results.AppendLine("Wzorzec: Mediator definiuje obiekt, który enkapsuluje sposób, w jaki zbiór obiektów współdziała ze sobą.");
            results.AppendLine();
            results.AppendLine("Mediator w tym systemie koordynuje:");
            results.AppendLine("  - Rejestrację rezerwacji");
            results.AppendLine("  - Anulowanie rezerwacji");
            results.AppendLine("  - Powiadamianie o zmianach dostępności");
            results.AppendLine("  - Sprawdzanie konfliktów");
            results.AppendLine();
            results.AppendLine("Mediator (ParkingMediator) jest zintegrowany z ReservationService i działa w tle:");
            results.AppendLine("  1. Przed utworzeniem rezerwacji sprawdza konflikty");
            results.AppendLine("  2. Waliduje limity użytkownika");
            results.AppendLine("  3. Aktualizuje status miejsca parkingowego");
            results.AppendLine("  4. Zarządza całym procesem rezerwacji");
            results.AppendLine();
            results.AppendLine("Zobacz ReservationService i ParkingMediator w kodzie źródłowym.");

            ViewBag.Results = results.ToString();
            return View();
        }

        /// <summary>
        /// Podsumowanie wszystkich wzorców
        /// </summary>
        public IActionResult Summary()
        {
            var results = new StringBuilder();
            results.AppendLine("=== PODSUMOWANIE WZORCÓW PROJEKTOWYCH ===");
            results.AppendLine();

            results.AppendLine("1. SIMPLE FACTORY (Factories/VehicleFactory.cs)");
            results.AppendLine("   - Tworzy różne typy pojazdów (Car, Motorcycle, Truck, Bus)");
            results.AppendLine("   - Klasa VehicleFactory z metodą CreateVehicle()");
            results.AppendLine();

            results.AppendLine("2. FACTORY METHOD (Factories/ParkingSpotCreator.cs)");
            results.AppendLine("   - Tworzy różne typy miejsc parkingowych");
            results.AppendLine("   - Klasa abstrakcyjna ParkingSpotCreator");
            results.AppendLine("   - Konkretne implementacje: StandardSpotCreator, VipSpotCreator, itd.");
            results.AppendLine();

            results.AppendLine("3. ABSTRACT FACTORY (Factories/IParkingZoneFactory.cs)");
            results.AppendLine("   - Tworzy kompletne zestawy obiektów dla różnych stref");
            results.AppendLine("   - PublicParkingFactory, EmployeeParkingFactory, VipParkingFactory");
            results.AppendLine("   - Każda fabryka tworzy: Zone, Spot, ReservationRule");
            results.AppendLine();

            results.AppendLine("4. SINGLETON (Configuration/ParkingConfiguration.cs)");
            results.AppendLine("   - Globalna konfiguracja systemu parkingowego");
            results.AppendLine("   - Thread-safe z Lazy<T>");
            results.AppendLine("   - Dostęp: ParkingConfiguration.Instance");
            results.AppendLine();

            results.AppendLine("5. MEDIATOR (Mediators/ParkingMediator.cs)");
            results.AppendLine("   - Koordynuje komunikację między komponentami");
            results.AppendLine("   - Zarządza rezerwacjami, konfliktami, dostępnością");
            results.AppendLine("   - Zintegrowany z ReservationService");
            results.AppendLine();

            results.AppendLine("6. INTERPRETER (Interpreters/ParkingRuleInterpreter.cs)");
            results.AppendLine("   - Interpretuje i waliduje reguły parkingowe");
            results.AppendLine("   - Terminal expressions: UserRole, VehicleType, TimeRange, ZoneType");
            results.AppendLine("   - Logical operators: AND, OR, NOT");
            results.AppendLine();

            results.AppendLine("INTEGRACJA Z SYSTEMEM:");
            results.AppendLine("  - VehicleService używa Simple Factory");
            results.AppendLine("  - ReservationService używa Mediator");
            results.AppendLine("  - Wszystkie serwisy zarejestrowane w Program.cs z DI");
            results.AppendLine("  - Singleton dostępny globalnie");

            ViewBag.Results = results.ToString();
            return View();
        }
    }
}
