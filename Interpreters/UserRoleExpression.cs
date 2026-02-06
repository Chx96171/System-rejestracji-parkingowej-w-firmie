// Terminal expression - User role
namespace SystemRejestracjiParkingowej.Interpreters
{
    public class UserRoleExpression : IParkingRuleExpression
    {
        private readonly string _requiredRole;

        public UserRoleExpression(string requiredRole)
        {
            _requiredRole = requiredRole;
        }

        public bool Interpret(ParkingContext context)
        {
            if (context.UserRole == null)
                return false;

            return context.UserRole.Equals(_requiredRole, StringComparison.OrdinalIgnoreCase);
        }
    }
}
