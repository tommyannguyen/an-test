using AnNguyen.Abtraction;
using AnNguyen.Abtraction.Models;
using AnNguyen.Services.SearchEngine.Google.Parsers;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace AnNguyen.Services.SearchEngine.Google;

public record GoogleSearchEngineConfiguration(string BaseUrl, int MaxRetry = 5);
public class GoogleSearchEngine(
    HttpClient httpClient,
    GoogleSearchEngineConfiguration configuration,
    ILoggerFactory loggerFactory) : ISearchEngine
{
    readonly ILogger _logger = loggerFactory.CreateLogger<GoogleSearchEngine>();
    readonly List<IGoogleHtmlParser> parsers =
    [
        new GoogleRsoHtmlParser(),
        new MainHtmlParser()
    ];

    public async Task<SearchResult> Search(SearchRequest request)
    {
        var url = BuildQuery(request.Query);
        var indexes = await ExtractHtml(BuildGoogleQuery(url, request.Limit));

        return new SearchResult(indexes
                                .Where(s => s.Value.Contains(request.Match))
                                .Select(s => s.Key));
    }

    async Task<Dictionary<int, string>> ExtractHtml(string url)
    {
        var doc = await LoadHtml(url);
        if (doc is null)
        {
            return [];
        }

        foreach(var parser in parsers)
        {
            var result = parser.Parse(doc);
            if (result.Count != 0)
            {
                _logger.LogInformation($"{nameof(parser)} : {result.Count}");
                return result;
            }
        }
        return [];
    }

    async Task<HtmlNode?> LoadHtml(string url)
    {
        var html = await httpClient.GetStringAsync(url);
        var maxRetry = configuration.MaxRetry;
        do
        {
            HtmlDocument doc = new();
            doc.LoadHtml(html);
            if (doc is { DocumentNode: not null })
            {
                var isBotDetected = IsBotDetected(doc.DocumentNode);
                if (isBotDetected)
                {
                    throw new GoogleErrorExeption("Google have detected unusual traffic from our computer network");
                }
                return doc.DocumentNode;
            }
            maxRetry--;
        }
        while ( maxRetry > 0);

        return null;
    }

    bool IsBotDetected(HtmlNode documentNode)
    {
        var error = documentNode.SelectSingleNode("//div[@id='recaptcha']");
        return error is not null;
    }
    string BuildQuery(string query)
    {
        if (string.IsNullOrEmpty(query)) return string.Empty;

        var terms = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return string.Join('+', terms);
    }

    string BuildGoogleQuery(string q, int num) => $"{configuration.BaseUrl}/search?num={num}&q={q}";
}