using AnNguyen.Abtraction;
using AnNguyen.Services.SearchEngine.Google;
using Polly.Extensions.Http;
using Polly;
using System.Net;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;

namespace AnNguyen.Spa.Server;

public static class Extensions
{
    public static IServiceCollection AddSearchEngine(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddSingleton(new GoogleSearchEngineConfiguration(
                               configuration.GetValue<string>("SearchEngine:Url") ?? string.Empty,
                               configuration.GetValue("SearchEngine:MaxRetry", 5)));
        services.AddScoped<ISearchEngine, GoogleSearchEngine>();
        services.AddHttpClient(configuration);
        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
            .OrResult(msg => msg.StatusCode == HttpStatusCode.Unauthorized)
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
    public static IServiceCollection AddHttpClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<GoogleSearchEngine>("RetryHttpClient", c =>
        {
            c.BaseAddress = new Uri($"{configuration["SearchEngine:Url"]}");
            c.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            c.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/125.0.0.0 Safari/537.36"));
            c.Timeout = TimeSpan.FromMinutes(5);
            c.DefaultRequestHeaders.ConnectionClose = true;

        }).AddPolicyHandler(GetRetryPolicy());

        return services;
    }
}
