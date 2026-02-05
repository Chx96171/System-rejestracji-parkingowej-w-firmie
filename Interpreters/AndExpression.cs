namespace SystemRejestracjiParkingowej.Interpreters
{
    /// <summary>
    /// Wyrażenie logiczne AND - łączy dwa wyrażenia operatorem AND
    /// Non-Terminal Expression w Interpreter Pattern
    /// </summary>
    public class AndExpression : IParkingRuleExpression
    {
        private readonly IParkingRuleExpression _left;
        private readonly IParkingRuleExpression _right;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="left">Lewe wyrażenie</param>
        /// <param name="right">Prawe wyrażenie</param>
        public AndExpression(IParkingRuleExpression left, IParkingRuleExpression right)
        {
            _left = left;
            _right = right;
        }

        /// <summary>
        /// Interpretuje wyrażenie - zwraca true tylko jeśli oba wyrażenia są prawdziwe
        /// </summary>
        public bool Interpret(ParkingContext context)
        {
            return _left.Interpret(context) && _right.Interpret(context);
        }
    }
}
