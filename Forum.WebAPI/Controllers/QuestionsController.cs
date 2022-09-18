using Forum.WebAPI.Dto_s;
using Forum.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

    [AllowAnonymous]
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

    [AllowAnonymous]
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

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] CreateQuestionDto createQuestionDto)
    {
        int questionId = await questionsService.InsertQuestionAsync(createQuestionDto, User.FindFirstValue(ClaimTypes.NameIdentifier));

        return Created($"api/questions/{questionId}", null);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] UpdateQuestionDto updateQuestionDto)
    {
        if (id == updateQuestionDto.Id)
        {
            bool updated = await questionsService.UpdateQuestionAsync(updateQuestionDto, User.FindFirstValue(ClaimTypes.NameIdentifier));

            if(updated) return Ok(updateQuestionDto);

            return Unauthorized();
        }

        return NotFound();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        bool deleted = await questionsService.DeleteQuestionAsync(id, User.FindFirstValue(ClaimTypes.NameIdentifier));

        if(deleted) return Ok();

        return Unauthorized();
    }
}
