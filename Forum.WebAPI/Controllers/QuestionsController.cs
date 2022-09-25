using Forum.WebAPI.Dto_s;
using Forum.WebAPI.Pagination;
using Forum.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
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

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<QuestionDto>>> GetAsync([FromQuery] Query query)
    {
        PagedResult<QuestionDto> pagedQuestionsDtos = await questionsService.GetQuestionsAsync(query);

        if (pagedQuestionsDtos is null || pagedQuestionsDtos.Items is null || pagedQuestionsDtos.Items.Count() == 0)
        {
            return NotFound(pagedQuestionsDtos);
        }

        return Ok(pagedQuestionsDtos);
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
        int questionId = await questionsService.InsertQuestionAsync(createQuestionDto);

        return Created($"api/questions/{questionId}", null);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] UpdateQuestionDto updateQuestionDto)
    {
        if (id != updateQuestionDto.Id)
        {
            return NotFound();
        }

        await questionsService.UpdateQuestionAsync(updateQuestionDto);

        return Ok(updateQuestionDto);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
       await questionsService.DeleteQuestionAsync(id);

       return Ok();
    }
}
