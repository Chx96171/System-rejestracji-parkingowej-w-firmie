namespace SystemRejestracjiParkingowej.Interpreters
{
    /// <summary>
    /// Wyrażenie logiczne NOT - neguje wyrażenie
    /// Non-Terminal Expression w Interpreter Pattern
    /// </summary>
    public class NotExpression : IParkingRuleExpression
    {
        private readonly IParkingRuleExpression _expression;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="expression">Wyrażenie do zanegowania</param>
        public NotExpression(IParkingRuleExpression expression)
        {
            _expression = expression;
        }

        /// <summary>
        /// Interpretuje wyrażenie - zwraca odwrotność wyniku wyrażenia
        /// </summary>
        public bool Interpret(ParkingContext context)
        {
            return !_expression.Interpret(context);
        }
    }
}
