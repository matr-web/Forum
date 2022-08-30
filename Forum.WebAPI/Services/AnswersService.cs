using Forum.Entities;
using Forum.WebAPI.Repositories;

namespace Forum.WebAPI.Services;

public interface IAnswersService
{
    Task<IEnumerable<Answer>> GetAnswersAsync();
    Task<Answer> GetAnswerByIdAsync(int id);
    Task InsertAnswerAsync(Answer answer);
    Task DeleteAnswerAsync(int id);
    Task UpdateAnswerAsync(Answer answer);
}

public class AnswersService : IAnswersService
{
    private readonly IAnswersRepository answersRepository; 

    public AnswersService(IAnswersRepository answerRepository)
    {
        this.answersRepository = answerRepository;
    }

    public async Task<IEnumerable<Answer>> GetAnswersAsync() => await answersRepository.GetAnswersAsync();

    public async Task<Answer> GetAnswerByIdAsync(int id) => await answersRepository.GetAnswerByIDAsync(id);

    public async Task InsertAnswerAsync(Answer answer)
    {
        answer.Date = DateTime.Now;

        await answersRepository.InsertAnswerAsync(answer);
        await answersRepository.SaveAsync();
    }

    public async Task UpdateAnswerAsync(Answer answer)
    {
        answer.Date = DateTime.Now;

        await answersRepository.UpdateAnswerAsync(answer);
        await answersRepository.SaveAsync();
    }

    public async Task DeleteAnswerAsync(int id)
    {
        await answersRepository.DeleteAnswerAsync(id);
        await answersRepository.SaveAsync();
    }
}
