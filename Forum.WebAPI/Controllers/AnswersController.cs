using Forum.Entities;
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
    public async Task<ActionResult<IEnumerable<Answer>>> GetAsync()
    {
        IEnumerable<Answer> answers = await answersService.GetAnswersAsync();

        if (answers is null || answers.Count() == 0)
        {
            return NotFound(answers);
        }

        return Ok(answers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Answer>> GetAsync(int id)
    {
        Answer answer = await answersService.GetAnswerByIdAsync(id);

        if (answer is null)
        {
            return NotFound(answer);
        }

        return Ok(answer);
    }

    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] Answer answer)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await answersService.InsertAnswerAsync(answer);

        return Created($"api/answers/{answer.Id}", null);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] Answer answer)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id == answer.Id)
        {
            await answersService.UpdateAnswerAsync(answer);
            return Ok();
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
