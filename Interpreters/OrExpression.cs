// Non-terminal expression - OR
namespace SystemRejestracjiParkingowej.Interpreters
{
    public class OrExpression : IParkingRuleExpression
    {
        private readonly IParkingRuleExpression _left;
        private readonly IParkingRuleExpression _right;

        public OrExpression(IParkingRuleExpression left, IParkingRuleExpression right)
        {
            _left = left;
            _right = right;
        }

        public bool Interpret(ParkingContext context)
        {
            return _left.Interpret(context) || _right.Interpret(context);
        }
    }
}
