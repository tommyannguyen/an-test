using HtmlAgilityPack;

namespace AnNguyen.Services.SearchEngine.Google.Parsers;

internal interface IGoogleHtmlParser
{
    Dictionary<int, string> Parse(HtmlNode doc);
}
