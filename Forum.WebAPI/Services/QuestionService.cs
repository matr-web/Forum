using Forum.Entities;
using Forum.WebAPI.Repositories;

namespace Forum.WebAPI.Services
{
    public interface IQuestionService
    {
        public IEnumerable<Question> GetQuestions();
        public Question GetQuestionById(int id);
        public void InsertQuestion(Question question);
        public void DeleteQuestion(int id);
        public void UpdateQuestion(Question question);
    }

    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository questionRepository;

        public QuestionService(IQuestionRepository questionRepository)
        {
            this.questionRepository = questionRepository;
        }

        public IEnumerable<Question> GetQuestions() => questionRepository.GetQuestions();

        public Question GetQuestionById(int id) => questionRepository.GetQuestionByID(id);

        public void InsertQuestion(Question question)
        {
            question.Date = DateTime.Now;

            questionRepository.InsertQuestion(question);
            questionRepository.Save();
        }

        public void UpdateQuestion(Question question)
        {
            question.Date = DateTime.Now;

            questionRepository.UpdateQuestion(question);
            questionRepository.Save();
        }

        public void DeleteQuestion(int id)
        {
            questionRepository.DeleteQuestion(id);
            questionRepository.Save();
        }   

        public void Dispose()
        {
            questionRepository.Dispose();
        }
    }
}
