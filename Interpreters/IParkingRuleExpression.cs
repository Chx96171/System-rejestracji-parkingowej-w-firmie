namespace SystemRejestracjiParkingowej.Interpreters
{
    /// <summary>
    /// Interfejs dla wyrażeń reguł parkingowych
    /// Wzorzec projektowy: Interpreter Pattern
    /// </summary>
    public interface IParkingRuleExpression
    {
        /// <summary>
        /// Interpretuje i waliduje regułę w danym kontekście
        /// </summary>
        /// <param name="context">Kontekst parkingowy zawierający dane do walidacji</param>
        /// <returns>True jeśli reguła jest spełniona</returns>
        bool Interpret(ParkingContext context);
    }
}
