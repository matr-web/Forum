using Forum.Entities;
using Forum.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService questionService;

        public QuestionController(IQuestionService questionService)
        {
            this.questionService = questionService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Question>> Get()
        {
            IEnumerable<Question> questions = questionService.GetQuestions();

            if (questions is null || questions.Count() == 0)
            {
                return NotFound(questions);
            }

            return Ok(questions);
        }

        [HttpGet("{id}")]
        public ActionResult<Question> Get(int id)
        {
            Question question = questionService.GetQuestionById(id);

            if (question is null)
            {
                return NotFound(question);
            }

            return Ok(question);
        }

        [HttpPost]
        public ActionResult Post([FromBody] Question question)
        {
            questionService.InsertQuestion(question);

            return Created($"api/question/{question.Id}", null);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Question question)
        {
            if (id == question.Id)
            {
                questionService.UpdateQuestion(question);
                return Ok();
            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            questionService.DeleteQuestion(id);

            return Ok();
        }
    }
}
