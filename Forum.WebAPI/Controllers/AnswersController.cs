using Forum.WebAPI.Dto_s;
using Forum.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Forum.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AnswersController : ControllerBase
{
    private readonly IAnswersService answersService;

    public AnswersController(IAnswersService answersService)
    {
        this.answersService = answersService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AnswerDto>>> GetAsync()
    {
        IEnumerable<AnswerDto> answersDtos = await answersService.GetAnswersAsync();

        if (answersDtos is null || answersDtos.Count() == 0)
        {
            return NotFound(answersDtos);
        }

        return Ok(answersDtos);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<AnswerDto>> GetAsync(int id)
    {
        AnswerDto answerDto = await answersService.GetAnswerByIdAsync(id);

        if (answerDto is null)
        {
            return NotFound(answerDto);
        }

        return Ok(answerDto);
    }

    [Authorize]
    [HttpPost("{qestionId}")]
    public async Task<ActionResult> PostAsync(int qestionId, [FromBody] CreateAnswerDto creatAnswerDto)
    {
        int answerId = await answersService.InsertAnswerAsync(qestionId, creatAnswerDto, User.FindFirstValue(ClaimTypes.NameIdentifier));

        return Created($"api/answers/{answerId}", null);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] UpdateAnswerDto updateAnswerDto)
    {
        if (id == updateAnswerDto.Id)
        {
            bool updated = await answersService.UpdateAnswerAsync(updateAnswerDto, User.FindFirstValue(ClaimTypes.NameIdentifier));
           
            if(updated) return Ok(updateAnswerDto);

            return Unauthorized();
        }

        return NotFound();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        bool deleted = await answersService.DeleteAnswerAsync(id, User.FindFirstValue(ClaimTypes.NameIdentifier));

        if(deleted) return Ok();

        return Unauthorized();
    }   
}
