using Forum.WebAPI.Dto_s;
using Forum.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RatingDto>>> GetAsync()
    {
        IEnumerable<RatingDto> ratingDtos = await ratingsService.GetRatingsAsync();

        if (ratingDtos is null || ratingDtos.Count() == 0)
        {
            return NotFound(ratingDtos);
        }

        return Ok(ratingDtos);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] CreateRatingDto createRatingDto)
    {
        await ratingsService.InsertRatingAsync(createRatingDto, User.FindFirstValue(ClaimTypes.NameIdentifier));

        return Ok();
    }
}

