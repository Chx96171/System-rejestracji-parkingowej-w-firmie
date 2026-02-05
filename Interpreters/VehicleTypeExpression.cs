namespace SystemRejestracjiParkingowej.Interpreters
{
    /// <summary>
    /// Wyrażenie sprawdzające typ pojazdu
    /// Terminal Expression w Interpreter Pattern
    /// </summary>
    public class VehicleTypeExpression : IParkingRuleExpression
    {
        private readonly string _allowedType;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="allowedType">Dozwolony typ pojazdu (Car, Motorcycle, Truck, Bus)</param>
        public VehicleTypeExpression(string allowedType)
        {
            _allowedType = allowedType;
        }

        /// <summary>
        /// Sprawdza czy typ pojazdu jest dozwolony
        /// </summary>
        public bool Interpret(ParkingContext context)
        {
            if (string.IsNullOrEmpty(context.VehicleType))
                return false;

            return context.VehicleType.Equals(_allowedType, StringComparison.OrdinalIgnoreCase);
        }
    }
}
