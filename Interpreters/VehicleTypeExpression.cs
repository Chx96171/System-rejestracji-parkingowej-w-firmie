// Terminal expression - Vehicle type
namespace SystemRejestracjiParkingowej.Interpreters
{
    public class VehicleTypeExpression : IParkingRuleExpression
    {
        private readonly string _allowedType;

        public VehicleTypeExpression(string allowedType)
        {
            _allowedType = allowedType;
        }

        public bool Interpret(ParkingContext context)
        {
            if (string.IsNullOrEmpty(context.VehicleType))
                return false;

            return context.VehicleType.Equals(_allowedType, StringComparison.OrdinalIgnoreCase);
        }
    }
}
