// Terminal expression - Time range
namespace SystemRejestracjiParkingowej.Interpreters
{
    public class TimeRangeExpression : IParkingRuleExpression
    {
        private readonly TimeSpan _startTime;
        private readonly TimeSpan _endTime;

        public TimeRangeExpression(TimeSpan startTime, TimeSpan endTime)
        {
            _startTime = startTime;
            _endTime = endTime;
        }

        public bool Interpret(ParkingContext context)
        {
            var currentTime = context.DateTime.TimeOfDay;
            
            if (_startTime > _endTime)
            {
                return currentTime >= _startTime || currentTime <= _endTime;
            }
            
            return currentTime >= _startTime && currentTime <= _endTime;
        }
    }
}
