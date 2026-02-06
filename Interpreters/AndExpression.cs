// Non-terminal expression - AND
namespace SystemRejestracjiParkingowej.Interpreters
{
    public class AndExpression : IParkingRuleExpression
    {
        private readonly IParkingRuleExpression _left;
        private readonly IParkingRuleExpression _right;

        public AndExpression(IParkingRuleExpression left, IParkingRuleExpression right)
        {
            _left = left;
            _right = right;
        }

        public bool Interpret(ParkingContext context)
        {
            return _left.Interpret(context) && _right.Interpret(context);
        }
    }
}
