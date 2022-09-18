using Forum.Entities;
using Microsoft.EntityFrameworkCore;

namespace Forum.WebAPI.Repositories;

public interface IAnswersRepository
{
    Task<IEnumerable<Answer>> GetAnswersAsync();
    Task<Answer> GetAnswerByIdAsync(int answerId);
    Task InsertAnswerAsync(Answer answer);
    Task DeleteAnswerAsync(int answerId);
    Task UpdateAnswerAsync(Answer answer);
    Task SaveAsync();
}

public class AnswersRepository : IAnswersRepository
{
    private DatabaseContext context;

    public AnswersRepository(DatabaseContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<Answer>> GetAnswersAsync()
    {
        return await context.Answers
            .Include(a => a.Ratings)
            .Include(a => a.Author)
            .ToListAsync();
    }

    public async Task<Answer> GetAnswerByIdAsync(int id)
    {
        return await context.Answers
            .Include(a => a.Ratings)
            .Include(a => a.Author)
            .Include(a => a.Question)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task InsertAnswerAsync(Answer answer)
    {
        await context.Answers.AddAsync(answer);
    }

    public async Task DeleteAnswerAsync(int answerID)
    {
        Answer answer = await context.Answers
            .Include(a => a.Ratings)
            .FirstOrDefaultAsync(a => a.Id == answerID);

        context.Answers.Remove(answer);
    }

    public Task UpdateAnswerAsync(Answer answer)
    {
        return Task.FromResult(context.Entry(answer).State = EntityState.Modified);
    }

    public async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }
}
