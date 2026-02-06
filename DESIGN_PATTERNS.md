# Dokumentacja Wzorców Projektowych - System Rejestracji Parkingowej

## Przegląd

System rejestracji parkingowej w firmie implementuje **6 kluczowych wzorców projektowych** zgodnie z wymaganiami zaliczenia. Każdy wzorzec został starannie zaimplementowany i zintegrowany z istniejącym systemem ASP.NET Core.

## 💡 Parking firmowy - DARMOWY

**UWAGA:** System zarządza parkingiem firmowym, który jest **bezpłatny** dla pracowników.

Miejsca różnicowane są po:
- **Typie**: Standard, VIP, Electric, Disabled
- **Rozmiarze**: Small, Normal, Large
- **Wyposażeniu**: Ładowarka elektryczna (tak/nie)
- **Przeznaczeniu**: Dla pracowników, kadry zarządzającej, osób niepełnosprawnych
- **Priorytecie**: 1 (najwyższy) - 5 (najniższy)

Wzorce **Factory Method** i **Abstract Factory** różnicują miejsca po tych charakterystykach, a **nie po cenie**.

## Wzorce Projektowe

### 1. Simple Factory Pattern 🏭

**Lokalizacja:** `Factories/VehicleFactory.cs`

**Cel:** Tworzenie różnych typów pojazdów bez określania ich dokładnej klasy.

**Implementacja:**
- **Interfejs:** `IVehicle` - definiuje kontrakt dla wszystkich pojazdów
- **Klasa abstrakcyjna:** `VehicleBase` - wspólna funkcjonalność dla wszystkich pojazdów
- **Produkty konkretne:**
  - `Car` - samochód osobowy (właściwości: NumberOfDoors, BodyType)
  - `Motorcycle` - motocykl (właściwości: EngineCapacity, MotorcycleType)
  - `Truck` - ciężarówka (właściwości: LoadCapacity, NumberOfAxles, HasTrailer)
  - `Bus` - autobus (właściwości: SeatingCapacity, BusType, HasDisabledAccess)
- **Fabryka:** `VehicleFactory` - statyczna klasa z metodą `CreateVehicle()`

**Użycie:**
```csharp
var car = VehicleFactory.CreateVehicle("Car", "WA12345", "Toyota", "Corolla", "Srebrny");
var motorcycle = VehicleFactory.CreateVehicle("Motorcycle", "WB67890", "Honda", "CBR600", "Czerwony");
```

### 2. Factory Method Pattern 🏗️

**Lokalizacja:** `Factories/ParkingSpotCreator.cs`

**Cel:** Definiowanie interfejsu do tworzenia obiektów, ale pozwolenie podklasom na decydowanie o konkretnej klasie.

**Implementacja:**
- **Interfejs produktu:** `IParkingSpot`
- **Klasa bazowa produktu:** `ParkingSpotBase`
- **Creator abstrakcyjny:** `ParkingSpotCreator` z metodą `CreateParkingSpot()`
- **Creators konkretne:**
  - `StandardSpotCreator` - tworzy standardowe miejsca (rozmiar normalny, dla pracowników)
  - `VipSpotCreator` - tworzy miejsca VIP (rozmiar duży, z ładowaniem, dla kadry zarządzającej)
  - `DisabledSpotCreator` - tworzy miejsca dla niepełnosprawnych (rozmiar duży, priorytetowe)
  - `ElectricSpotCreator` - tworzy miejsca z ładowaniem elektrycznym (rozmiar normalny, ładowarka 22kW)

**Użycie:**
```csharp
ParkingSpotCreator creator = new VipSpotCreator();
var vipSpot = creator.CreateAndConfigureSpot("V1", zoneId);
```

### 3. Abstract Factory Pattern 🏭🏭🏭

**Lokalizacja:** `Factories/IParkingZoneFactory.cs`

**Cel:** Tworzenie rodzin powiązanych obiektów bez określania ich konkretnych klas.

**Implementacja:**
- **Abstract Factory:** `IParkingZoneFactory`
- **Metody fabryki:**
  - `CreateZone()` - tworzy strefę parkingową
  - `CreateSpot()` - tworzy miejsce parkingowe
  - `CreateReservationRule()` - tworzy reguły rezerwacji
