namespace SystemRejestracjiParkingowej.Interpreters
{
    /// <summary>
    /// Interpreter reguł parkingowych - buduje i wykonuje drzewo wyrażeń
    /// Wzorzec projektowy: Interpreter Pattern
    /// </summary>
    public class ParkingRuleInterpreter
    {
        /// <summary>
        /// Tworzy regułę dla parkingu pracowniczego
        /// Reguła: (UserRole = "Admin" OR UserRole = "User") AND NOT (VehicleType = "Truck")
        /// </summary>
        public static IParkingRuleExpression CreateEmployeeParkingRule()
        {
            var adminRole = new UserRoleExpression("Admin");
            var userRole = new UserRoleExpression("User");
            var truckType = new VehicleTypeExpression("Truck");

            var roleCheck = new OrExpression(adminRole, userRole);
            var noTrucks = new NotExpression(truckType);

            return new AndExpression(roleCheck, noTrucks);
        }

        /// <summary>
        /// Tworzy regułę dla parkingu VIP
        /// Reguła: UserRole = "Admin" OR ZoneType = "VIP"
        /// </summary>
        public static IParkingRuleExpression CreateVipParkingRule()
        {
            var adminRole = new UserRoleExpression("Admin");
            var vipZone = new ZoneTypeExpression("VIP");

            return new OrExpression(adminRole, vipZone);
        }

        /// <summary>
        /// Tworzy regułę dla parkingu publicznego
        /// Reguła: TimeRange(6:00-22:00) AND NOT (VehicleType = "Truck" OR VehicleType = "Bus")
        /// </summary>
        public static IParkingRuleExpression CreatePublicParkingRule()
        {
            var timeRange = new TimeRangeExpression(
                new TimeSpan(6, 0, 0), 
                new TimeSpan(22, 0, 0));
            
            var truckType = new VehicleTypeExpression("Truck");
            var busType = new VehicleTypeExpression("Bus");
            
            var largeVehicles = new OrExpression(truckType, busType);
            var noLargeVehicles = new NotExpression(largeVehicles);

            return new AndExpression(timeRange, noLargeVehicles);
        }

        /// <summary>
        /// Tworzy regułę zakazu ciężarówek
        /// Reguła: NOT (VehicleType = "Truck")
        /// </summary>
        public static IParkingRuleExpression CreateNoTrucksRule()
        {
            var truckType = new VehicleTypeExpression("Truck");
            return new NotExpression(truckType);
        }

        /// <summary>
        /// Tworzy regułę tylko dla pracowników
        /// Reguła: UserRole = "Admin" OR UserRole = "User"
        /// </summary>
        public static IParkingRuleExpression CreateWorkersOnlyRule()
        {
            var adminRole = new UserRoleExpression("Admin");
            var userRole = new UserRoleExpression("User");
            
            return new OrExpression(adminRole, userRole);
        }

        /// <summary>
        /// Tworzy regułę dostępu VIP
        /// Reguła: UserRole = "Admin"
        /// </summary>
        public static IParkingRuleExpression CreateVipAccessRule()
        {
            return new UserRoleExpression("Admin");
        }

        /// <summary>
        /// Wykonuje regułę w danym kontekście
        /// </summary>
        /// <param name="rule">Reguła do wykonania</param>
        /// <param name="context">Kontekst parkingowy</param>
        /// <returns>True jeśli reguła jest spełniona</returns>
        public static bool ExecuteRule(IParkingRuleExpression rule, ParkingContext context)
        {
            return rule.Interpret(context);
        }

        /// <summary>
        /// Tworzy niestandardową regułę na podstawie nazwy
        /// </summary>
        /// <param name="ruleName">Nazwa reguły (WorkersOnly, VIPAccess, NoTrucks, PublicParking, EmployeeParking, VIPParking)</param>
        /// <returns>Wyrażenie reguły</returns>
        public static IParkingRuleExpression? CreateRuleByName(string ruleName)
        {
            return ruleName.ToLower() switch
            {
                "workersonly" => CreateWorkersOnlyRule(),
                "vipaccess" => CreateVipAccessRule(),
                "notrucks" => CreateNoTrucksRule(),
                "publicparking" => CreatePublicParkingRule(),
                "employeeparking" => CreateEmployeeParkingRule(),
                "vipparking" => CreateVipParkingRule(),
                _ => null
            };
        }

        /// <summary>
        /// Waliduje dostęp na podstawie nazwy reguły
        /// </summary>
        /// <param name="ruleName">Nazwa reguły</param>
        /// <param name="context">Kontekst parkingowy</param>
        /// <returns>True jeśli dostęp jest dozwolony</returns>
        public static bool ValidateAccess(string ruleName, ParkingContext context)
        {
            var rule = CreateRuleByName(ruleName);
            if (rule == null)
                return false;

            return ExecuteRule(rule, context);
        }
    }
}
