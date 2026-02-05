namespace SystemRejestracjiParkingowej.Interpreters
{
    /// <summary>
    /// Wyrażenie sprawdzające rolę użytkownika
    /// Terminal Expression w Interpreter Pattern
    /// </summary>
    public class UserRoleExpression : IParkingRuleExpression
    {
        private readonly string _requiredRole;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="requiredRole">Wymagana rola (Admin, User, VIP)</param>
        public UserRoleExpression(string requiredRole)
        {
            _requiredRole = requiredRole;
        }

        /// <summary>
        /// Sprawdza czy użytkownik ma wymaganą rolę
        /// </summary>
        public bool Interpret(ParkingContext context)
        {
            if (context.UserRole == null)
                return false;

            return context.UserRole.Equals(_requiredRole, StringComparison.OrdinalIgnoreCase);
        }
    }
}
