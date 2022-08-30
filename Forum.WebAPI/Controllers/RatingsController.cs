using Forum.Entities;
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
    public async Task<ActionResult<IEnumerable<Rating>>> GetAsync()
    {
        IEnumerable<Rating> ratings = await ratingsService.GetRatingsAsync();

        if (ratings is null || ratings.Count() == 0)
        {
            return NotFound(ratings);
        }

        return Ok(ratings);
    }

    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] Rating rating)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await ratingsService.InsertRatingAsync(rating);

        return Created($"api/ratings/{rating.Id}", null);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] Rating rating)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id == rating.Id)
        {
            await ratingsService.UpdateRatingAsync(rating);
            return Ok();
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

