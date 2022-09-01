using Forum.Entities;
using Microsoft.EntityFrameworkCore;

namespace Forum.WebAPI.Repositories;

public interface IQuestionsRepository
{
    Task<IEnumerable<Question>> GetQuestionsAsync();
    Task<Question> GetQuestionByIDAsync(int questionId);
    Task InsertQuestionAsync(Question question);
    Task DeleteQuestionAsync(int questionId);
    Task UpdateQuestionAsync(Question question);
    Task SaveAsync();
}

public class QuestionsRepository : IQuestionsRepository
{
    private DatabaseContext context;

    public QuestionsRepository(DatabaseContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<Question>> GetQuestionsAsync()
    {
        return await context.Questions
            .Include(q => q.Ratings)
            .Include(q => q.Author)
            .ToListAsync();
    }

    public async Task<Question> GetQuestionByIDAsync(int id)
    {
        return await context.Questions
            .Include(q => q.Answers)
                .ThenInclude(a => a.Ratings)
            .Include(q => q.Answers)
                .ThenInclude(a => a.Author)
            .Include(q => q.Ratings)
            .Include(q => q.Author)
            .FirstOrDefaultAsync(q => q.Id == id);
    }

    public async Task InsertQuestionAsync(Question question)
    {
        await context.Questions.AddAsync(question);
    }

    public async Task DeleteQuestionAsync(int questionID)
    {
        Question question = await context.Questions
            .Include(q => q.Answers)
            .ThenInclude(a => a.Ratings)
            .Include(q => q.Ratings)
            .FirstOrDefaultAsync(q => q.Id == questionID);
        
        context.Questions.Remove(question);
    }

    public async Task UpdateQuestionAsync(Question question)
    {
        await Task.FromResult(context.Entry(question).State = EntityState.Modified);
    }

    public async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }
}
