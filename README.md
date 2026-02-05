# 🅿️ System Rejestracji Parkingowej w Firmie

![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat&logo=dotnet)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-512BD4?style=flat&logo=dotnet)
![Entity Framework](https://img.shields.io/badge/Entity%20Framework-8.0-512BD4?style=flat)
![C#](https://img.shields.io/badge/C%23-12.0-239120?style=flat&logo=c-sharp)
![SQLite](https://img.shields.io/badge/SQLite-3-003B57?style=flat&logo=sqlite)

> System zarządzania parkingiem firmowym z implementacją 6 wzorców projektowych w ASP.NET Core MVC

---

## 📑 Spis Treści

- [O Projekcie](#-o-projekcie)
- [Funkcjonalności](#-funkcjonalności)
- [Wzorce Projektowe](#-wzorce-projektowe)
- [Technologie](#-technologie)
- [Wymagania Systemowe](#-wymagania-systemowe)
- [Instalacja i Uruchomienie](#-instalacja-i-uruchomienie)
- [Struktura Projektu](#-struktura-projektu)
- [Demo Wzorców Projektowych](#-demo-wzorców-projektowych)
- [Konto Testowe](#-konto-testowe)
- [Baza Danych](#-baza-danych)
- [Dependency Injection](#-dependency-injection)
- [Dokumentacja Dodatkowa](#-dokumentacja-dodatkowa)
- [Autor](#-autor)

---

## 📖 O Projekcie

**System Rejestracji Parkingowej w Firmie** to kompleksowa aplikacja webowa stworzona w technologii ASP.NET Core MVC, która umożliwia zarządzanie parkingiem firmowym. Projekt został stworzony jako praca zaliczeniowa z przedmiotu **Programowanie Obiektowe - Wzorce Projektowe** i implementuje **6 klasycznych wzorców projektowych** zgodnie z książką "Gang of Four".

System oferuje pełne zarządzanie pojazdami, miejscami parkingowymi, strefami oraz rezerwacjami, z wykorzystaniem nowoczesnych technologii .NET 8 i najlepszych praktyk programowania obiektowego (SOLID).

---

## 🚀 Funkcjonalności

- 🔐 **System logowania i rejestracji** - ASP.NET Identity z rolami (Admin, User)
- 🚗 **Zarządzanie pojazdami** - CRUD dla różnych typów pojazdów (samochody, motocykle, ciężarówki, autobusy)
- 🅿️ **Zarządzanie miejscami parkingowymi** - różne typy miejsc (Standard, VIP, dla niepełnosprawnych, z ładowaniem elektrycznym)
- 🏢 **Zarządzanie strefami parkingowymi** - strefy publiczne, pracownicze i VIP
- 📅 **System rezerwacji miejsc** - rezerwacje z walidacją konfliktów i limitów czasowych
- 👥 **Role użytkowników** - Admin (pełny dostęp) i User (zarządzanie własnymi zasobami)
- ✅ **Walidacja danych** - po stronie serwera z wykorzystaniem Data Annotations
- 🎨 **Demonstracja wzorców projektowych** - dedykowany moduł pokazujący działanie wzorców

---

## 🏗️ Wzorce Projektowe

System implementuje **6 wzorców projektowych** zgodnie z wymaganiami projektu zaliczeniowego:

### 1. 🏭 Simple Factory Pattern

**Zastosowanie:** Tworzenie różnych typów pojazdów bez określania konkretnej klasy.

**Lokalizacja:** `Factories/VehicleFactory.cs`

**Opis:** Factory tworzy pojazdy (Car, Motorcycle, Truck, Bus) na podstawie przekazanego typu, ukrywając logikę tworzenia i inicjalizacji obiektów.

**Przykład użycia:**
```csharp
// Tworzenie samochodu
var car = VehicleFactory.CreateVehicle(
    "Car", 
    "WA12345", 
    "Toyota", 
    "Corolla", 
    "Srebrny"
);

// Tworzenie motocykla
var motorcycle = VehicleFactory.CreateVehicle(
    "Motorcycle", 
    "WB67890", 
    "Honda", 
    "CBR600", 
    "Czerwony"
);
```

**Hierarchia klas:**
- `IVehicle` - interfejs
- `VehicleBase` - klasa abstrakcyjna
- `Car`, `Motorcycle`, `Truck`, `Bus` - konkretne implementacje

---

### 2. 🔨 Factory Method Pattern

**Zastosowanie:** Tworzenie różnych typów miejsc parkingowych z różnymi właściwościami.

**Lokalizacja:** `Factories/ParkingSpotCreator.cs`

**Opis:** Wzorzec definiuje interfejs do tworzenia obiektów, ale pozwala podklasom decydować, jaką klasę utworzyć. Każdy typ miejsca parkingowego ma swojego dedykowanego creatora.

**Przykład użycia:**
```csharp
// Tworzenie miejsca VIP
ParkingSpotCreator vipCreator = new VipSpotCreator();
var vipSpot = vipCreator.CreateAndConfigureSpot("V1", zoneId);
// Cena: 15 zł/h, duże miejsce, z ładowaniem

// Tworzenie miejsca standardowego
ParkingSpotCreator standardCreator = new StandardSpotCreator();
var standardSpot = standardCreator.CreateAndConfigureSpot("A1", zoneId);
// Cena: 5 zł/h, standardowe miejsce
```

**Creators konkretne:**
- `StandardSpotCreator` - miejsce standardowe (5 zł/h)
- `VipSpotCreator` - miejsce VIP (15 zł/h, duże, z ładowaniem)
- `DisabledSpotCreator` - miejsce dla niepełnosprawnych (darmowe, duże)
- `ElectricSpotCreator` - miejsce z ładowaniem elektrycznym (8 zł/h)

---

### 3. 🏗️ Abstract Factory Pattern

**Zastosowanie:** Tworzenie kompletnych rodzin powiązanych obiektów (strefy, miejsca, reguły) bez określania konkretnych klas.

**Lokalizacja:** `Factories/IParkingZoneFactory.cs`

**Opis:** Wzorzec tworzy rodziny powiązanych obiektów. Każda fabryka (Public, Employee, VIP) tworzy kompletny zestaw obiektów dostosowanych do danego typu parkingu.

**Przykład użycia:**
```csharp
// Fabryka parkingu VIP
IParkingZoneFactory vipFactory = new VipParkingFactory();
var zone = vipFactory.CreateZone("Strefa VIP", "Parking premium");
var spot = vipFactory.CreateSpot("V-A1", zone.Id);
var rules = vipFactory.CreateReservationRule();

// Reguły VIP: 90 dni max rezerwacji, 1h na anulowanie, wyższe ceny
```

**Fabryki konkretne:**
- `PublicParkingFactory` - parking publiczny (7 dni max, 2h anulowanie)
- `EmployeeParkingFactory` - parking pracowniczy (30 dni max, 24h anulowanie)
- `VipParkingFactory` - parking VIP (90 dni max, 1h anulowanie)

---

### 4. 🔒 Singleton Pattern

**Zastosowanie:** Globalna konfiguracja systemu parkingowego dostępna z każdego miejsca w aplikacji.

**Lokalizacja:** `Configuration/ParkingConfiguration.cs`

**Opis:** Thread-safe Singleton z wykorzystaniem `Lazy<T>`. Zapewnia jedną instancję konfiguracji w całej aplikacji.

**Przykład użycia:**
```csharp
// Pobranie instancji konfiguracji
var config = ParkingConfiguration.Instance;

// Sprawdzenie czy parking jest otwarty
bool isOpen = config.IsParkingOpen(DateTime.Now.TimeOfDay);

// Walidacja liczby dni rezerwacji
bool validDays = config.ValidateReservationDays(15); // max 30 dni

// Dostęp do konfiguracji
int maxVehicles = config.MaxVehiclesPerUser; // 5
int maxDays = config.MaxReservationDays; // 30
```

**Konfiguracja:**
- `MaxReservationDays = 30`
- `DefaultReservationHours = 2`
- `AllowMultipleReservations = false`
- `ParkingOpenHour = 06:00`
- `ParkingCloseHour = 22:00`
- `MaxVehiclesPerUser = 5`

---

### 5. 🤝 Mediator Pattern

**Zastosowanie:** Koordynacja procesu rezerwacji miejsc parkingowych między różnymi komponentami systemu.

**Lokalizacja:** `Mediators/ParkingMediator.cs`

**Opis:** Mediator enkapsuluje sposób komunikacji między obiektami, zmniejszając ich wzajemne powiązanie. Zarządza procesem rezerwacji, sprawdza konflikty i aktualizuje statusy.

**Przykład użycia:**
```csharp
// Rejestracja rezerwacji przez Mediator
var mediator = new ParkingMediator(dbContext);
bool success = await mediator.RegisterReservation(reservation);

// Mediator automatycznie:
// 1. Sprawdza konflikty czasowe
// 2. Weryfikuje limity użytkownika
// 3. Aktualizuje status miejsca parkingowego
// 4. Koordynuje cały proces
```

**Metody Mediatora:**
- `RegisterReservation()` - koordynuje tworzenie rezerwacji
- `CancelReservation()` - zarządza anulowaniem
- `CheckConflicts()` - sprawdza konflikty czasowe
- `NotifyAvailability()` - aktualizuje dostępność miejsc
- `GetUserActiveReservationsCount()` - zwraca liczbę aktywnych rezerwacji

---

### 6. 📖 Interpreter Pattern

**Zastosowanie:** Interpretacja i walidacja reguł parkingowych (kto może parkować, gdzie i kiedy).

**Lokalizacja:** `Interpreters/ParkingRuleInterpreter.cs`

**Opis:** Wzorzec definiuje gramatykę dla reguł parkingowych i interpreter do ich wykonania. Pozwala na budowanie złożonych reguł z prostych wyrażeń.

**Przykład użycia:**
```csharp
// Sprawdzenie dostępu VIP
var context = new ParkingContext(user, "Admin", vehicle, zone);
bool hasAccess = ParkingRuleInterpreter.ValidateAccess("VIPAccess", context);

// Budowanie własnych reguł
var rule = new AndExpression(
    new UserRoleExpression("Admin"),
    new NotExpression(new VehicleTypeExpression("Truck"))
);
bool result = rule.Interpret(context);
// Reguła: Admin AND NOT Truck
```

**Wyrażenia terminalne:**
- `UserRoleExpression` - sprawdza rolę użytkownika
- `VehicleTypeExpression` - sprawdza typ pojazdu
- `TimeRangeExpression` - sprawdza zakres czasowy
- `ZoneTypeExpression` - sprawdza typ strefy

**Operatory logiczne:**
- `AndExpression` - operator AND
- `OrExpression` - operator OR
- `NotExpression` - operator NOT

**Predefiniowane reguły:**
- `WorkersOnly` - tylko pracownicy
- `VIPAccess` - tylko Admin
- `NoTrucks` - zakaz ciężarówek
- `PublicParking` - parking publiczny (6:00-22:00)
- `EmployeeParking` - parking pracowniczy
- `VIPParking` - parking VIP

---

## 💻 Technologie

| Technologia | Wersja | Zastosowanie |
|------------|--------|--------------|
| **.NET** | 8.0 | Framework aplikacji |
| **ASP.NET Core MVC** | 8.0 | Warstwa prezentacji |
| **Entity Framework Core** | 8.0.0 | ORM, dostęp do bazy danych |
| **SQLite** | 3.x | Baza danych |
| **ASP.NET Identity** | 8.0.0 | Autentykacja i autoryzacja |
| **Bootstrap** | 5.x | Framework CSS |
| **C#** | 12.0 | Język programowania |

---

## 🔧 Wymagania Systemowe

- **.NET 8 SDK** - [Pobierz tutaj](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Visual Studio 2022** lub **VS Code** z rozszerzeniem C#
- **Przeglądarka** - Chrome, Firefox, Edge lub Safari
- **System operacyjny** - Windows 10+, macOS, Linux

---

## 📥 Instalacja i Uruchomienie

### Krok 1: Klonowanie repozytorium
```bash
git clone https://github.com/Chx96171/System-rejestracji-parkingowej-w-firmie.git
cd System-rejestracji-parkingowej-w-firmie
```

### Krok 2: Przywracanie pakietów
```bash
dotnet restore
```

### Krok 3: Uruchomienie migracji (opcjonalnie)
```bash
# Baza danych SQLite jest już wygenerowana, ale możesz ją odświeżyć:
dotnet ef database update
```

### Krok 4: Uruchomienie aplikacji
```bash
dotnet run
```

### Krok 5: Otwórz w przeglądarce
```
https://localhost:5001
```

lub

```
http://localhost:5000
```

### Zatrzymanie aplikacji
```
Ctrl + C
```

---

## 📁 Struktura Projektu

```
📁 System-rejestracji-parkingowej-w-firmie/
├── 📁 Configuration/              # Singleton - globalna konfiguracja
│   └── ParkingConfiguration.cs   # Thread-safe Singleton
│
├── 📁 Controllers/                # Kontrolery MVC
│   ├── AccountController.cs      # Logowanie/Rejestracja
│   ├── VehiclesController.cs     # CRUD pojazdów
│   ├── ParkingSpotsController.cs # CRUD miejsc parkingowych
│   ├── ParkingZonesController.cs # CRUD stref parkingowych
│   ├── ReservationsController.cs # CRUD rezerwacji
│   ├── PatternsController.cs     # Demo wzorców projektowych
│   └── HomeController.cs         # Strona główna
│
├── 📁 Data/                       # Kontekst bazy danych
│   └── ApplicationDbContext.cs   # DbContext EF Core
│
├── 📁 Factories/                  # Wzorce Factory (3 wzorce)
│   ├── VehicleFactory.cs         # Simple Factory - pojazdy
│   ├── ParkingSpotCreator.cs     # Factory Method - abstract creator
│   ├── StandardSpotCreator.cs    # Factory Method - concrete
│   ├── VipSpotCreator.cs         # Factory Method - concrete
│   ├── DisabledSpotCreator.cs    # Factory Method - concrete
│   ├── ElectricSpotCreator.cs    # Factory Method - concrete
│   ├── IParkingZoneFactory.cs    # Abstract Factory - interface
│   ├── PublicParkingFactory.cs   # Abstract Factory - concrete
│   ├── EmployeeParkingFactory.cs # Abstract Factory - concrete
│   ├── VipParkingFactory.cs      # Abstract Factory - concrete
│   └── IReservationRule.cs       # Interfejs reguł rezerwacji
│
├── 📁 Interpreters/               # Wzorzec Interpreter
│   ├── IParkingRuleExpression.cs # Interfejs wyrażeń
│   ├── ParkingContext.cs         # Kontekst interpretacji
│   ├── UserRoleExpression.cs     # Terminal expression
│   ├── VehicleTypeExpression.cs  # Terminal expression
│   ├── TimeRangeExpression.cs    # Terminal expression
│   ├── ZoneTypeExpression.cs     # Terminal expression
│   ├── AndExpression.cs          # Non-terminal (AND)
│   ├── OrExpression.cs           # Non-terminal (OR)
│   ├── NotExpression.cs          # Non-terminal (NOT)
│   └── ParkingRuleInterpreter.cs # Główny interpreter
│
├── 📁 Mediators/                  # Wzorzec Mediator
│   ├── IParkingMediator.cs       # Interfejs mediatora
│   └── ParkingMediator.cs        # Implementacja mediatora
│
├── 📁 Migrations/                 # Migracje EF Core
│   └── ...                        # Pliki migracji
│
├── 📁 Models/                     # Modele danych
│   ├── 📁 Vehicles/              # Hierarchia pojazdów (Simple Factory)
│   │   ├── IVehicle.cs           # Interfejs pojazdu
│   │   ├── VehicleBase.cs        # Klasa abstrakcyjna
│   │   ├── Car.cs                # Samochód
│   │   ├── Motorcycle.cs         # Motocykl
│   │   ├── Truck.cs              # Ciężarówka
│   │   └── Bus.cs                # Autobus
│   │
│   ├── 📁 ParkingSpots/          # Miejsca parkingowe (Factory Method)
│   │   ├── IParkingSpot.cs       # Interfejs miejsca
│   │   └── ParkingSpotBase.cs    # Klasa bazowa miejsca
│   │
│   ├── ApplicationUser.cs        # Model użytkownika (Identity)
│   ├── ParkingZone.cs            # Model strefy parkingowej
│   └── Reservation.cs            # Model rezerwacji
│
├── 📁 Services/                   # Warstwa biznesowa
│   ├── IReservationService.cs    # Interfejs serwisu rezerwacji
│   ├── ReservationService.cs     # Implementacja (używa Mediator)
│   ├── IVehicleService.cs        # Interfejs serwisu pojazdów
│   └── VehicleService.cs         # Implementacja (używa Simple Factory)
│
├── 📁 Views/                      # Widoki Razor
│   ├── 📁 Account/               # Logowanie/Rejestracja
│   ├── 📁 Patterns/              # Demo wzorców projektowych
│   │   ├── Index.cshtml          # Lista wszystkich wzorców
│   │   ├── SimpleFactory.cshtml  # Demo Simple Factory
│   │   ├── FactoryMethod.cshtml  # Demo Factory Method
│   │   ├── AbstractFactory.cshtml# Demo Abstract Factory
│   │   ├── Singleton.cshtml      # Demo Singleton
│   │   ├── Mediator.cshtml       # Demo Mediator
│   │   └── Interpreter.cshtml    # Demo Interpreter
│   ├── 📁 Vehicles/              # CRUD pojazdów
│   ├── 📁 ParkingSpots/          # CRUD miejsc parkingowych
│   ├── 📁 ParkingZones/          # CRUD stref parkingowych
│   ├── 📁 Reservations/          # CRUD rezerwacji
│   └── 📁 Shared/                # Layouty i komponenty współdzielone
│
├── 📁 wwwroot/                    # Pliki statyczne
│   ├── 📁 css/                   # Style CSS
│   ├── 📁 js/                    # Skrypty JavaScript
│   └── 📁 lib/                   # Biblioteki (Bootstrap, jQuery)
│
├── Program.cs                     # Punkt wejścia aplikacji
├── appsettings.json              # Konfiguracja aplikacji
├── parking.db                     # Baza danych SQLite
├── DESIGN_PATTERNS.md            # Dokumentacja wzorców
├── IMPLEMENTATION_SUMMARY.md     # Podsumowanie implementacji
└── README.md                      # Ten plik
```

---

## 🎨 Demo Wzorców Projektowych

System zawiera **dedykowany moduł demonstracyjny** dostępny dla administratorów w menu nawigacyjnym.

### Dostęp do demo:
1. Zaloguj się jako **Admin** (dane w sekcji [Konto Testowe](#-konto-testowe))
2. Kliknij **"Wzorce Projektowe"** w górnym menu
3. Wybierz wzorzec do demonstracji

### Dostępne demonstracje:

| URL | Wzorzec | Opis |
|-----|---------|------|
| `/Patterns` | Lista wszystkich | Przegląd wszystkich 6 wzorców |
| `/Patterns/SimpleFactory` | Simple Factory | Tworzenie różnych typów pojazdów |
| `/Patterns/FactoryMethod` | Factory Method | Tworzenie miejsc parkingowych |
| `/Patterns/AbstractFactory` | Abstract Factory | Tworzenie kompletnych stref |
| `/Patterns/Singleton` | Singleton | Globalna konfiguracja |
| `/Patterns/Mediator` | Mediator | Koordynacja rezerwacji |
| `/Patterns/Interpreter` | Interpreter | Walidacja reguł parkingowych |

Każda demonstracja zawiera:
- 📝 Opis wzorca
- 💡 Przykłady użycia
- 🖼️ Interaktywne pokazanie działania
- 📊 Wizualizację struktury klas

---

## 🔑 Konto Testowe

System automatycznie tworzy konto administratora przy pierwszym uruchomieniu.

### Dane logowania:

```
👤 Admin:
Email:    admin@test.com
Hasło:    Admin123
Rola:     Administrator
```

### Uprawnienia Admin:
- ✅ Pełny dostęp do wszystkich funkcji
- ✅ Zarządzanie wszystkimi pojazdami
- ✅ Zarządzanie miejscami i strefami parkingowymi
- ✅ Przeglądanie wszystkich rezerwacji
- ✅ Dostęp do demonstracji wzorców projektowych

### Tworzenie użytkownika zwykłego:
1. Kliknij **"Rejestracja"** na stronie logowania
2. Wypełnij formularz
3. Zaloguj się z nowymi danymi
4. Rola **User** przypisana automatycznie

---

## 🗄️ Baza Danych

System używa **SQLite** - lekkiej, bezserwerowej bazy danych przechowywanej w pliku `parking.db`.

### Tabele (modele):

| Tabela | Model | Opis |
|--------|-------|------|
| `AspNetUsers` | `ApplicationUser` | Użytkownicy systemu (Identity) |
| `AspNetRoles` | `IdentityRole` | Role (Admin, User) |
| `Vehicles` | `VehicleBase` | Pojazdy użytkowników |
| `ParkingSpots` | `ParkingSpotBase` | Miejsca parkingowe |
| `ParkingZones` | `ParkingZone` | Strefy parkingowe |
| `Reservations` | `Reservation` | Rezerwacje miejsc |

### Relacje:
- **ApplicationUser → Vehicles** (1:N) - użytkownik może mieć wiele pojazdów
- **ApplicationUser → Reservations** (1:N) - użytkownik może mieć wiele rezerwacji
- **ParkingZone → ParkingSpots** (1:N) - strefa zawiera wiele miejsc
- **ParkingSpot → Reservations** (1:N) - miejsce może mieć wiele rezerwacji (w różnych czasach)

### Connection String:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=parking.db"
  }
}
```

---

## 💉 Dependency Injection

Wszystkie serwisy i wzorce są zarejestrowane w kontenerze DI w pliku `Program.cs`:

```csharp
// Kontekst bazy danych
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// ASP.NET Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Serwisy z wzorcami projektowymi
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IParkingMediator, ParkingMediator>();
```

### Cykl życia serwisów:
- **Scoped** - nowa instancja na każde żądanie HTTP
- Używane dla serwisów korzystających z `DbContext`

---

## 📚 Dokumentacja Dodatkowa

Projekt zawiera dodatkową dokumentację techniczną:

| Plik | Opis |
|------|------|
| 📄 [DESIGN_PATTERNS.md](DESIGN_PATTERNS.md) | Szczegółowa dokumentacja wszystkich 6 wzorców projektowych z przykładami kodu |
| 📄 [IMPLEMENTATION_SUMMARY.md](IMPLEMENTATION_SUMMARY.md) | Podsumowanie implementacji, statystyki i zgodność z wymaganiami |

### Dokumentacja XML
Każda klasa i metoda w projekcie zawiera pełną dokumentację XML:
- Opis celu i funkcjonalności
- Parametry i typy zwracane
- Przykłady użycia
- Oznaczenie użytego wzorca projektowego

---

## 👨‍💻 Autor

**Autor:** Chx96171  
**Uczelnia:** Merito Chorzów  
**Przedmiot:** Programowanie Obiektowe - Wzorce Projektowe  
**Rok akademicki:** 2026  
**Typ projektu:** Projekt zaliczeniowy

### Wymagania projektu:
✅ Implementacja 6 wzorców projektowych (Simple Factory, Factory Method, Abstract Factory, Singleton, Mediator, Interpreter)  
✅ Użycie klas abstrakcyjnych i interfejsów  
✅ Dokumentacja XML wszystkich klas i metod  
✅ Integracja wzorców z działającą aplikacją  
✅ Zgodność z zasadami SOLID  
✅ Kod w języku C# (.NET 8)

---

## 📜 Licencja

Ten projekt został stworzony na potrzeby edukacyjne jako projekt zaliczeniowy.

---

## 🎯 Podsumowanie Wzorców

| Wzorzec | Kategoria | Zastosowanie | Lokalizacja |
|---------|-----------|--------------|-------------|
| 🏭 Simple Factory | Kreacyjny | Tworzenie pojazdów | `Factories/VehicleFactory.cs` |
| 🔨 Factory Method | Kreacyjny | Tworzenie miejsc parkingowych | `Factories/ParkingSpotCreator.cs` |
| 🏗️ Abstract Factory | Kreacyjny | Tworzenie stref parkingowych | `Factories/IParkingZoneFactory.cs` |
| 🔒 Singleton | Kreacyjny | Globalna konfiguracja | `Configuration/ParkingConfiguration.cs` |
| 🤝 Mediator | Behawioralny | Koordynacja rezerwacji | `Mediators/ParkingMediator.cs` |
| 📖 Interpreter | Behawioralny | Walidacja reguł parkingowych | `Interpreters/ParkingRuleInterpreter.cs` |

---

## 🚀 Rozpocznij teraz!

```bash
# Sklonuj repozytorium
git clone https://github.com/Chx96171/System-rejestracji-parkingowej-w-firmie.git

# Przejdź do katalogu
cd System-rejestracji-parkingowej-w-firmie

# Uruchom aplikację
dotnet run

# Otwórz w przeglądarce
# https://localhost:5001

# Zaloguj się jako Admin
# Email: admin@test.com
# Hasło: Admin123
```

---

**Miłego korzystania! 🅿️**