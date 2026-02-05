namespace SystemRejestracjiParkingowej.Interpreters
{
    /// <summary>
    /// Wyrażenie sprawdzające typ strefy parkingowej
    /// Terminal Expression w Interpreter Pattern
    /// </summary>
    public class ZoneTypeExpression : IParkingRuleExpression
    {
        private readonly string _requiredZoneType;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="requiredZoneType">Wymagany typ strefy (Public, Employee, VIP)</param>
        public ZoneTypeExpression(string requiredZoneType)
        {
            _requiredZoneType = requiredZoneType;
        }

        /// <summary>
        /// Sprawdza czy typ strefy pasuje do wymaganego
        /// </summary>
        public bool Interpret(ParkingContext context)
        {
            if (string.IsNullOrEmpty(context.ZoneType))
                return false;

            return context.ZoneType.Equals(_requiredZoneType, StringComparison.OrdinalIgnoreCase);
        }
    }
}
