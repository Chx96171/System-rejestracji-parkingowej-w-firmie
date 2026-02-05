namespace SystemRejestracjiParkingowej.Interpreters
{
    /// <summary>
    /// Wyrażenie sprawdzające zakres czasowy
    /// Terminal Expression w Interpreter Pattern
    /// </summary>
    public class TimeRangeExpression : IParkingRuleExpression
    {
        private readonly TimeSpan _startTime;
        private readonly TimeSpan _endTime;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="startTime">Początek zakresu czasowego</param>
        /// <param name="endTime">Koniec zakresu czasowego</param>
        public TimeRangeExpression(TimeSpan startTime, TimeSpan endTime)
        {
            _startTime = startTime;
            _endTime = endTime;
        }

        /// <summary>
        /// Sprawdza czy czas operacji mieści się w dozwolonym zakresie
        /// </summary>
        public bool Interpret(ParkingContext context)
        {
            var currentTime = context.DateTime.TimeOfDay;
            
            // Obsługa zakresu przechodzącego przez północ (np. 22:00 - 06:00)
            if (_startTime > _endTime)
            {
                return currentTime >= _startTime || currentTime <= _endTime;
            }
            
            return currentTime >= _startTime && currentTime <= _endTime;
        }
    }
}
