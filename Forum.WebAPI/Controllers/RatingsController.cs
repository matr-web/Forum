using Forum.WebAPI.Dto_s;
using Forum.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

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

    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] CreateRatingDto createRatingDto)
    {
        int ratingId = await ratingsService.InsertRatingAsync(createRatingDto);

        return Created($"api/ratings/{ratingId}", null);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] UpdateRatingDto updateRatingDto)
    {
        if (id == updateRatingDto.Id)
        {
            await ratingsService.UpdateRatingAsync(updateRatingDto);
            return Ok(updateRatingDto);
        }

        return NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await ratingsService.DeleteRatingAsync(id);

        return Ok();
    }
}

