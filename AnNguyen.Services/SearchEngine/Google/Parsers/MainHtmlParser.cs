using HtmlAgilityPack;

namespace AnNguyen.Services.SearchEngine.Google.Parsers;

internal class MainHtmlParser : IGoogleHtmlParser
{
    public Dictionary<int, string> Parse(HtmlNode doc)
    {
        var mainNode = doc.SelectSingleNode("//div[@id='main']");
        if (mainNode is null)
        {
            return [];
        }
        var nodes = mainNode
                        .ChildNodes
                        .Where(n => n.Name.Equals("div"))
                        .SkipWhile(n => !string.IsNullOrEmpty(n.InnerHtml))
                        .Skip(1);

        return nodes.Select((x, i) => new { Index = i, x.InnerHtml }).ToDictionary(t => t.Index, t => t.InnerHtml);
    }
}