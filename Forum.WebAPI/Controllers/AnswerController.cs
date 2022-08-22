using Forum.Entities;
using Forum.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnswerController : ControllerBase
    {
        private readonly IAnswerService answerService;

        public AnswerController(IAnswerService answerService)
        {
            this.answerService = answerService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Answer>> Get()
        {
            IEnumerable<Answer> answers = answerService.GetAnswers();

            if (answers is null || answers.Count() == 0)
            {
                return NotFound(answers);
            }

            return Ok(answers);
        }

        [HttpGet("{id}")]
        public ActionResult<Answer> Get(int id)
        {
            Answer answer = answerService.GetAnswerById(id);

            if (answer is null)
            {
                return NotFound(answer);
            }

            return Ok(answer);
        }

        [HttpPost]
        public ActionResult Post([FromBody] Answer answer)
        {
            answerService.InsertAnswer(answer);

            return Created($"api/answer/{answer.Id}", null);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Answer answer)
        {
            if (id == answer.Id)
            {
                answerService.UpdateAnswer(answer);
                return Ok();
            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            answerService.DeleteAnswer(id);

            return Ok();
        }
    }
}
