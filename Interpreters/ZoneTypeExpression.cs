// Terminal expression - Zone type
namespace SystemRejestracjiParkingowej.Interpreters
{
    public class ZoneTypeExpression : IParkingRuleExpression
    {
        private readonly string _requiredZoneType;

        public ZoneTypeExpression(string requiredZoneType)
        {
            _requiredZoneType = requiredZoneType;
        }

        public bool Interpret(ParkingContext context)
        {
            if (string.IsNullOrEmpty(context.ZoneType))
                return false;

            return context.ZoneType.Equals(_requiredZoneType, StringComparison.OrdinalIgnoreCase);
        }
    }
}
