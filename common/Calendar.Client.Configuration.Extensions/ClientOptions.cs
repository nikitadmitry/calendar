
namespace Calendar.Client.Configuration.Extensions
{
    public class ClientOptions<T>
    {
        public string ApiUrl { get; set; }
        public int RequestTimeoutSeconds { get; set; } = 5;
        public int RequestRetries { get; set; } = 1;
        public int MaxConnectionPerServer { get; set; } = 500;
        public CircuitBreakerOptions CircuitBreaker { get; set; }
    }
}