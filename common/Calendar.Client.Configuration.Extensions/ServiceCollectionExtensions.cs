using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using Refit;

namespace Calendar.Client.Configuration.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private static IAsyncPolicy<HttpResponseMessage>? _circuitBreakerPolicy;

        public static IServiceCollection AddClient<T>(this IServiceCollection services,
            Action<IHttpClientBuilder>? setupClient = null,
            Action<ClientOptions<T>>? setupOptions = null) where T : class
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            
            var clientBuilder = services.AddRefitClient<T>()
                .ConfigureHttpClient(ConfigureHttpClient<T>)
                .AddPolicyHandler((provider, _) => GetRetryPolicy<T>(provider))
                .AddPolicyHandler((provider, _) => GetCircuitBreakerPolicy<T>(provider))
                .AddPolicyHandler((provider, _) => GetTimeoutPolicy<T>(provider));

            setupClient?.Invoke(clientBuilder);

            if (setupOptions != null)
            {
                services.Configure(setupOptions);
            }

            return services;
        }

        private static void ConfigureHttpClient<T>(IServiceProvider serviceProvider, HttpClient client)
        {
            var options = serviceProvider.GetRequiredService<IOptions<ClientOptions<T>>>().Value;
            client.BaseAddress = new Uri(options.ApiUrl);
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy<T>(IServiceProvider services)
        {
            var options = services.GetRequiredService<IOptions<ClientOptions<T>>>().Value;

            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .Or<TimeoutRejectedException>()
                .WaitAndRetryAsync(options.RequestRetries,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        private static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy<T>(IServiceProvider services)
        {
            var options = services.GetRequiredService<IOptions<ClientOptions<T>>>().Value;

            return Policy.TimeoutAsync<HttpResponseMessage>(options.RequestTimeoutSeconds);
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy<T>(IServiceProvider services)
        {
            var options = services.GetRequiredService<IOptions<ClientOptions<T>>>().Value;
            return _circuitBreakerPolicy ??= CreateCircuitBreakerPolicy(options.CircuitBreaker ?? new CircuitBreakerOptions());
        }

        private static IAsyncPolicy<HttpResponseMessage> CreateCircuitBreakerPolicy(CircuitBreakerOptions options)
        {
            if (!options.IsActive)
            {
                return Policy.NoOpAsync<HttpResponseMessage>();
            }

            var policy = Policy<HttpResponseMessage>
                .Handle<Exception>();

            return policy
                .AdvancedCircuitBreakerAsync(
                    options.AllowedExceptionPercentageForThreshold,
                    TimeSpan.FromSeconds(options.ThresholdWindowDurationInSeconds),
                    options.MinimumThroughput,
                    TimeSpan.FromSeconds(options.CooldownDurationInSeconds));
        }

    }
}