using AnNguyen.Abtraction.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AnNguyen.Spa.Server;
using Microsoft.Extensions.Configuration;
using AnNguyen.Abtraction;
namespace AnNguyen.Test;

public class SearchEngineTest
{
    ServiceProvider? _service;
    ServiceProvider GetServiceProvider()
    {
        if(_service != null)
            return _service;

        var baseUrl = "https://www.google.co.uk/";
        var maxRetry = 5;
        var configurationSetting = new List<KeyValuePair<string, string?>>
        {
            new ("SearchEngine:Url", baseUrl),
            new ("SearchEngine:Retry", maxRetry.ToString()),
        };

        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection(configurationSetting)
                            .Build();

        var service = new ServiceCollection();
        service.AddLogging(logging => logging.AddConsole());
        service.AddSearchEngine(configuration);
        _service = service.BuildServiceProvider();
        return _service;
    }

    [Fact]
    public async Task TestGotResultFromGoogleForTrackInfo()
    {
        var service = GetServiceProvider();
        var searchEngine = service.GetRequiredService<ISearchEngine>();
        var result = await searchEngine.Search(new SearchRequest("land registry searches", "www.infotrack.co.uk"));

        Assert.NotNull(result);
        Assert.NotEmpty(result.Indexes);
    }

    [Fact]
    public async Task TestGotResultFromGoogleForTinhTe()
    {
        var service = GetServiceProvider();
        var searchEngine = service.GetRequiredService<ISearchEngine>();
        var result = await searchEngine.Search(new SearchRequest("Tinh te", "tinhte.vn"));

        Assert.NotNull(result);
        Assert.NotEmpty(result.Indexes);
    }

    [Fact]
    public async Task TestGotEmpty()
    {
        var service = GetServiceProvider();
        var searchEngine = service.GetRequiredService<ISearchEngine>();
        var result = await searchEngine.Search(new SearchRequest("Tinh Te", "ssssss"));

        Assert.NotNull(result);
        Assert.Empty(result.Indexes);
    }
}
