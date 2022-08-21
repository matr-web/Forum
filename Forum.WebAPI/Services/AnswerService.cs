using Forum.Entities;
using Forum.WebAPI.Repositories;

namespace Forum.WebAPI.Services;

public interface IAnswerService
{
    public IEnumerable<Answer> GetAnswers();
    public Answer GetAnswerById(int id);
    public void InsertAnswer(Answer answer);
    public void DeleteAnswer(int id);
    public void UpdateAnswer(Answer answer);
}

public class AnswerService : IAnswerService
{
    private readonly IAnswerRepository AnswerRepository;

    public AnswerService(IAnswerRepository answerRepository)
    {
        AnswerRepository = answerRepository;
    }

    public IEnumerable<Answer> GetAnswers() => AnswerRepository.GetAnswers();

    public Answer GetAnswerById(int id) => AnswerRepository.GetAnswerByID(id);

    public void DeleteAnswer(int id)
    {
        AnswerRepository.DeleteAnswer(id);
        AnswerRepository.Save();
    }

    public void InsertAnswer(Answer answer)
    {
        AnswerRepository.InsertAnswer(answer);
        AnswerRepository.Save();
    }

    public void UpdateAnswer(Answer answer)
    {
        AnswerRepository.UpdateAnswer(answer);
        AnswerRepository.Save();
    }

    public void Dispose()
    {
        AnswerRepository.Dispose();
    }
}
