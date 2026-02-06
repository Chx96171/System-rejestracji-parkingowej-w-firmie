// Interpreter - Parking rules
namespace SystemRejestracjiParkingowej.Interpreters
{
    public class ParkingRuleInterpreter
    {
        public static IParkingRuleExpression CreateEmployeeParkingRule()
        {
            var adminRole = new UserRoleExpression("Admin");
            var userRole = new UserRoleExpression("User");
            var truckType = new VehicleTypeExpression("Truck");

            var roleCheck = new OrExpression(adminRole, userRole);
            var noTrucks = new NotExpression(truckType);

            return new AndExpression(roleCheck, noTrucks);
        }

        public static IParkingRuleExpression CreateVipParkingRule()
        {
            var adminRole = new UserRoleExpression("Admin");
            var vipZone = new ZoneTypeExpression("VIP");

            return new OrExpression(adminRole, vipZone);
        }

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

        public static IParkingRuleExpression CreateNoTrucksRule()
        {
            var truckType = new VehicleTypeExpression("Truck");
            return new NotExpression(truckType);
        }

        public static IParkingRuleExpression CreateWorkersOnlyRule()
        {
            var adminRole = new UserRoleExpression("Admin");
            var userRole = new UserRoleExpression("User");
            
            return new OrExpression(adminRole, userRole);
        }

        public static IParkingRuleExpression CreateVipAccessRule()
        {
            return new UserRoleExpression("Admin");
        }

        public static bool ExecuteRule(IParkingRuleExpression rule, ParkingContext context)
        {
            return rule.Interpret(context);
        }

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

        public static bool ValidateAccess(string ruleName, ParkingContext context)
        {
            var rule = CreateRuleByName(ruleName);
            if (rule == null)
                return false;

            return ExecuteRule(rule, context);
        }
    }
}
