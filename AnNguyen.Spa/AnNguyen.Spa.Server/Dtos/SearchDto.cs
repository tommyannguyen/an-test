using AnNguyen.Abtraction.Models;
using System.ComponentModel.DataAnnotations;

namespace AnNguyen.Spa.Server.Dtos;

public record SearchResultDto(IEnumerable<int> Indexes) : SearchResult(Indexes)
{
}

public record SearchRequestDto([Required]string Query, [Required] string Match, [Required] int Limit = 100) : SearchRequest(Query, Match, Limit)
{
}

public static class SearchExtension
{
    public static SearchRequest Convert(this SearchRequestDto requestDto) => new(requestDto.Query, requestDto.Match, requestDto.Limit);

    public static SearchResultDto Convert(this SearchResult searchResult) => new(searchResult.Indexes);
}