- **Factories konkretne:**
  - `PublicParkingFactory` - parking publiczny (7 dni max rezerwacji, 2h na anulowanie)
  - `EmployeeParkingFactory` - parking pracowniczy (30 dni max rezerwacji, 24h na anulowanie)
  - `VipParkingFactory` - parking VIP (90 dni max rezerwacji, 1h na anulowanie)

**Użycie:**
```csharp
IParkingZoneFactory factory = new VipParkingFactory();
var zone = factory.CreateZone("Strefa VIP", "Parking premium");
var spot = factory.CreateSpot("V-A1", zone.Id);
var rules = factory.CreateReservationRule();
```

### 4. Singleton Pattern 🔒

**Lokalizacja:** `Configuration/ParkingConfiguration.cs`

**Cel:** Zapewnienie, że klasa ma tylko jedną instancję z globalnym punktem dostępu.

**Implementacja:**
- Thread-safe z wykorzystaniem `Lazy<T>`
- Prywatny konstruktor
- Publiczna właściwość `Instance`
- Metoda `GetInstance()` jako alternatywa

**Konfiguracja:**
- `MaxReservationDays` - maksymalna liczba dni rezerwacji (30)
- `DefaultReservationHours` - domyślna długość rezerwacji (2h)
- `AllowMultipleReservations` - czy dozwolone wiele rezerwacji (false)
- `ParkingOpenHour` / `ParkingCloseHour` - godziny otwarcia (6:00-22:00)
- `Is24Hours` - czy parking 24/7 (false)
- `MaxVehiclesPerUser` - maksymalna liczba pojazdów na użytkownika (5)

**Użycie:**
```csharp
var config = ParkingConfiguration.Instance;
bool isOpen = config.IsParkingOpen(DateTime.Now.TimeOfDay);
bool validDays = config.ValidateReservationDays(15);
```

### 5. Mediator Pattern 🤝

**Lokalizacja:** `Mediators/ParkingMediator.cs`

**Cel:** Enkapsulacja sposobu, w jaki zbiór obiektów ze sobą współdziała.

**Implementacja:**
- **Interfejs:** `IParkingMediator`
- **Mediator konkretny:** `ParkingMediator`
- **Metody:**
  - `RegisterReservation()` - koordynuje proces tworzenia rezerwacji
  - `CancelReservation()` - zarządza anulowaniem rezerwacji
  - `NotifyAvailability()` - powiadamia o zmianie dostępności miejsca
  - `CheckConflicts()` - sprawdza konflikty rezerwacji
  - `CheckSpotAvailability()` - weryfikuje dostępność miejsca
  - `GetUserActiveReservationsCount()` - zwraca liczbę aktywnych rezerwacji

**Koordynacja:**
1. Sprawdzanie konfliktów przed rezerwacją
2. Walidacja limitów użytkownika
3. Aktualizacja statusu miejsc
4. Zarządzanie całym procesem rezerwacji

**Użycie:**
```csharp
var mediator = new ParkingMediator(context);
bool success = await mediator.RegisterReservation(reservation);
```

### 6. Interpreter Pattern 📖

**Lokalizacja:** `Interpreters/ParkingRuleInterpreter.cs`

**Cel:** Definiowanie reprezentacji gramatyki dla reguł parkingowych i interpretera do ich wykonania.

**Implementacja:**
- **Interfejs:** `IParkingRuleExpression`
- **Kontekst:** `ParkingContext` - zawiera User, Vehicle, Zone, DateTime
- **Terminal Expressions:**
  - `UserRoleExpression` - sprawdza rolę użytkownika
  - `VehicleTypeExpression` - sprawdza typ pojazdu
  - `TimeRangeExpression` - sprawdza zakres czasowy
  - `ZoneTypeExpression` - sprawdza typ strefy
- **Non-Terminal Expressions (operatory logiczne):**
  - `AndExpression` - operator AND
  - `OrExpression` - operator OR
  - `NotExpression` - operator NOT
- **Interpreter:** `ParkingRuleInterpreter` - buduje i wykonuje drzewo wyrażeń

**Predefiniowane reguły:**
- `WorkersOnly` - tylko pracownicy (Admin lub User)
- `VIPAccess` - tylko Admin
- `NoTrucks` - zakaz ciężarówek
- `PublicParking` - 6:00-22:00 i nie Truck/Bus
- `EmployeeParking` - (Admin lub User) i nie Truck
- `VIPParking` - Admin lub strefa VIP

