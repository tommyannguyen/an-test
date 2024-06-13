using AnNguyen.Abtraction.Models;

namespace AnNguyen.Abtraction;
public interface ISearchEngine
{
    Task<SearchResult> Search(SearchRequest request);
}
