// Interpreter interface
namespace SystemRejestracjiParkingowej.Interpreters
{
    public interface IParkingRuleExpression
    {
        bool Interpret(ParkingContext context);
    }
}
