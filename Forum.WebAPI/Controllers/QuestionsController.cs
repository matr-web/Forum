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
    [HttpGet("GetAll")]
    public async Task<ActionResult<IEnumerable<QuestionDto>>> GetAllAsync([FromQuery] Query query)
    {
        PagedResult<QuestionDto> pagedQuestionsDtos = await questionsService.GetQuestionsAsync(query);

        if (pagedQuestionsDtos is null || pagedQuestionsDtos.Items is null || pagedQuestionsDtos.Items.Count() == 0)
        {
            return NotFound(pagedQuestionsDtos);
        }

        return Ok(pagedQuestionsDtos);
    }

    [AllowAnonymous]
    [HttpGet("Get/{id}")]
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
        int questionId = 0;

        try
        {
            questionId = await questionsService.InsertQuestionAsync(createQuestionDto);
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

        return Created($"Questions/{questionId}", null);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] UpdateQuestionDto updateQuestionDto)
    {
        if (id != updateQuestionDto.Id)
        {
            return NotFound();
        }

        try
        {
            await questionsService.UpdateQuestionAsync(updateQuestionDto);
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

        return Ok(updateQuestionDto);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        try
        {
            await questionsService.DeleteQuestionAsync(id);
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
