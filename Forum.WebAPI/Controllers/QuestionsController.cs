using Forum.Entities;
using Forum.WebAPI.Dto_s;
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
    public async Task<ActionResult<IEnumerable<QuestionDto>>> GetAsync()
    {
        IEnumerable<QuestionDto> questionsDtos = await questionsService.GetQuestionsAsync();

        if (questionsDtos is null || questionsDtos.Count() == 0)
        {
            return NotFound(questionsDtos);
        }

        return Ok(questionsDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<QuestionDto>> GetAsync(int id)
    {
        QuestionDto questionDto = await questionsService.GetQuestionByIdAsync(id);

        if (questionDto is null)
        {
            return NotFound(questionDto);
        }

        return Ok(questionDto);
    }

    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] CreateQuestionDto createQuestionDto)
    {
        int questionId = await questionsService.InsertQuestionAsync(createQuestionDto);

        return Created($"api/questions/{questionId}", null);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] UpdateQuestionDto updateQuestionDto)
    {
        if (id == updateQuestionDto.Id)
        {
            await questionsService.UpdateQuestionAsync(updateQuestionDto);
            return Ok(updateQuestionDto);
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
