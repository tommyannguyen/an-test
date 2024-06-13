using AnNguyen.Abtraction;
using AnNguyen.Abtraction.Models;
using AnNguyen.Spa.Server.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace AnNguyen.Spa.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{

    [HttpPost()]
    public async Task<IActionResult> Search(
        SearchRequestDto requestDto,
        [FromServices] ISearchEngine searchEngine)
    {
        try
        {
            var result = await searchEngine.Search(requestDto.Convert());
            return Ok(result.Convert());
        }
        catch( SearchExeption searchExption)
        {
            return BadRequest(searchExption.Message);
        }
    }
}
