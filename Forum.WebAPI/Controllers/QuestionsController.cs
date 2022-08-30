using Forum.Entities;
using Forum.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Forum.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class QuestionsController : ControllerBase
{
    private readonly IQuestionsService questionsService;

    public QuestionsController(IQuestionsService questionsService)
    {
        this.questionsService = questionsService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Question>>> GetAsync()
    {
        IEnumerable<Question> questions = await questionsService.GetQuestionsAsync();

        if (questions is null || questions.Count() == 0)
        {
            return NotFound(questions);
        }

        return Ok(questions);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Question>> GetAsync(int id)
    {
        Question question = await questionsService.GetQuestionByIdAsync(id);

        if (question is null)
        {
            return NotFound(question);
        }

        return Ok(question);
    }

    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] Question question)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await questionsService.InsertQuestionAsync(question);

        return Created($"api/questions/{question.Id}", null);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] Question question)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id == question.Id)
        {
            await questionsService.UpdateQuestionAsync(question);
            return Ok();
        }

        return NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await questionsService.DeleteQuestionAsync(id);

        return Ok();
    }
}