**Użycie:**
```csharp
var context = new ParkingContext(user, "Admin", vehicle, zone);
bool allowed = ParkingRuleInterpreter.ValidateAccess("VIPAccess", context);

// Lub tworzenie własnych reguł:
var rule = new AndExpression(
    new UserRoleExpression("Admin"),
    new NotExpression(new VehicleTypeExpression("Truck"))
);
bool result = rule.Interpret(context);
```

## Warstwa Serwisów

### IReservationService / ReservationService
- Wykorzystuje **Mediator Pattern** do koordynacji rezerwacji
- Używa **Singleton Configuration** do walidacji
- Metody: CreateReservation, CancelReservation, GetUserReservations, CanUserReserve

### IVehicleService / VehicleService
- Wykorzystuje **Simple Factory** do tworzenia pojazdów
- Używa **Singleton Configuration** do limitów
- Metody: CreateVehicle, GetUserVehicles, CanAddVehicle, ConvertToVehicleBase

## Dependency Injection

Wszystkie serwisy zarejestrowane w `Program.cs`:
```csharp
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IParkingMediator, ParkingMediator>();
```

## Demonstracja

System zawiera dedykowany kontroler demonstracyjny dostępny dla administratorów:

**URL:** `/Patterns`

**Funkcje:**
- Demonstracja każdego wzorca z przykładami
- Interaktywne pokazanie działania wzorców
- Podsumowanie wszystkich implementacji

## Struktura Folderów

```
/Factories
  - VehicleFactory.cs (Simple Factory)
  - ParkingSpotCreator.cs (Factory Method - abstract)
  - StandardSpotCreator.cs, VipSpotCreator.cs, ... (Factory Method - concrete)
  - IParkingZoneFactory.cs (Abstract Factory - interface)
  - PublicParkingFactory.cs, EmployeeParkingFactory.cs, ... (Abstract Factory - concrete)
  - IReservationRule.cs

/Models/Vehicles
  - IVehicle.cs
  - VehicleBase.cs (abstract)
  - Car.cs, Motorcycle.cs, Truck.cs, Bus.cs

/Models/ParkingSpots
  - IParkingSpot.cs
  - ParkingSpotBase.cs

/Configuration
  - ParkingConfiguration.cs (Singleton)

/Mediators
  - IParkingMediator.cs
  - ParkingMediator.cs

/Interpreters
  - IParkingRuleExpression.cs
  - ParkingContext.cs
  - UserRoleExpression.cs, VehicleTypeExpression.cs, ...
  - AndExpression.cs, OrExpression.cs, NotExpression.cs
  - ParkingRuleInterpreter.cs

/Services
  - IReservationService.cs, ReservationService.cs
  - IVehicleService.cs, VehicleService.cs

/Controllers
  - PatternsController.cs (demonstracja)
```

## Dokumentacja XML

Każda klasa, interfejs i metoda zawiera pełną dokumentację XML z:
- Opisem celu i funkcjonalności
- Parametrami i typami zwracanymi
- Przykładami użycia w komentarzach
- Oznaczeniem użytego wzorca projektowego

## Zasady SOLID

Implementacja przestrzega zasad SOLID:
- **S**ingle Responsibility - każda klasa ma jedną odpowiedzialność
- **O**pen/Closed - otwarte na rozszerzenia, zamknięte na modyfikacje
- **L**iskov Substitution - podklasy mogą zastąpić klasy bazowe
- **I**nterface Segregation - interfejsy są małe i specyficzne
- **D**ependency Inversion - zależności od abstrakcji, nie konkretnych klas

## Testy

Aby przetestować wzorce:
1. Zaloguj się jako Admin (admin@test.com / Admin123)
2. Przejdź do menu "Wzorce Projektowe"
3. Wybierz wzorzec do demonstracji
4. Zobacz przykłady działania w praktyce

## Podsumowanie

System implementuje wszystkie 6 wymaganych wzorców projektowych:
✅ Simple Factory - tworzenie pojazdów
✅ Factory Method - tworzenie miejsc parkingowych
✅ Abstract Factory - tworzenie kompletnych stref parkingowych
✅ Singleton - globalna konfiguracja (thread-safe)
✅ Mediator - koordynacja rezerwacji
✅ Interpreter - walidacja reguł parkingowych

Wszystkie wzorce są:
- Zgodne z klasycznymi definicjami
- Zintegrowane z systemem
- Udokumentowane XML
- Praktycznie użyteczne
- Zgodne z zasadami SOLID
