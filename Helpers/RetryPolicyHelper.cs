using Polly.Extensions.Http;
using Polly;

namespace EmailSpamDetectionService.Helpers
{
    public static class RetryPolicyHelper
    {
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(3,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (outcome, timespan, retryCount, context) =>
                    {
                        Console.WriteLine($"Retry Attempt: {retryCount}");
                    });
        }
    }
}
