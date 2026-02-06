// Non-terminal expression - NOT
namespace SystemRejestracjiParkingowej.Interpreters
{
    public class NotExpression : IParkingRuleExpression
    {
        private readonly IParkingRuleExpression _expression;

        public NotExpression(IParkingRuleExpression expression)
        {
            _expression = expression;
        }

        public bool Interpret(ParkingContext context)
        {
            return !_expression.Interpret(context);
        }
    }
}
