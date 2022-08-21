using Forum.Entities;
using Forum.WebAPI.Repositories;

namespace Forum.WebAPI.Services;

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
    private readonly IQuestionRepository QuestionRepository;

    public QuestionService(IQuestionRepository questionRepository)
    {
        QuestionRepository = questionRepository;
    }

    public IEnumerable<Question> GetQuestions() => QuestionRepository.GetQuestions();

    public Question GetQuestionById(int id) => QuestionRepository.GetQuestionByID(id);

    public void DeleteQuestion(int id)
    {
        QuestionRepository.DeleteQuestion(id);
        QuestionRepository.Save();
    }

    public void InsertQuestion(Question question)
    {
        QuestionRepository.InsertQuestion(question);
        QuestionRepository.Save();
    }

    public void UpdateQuestion(Question question)
    {
        QuestionRepository.UpdateQuestion(question);
        QuestionRepository.Save();
    }

    public void Dispose()
    {
        QuestionRepository.Dispose();
    }
}
