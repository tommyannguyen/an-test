using HtmlAgilityPack;

namespace AnNguyen.Services.SearchEngine.Google.Parsers;

internal class GoogleRsoHtmlParser : IGoogleHtmlParser
{
    public Dictionary<int, string> Parse(HtmlNode doc)
    {
        var rsoNode = doc.SelectSingleNode("//div[@id='rso']");
        if (rsoNode is null)
        {
            return [];
        }

        var questionsBlock = rsoNode.SelectSingleNode("//div[@data-initq]");
        return (questionsBlock is not null) ? ParseByQuestionsBlock(questionsBlock.ParentNode): ParseLastBlock(rsoNode);
    }
    Dictionary<int, string> ParseLastBlock(HtmlNode rsoNode)
    {
        var resultNode = rsoNode
                        .ChildNodes
                        .LastOrDefault(n => n.Name.Equals("div"));

        if (resultNode is null)
        {
            return [];
        }

        var nodes = resultNode.ChildNodes.Where(n => n.Name.Equals("div"));

        return nodes.Select((x, i) => new { Index = i, x.InnerHtml }).ToDictionary(t => t.Index, t => t.InnerHtml);
    }
    Dictionary<int, string> ParseByQuestionsBlock(HtmlNode questionsBlock)
    {
        var resultNode = new List<HtmlNode>();

        var nextDiv = questionsBlock.NextSibling;
        while (nextDiv is { Name: "div" })
        {
            resultNode.Add(nextDiv);
            nextDiv = nextDiv.NextSibling;
        }

        return resultNode.Select((x, i) => new { Index = i, x.InnerHtml }).ToDictionary(t => t.Index, t => t.InnerHtml);
    }
}
