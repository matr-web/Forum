using Forum.WebAPI.Dto_s;
using Forum.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    [HttpGet("GetAll")]
    public async Task<ActionResult<IEnumerable<AnswerDto>>> GetAllAsync()
    {
        IEnumerable<AnswerDto> answersDtos = await answersService.GetAnswersAsync();

        if (answersDtos is null)
        {
            return NotFound(answersDtos);
        }

        return Ok(answersDtos);
    }

    [AllowAnonymous]
    [HttpGet("Get/{id}")]
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
        int answerId = 0;

        try
        {
            answerId = await answersService.InsertAnswerAsync(qestionId, creatAnswerDto);
        }
        catch (Exception ex)
        {
            if (ex.Message == "400")
                return BadRequest();
            else if (ex.Message == "401")
                return Unauthorized();
            else
                throw;
        }

        return Created($"Answers/{answerId}", null);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] UpdateAnswerDto updateAnswerDto)
    {
        if (id != updateAnswerDto.Id)
        {
            return NotFound();
        }

        try
        {
            await answersService.UpdateAnswerAsync(updateAnswerDto);
        }
        catch (Exception ex)
        {
            if (ex.Message == "404")
                return NotFound();
            else if (ex.Message == "403")
                return Forbid();
            else
                throw;
        }

        return Ok(updateAnswerDto);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        try
        {
            await answersService.DeleteAnswerAsync(id);
        }
        catch (Exception ex)
        {
            if (ex.Message == "404")
                return NotFound();
            else if (ex.Message == "403")
                return Forbid();
            else
                throw;
        }

        return NoContent();
    }   
}
