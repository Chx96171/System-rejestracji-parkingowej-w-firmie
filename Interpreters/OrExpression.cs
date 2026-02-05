namespace SystemRejestracjiParkingowej.Interpreters
{
    /// <summary>
    /// Wyrażenie logiczne OR - łączy dwa wyrażenia operatorem OR
    /// Non-Terminal Expression w Interpreter Pattern
    /// </summary>
    public class OrExpression : IParkingRuleExpression
    {
        private readonly IParkingRuleExpression _left;
        private readonly IParkingRuleExpression _right;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="left">Lewe wyrażenie</param>
        /// <param name="right">Prawe wyrażenie</param>
        public OrExpression(IParkingRuleExpression left, IParkingRuleExpression right)
        {
            _left = left;
            _right = right;
        }

        /// <summary>
        /// Interpretuje wyrażenie - zwraca true jeśli przynajmniej jedno wyrażenie jest prawdziwe
        /// </summary>
        public bool Interpret(ParkingContext context)
        {
            return _left.Interpret(context) || _right.Interpret(context);
        }
    }
}
