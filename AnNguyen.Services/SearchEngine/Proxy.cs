using HtmlAgilityPack;
using System.Linq;
namespace AnNguyen.Services.SearchEngine;

public record ProxyInfo(string IPAddress, int Port,bool Anonymous, bool GoogleSupported);
public static class ProxyHelper
{
    const string url = "https://free-proxy-list.net/";
    public static IEnumerable<ProxyInfo> GetAllProxies()
    {
        var hw = new HtmlWeb();
        var doc = hw.Load(url);
        var proxyTable = doc.DocumentNode.SelectSingleNode("//table");

        var ips = GetIps(proxyTable);
        var ports = GetPorts(proxyTable);
        var supportedGoogle = GetSupportGoogle(proxyTable);
        var anonymous = GetAnonymous(proxyTable);
        return ips.Select((ip, index) => new ProxyInfo(ip, ports[index], anonymous[index],  supportedGoogle[index]))
            .Where(s => s.Anonymous && s.GoogleSupported)
            .ToList();
    }
    
    static List<string> GetIps(HtmlNode proxyTable)
        => proxyTable.SelectNodes("tbody/tr/td[1]").Select(p => p.InnerHtml).ToList();

    static List<int> GetPorts(HtmlNode proxyTable)
        => proxyTable.SelectNodes("tbody/tr/td[2]").Select(p => int.TryParse(p.InnerHtml, out var value) ? value : 0).ToList();


    static List<bool> GetSupportGoogle(HtmlNode proxyTable)
        => proxyTable.SelectNodes("tbody/tr/td[6]").Select(p => p.InnerHtml.Equals("yes")).ToList();

    static List<bool> GetAnonymous(HtmlNode proxyTable)
       => proxyTable.SelectNodes("tbody/tr/td[5]").Select(p => p.InnerHtml.Equals("anonymous")).ToList();

}
