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
    private readonly IAnswerRepository answerRepository; 

    public AnswerService(IAnswerRepository answerRepository)
    {
        this.answerRepository = answerRepository;
    }

    public IEnumerable<Answer> GetAnswers() => answerRepository.GetAnswers();

    public Answer GetAnswerById(int id) => answerRepository.GetAnswerByID(id);

    public void InsertAnswer(Answer answer)
    {
        answer.Date = DateTime.Now;

        answerRepository.InsertAnswer(answer);
        answerRepository.Save();
    }

    public void UpdateAnswer(Answer answer)
    {
        answer.Date = DateTime.Now;

        answerRepository.UpdateAnswer(answer);
        answerRepository.Save();
    }

    public void DeleteAnswer(int id)
    {
        answerRepository.DeleteAnswer(id);
        answerRepository.Save();
    }
    
    public void Dispose()
    {
        answerRepository.Dispose();
    }
}
