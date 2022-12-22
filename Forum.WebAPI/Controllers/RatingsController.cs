using Forum.WebAPI.Dto_s;
using Forum.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Forum.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class RatingsController : ControllerBase
{
    private readonly IRatingsService ratingsService;

    public RatingsController(IRatingsService ratingsService)
    {
        this.ratingsService = ratingsService;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] CreateRatingDto createRatingDto)
    {
        await ratingsService.InsertRatingAsync(createRatingDto);

        return Ok();
    }
}

