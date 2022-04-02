using System.Diagnostics.CodeAnalysis;

namespace Calendar.Client.Configuration.Extensions
{
    public class CircuitBreakerOptions
    {
        public bool IsActive { get; set; }
        public double AllowedExceptionPercentageForThreshold { get; set; } = 0.5;
        public int ThresholdWindowDurationInSeconds { get; set; } = 30;
        public int CooldownDurationInSeconds { get; set; } = 30;
        public int MinimumThroughput { get; set; } = 120;
    }
}