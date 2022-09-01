using Forum.WebAPI.Dto_s;
using Forum.WebAPI.Services;
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

    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] CreateAnswerDto creatAnswerDto)
    {
        int answerId = await answersService.InsertAnswerAsync(creatAnswerDto);

        return Created($"api/answers/{answerId}", null);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] UpdateAnswerDto updateAnswerDto)
    {
        if (id == updateAnswerDto.Id)
        {
            await answersService.UpdateAnswerAsync(updateAnswerDto);
            return Ok(updateAnswerDto);
        }

        return NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await answersService.DeleteAnswerAsync(id);

        return Ok();
    }   
}
