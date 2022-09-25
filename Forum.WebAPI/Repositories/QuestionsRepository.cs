using Forum.Entities;
using Forum.WebAPI.Dto_s;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Forum.WebAPI.Repositories;

public interface IQuestionsRepository
{
    Task<Dictionary<int, IEnumerable<Question>>> GetQuestionsAsync(Query query);
    Task<Question> GetQuestionByIdAsync(int questionId);
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

    public Task<Dictionary<int, IEnumerable<Question>>> GetQuestionsAsync(Query query)
    {
        IQueryable<Question> baseQuery = context.Questions
            .Include(q => q.Ratings)
            .Include(q => q.Author)
            .Where(q => query.SearchPhrase == null 
            || q.Topic.ToLower().Contains(query.SearchPhrase.ToLower())
            || q.Content.ToLower().Contains(query.SearchPhrase.ToLower()));

        if(!string.IsNullOrEmpty(query.SortBy))
        {
            var columnSelector = new Dictionary<string, Expression<Func<Question, object>>>
            {
                { nameof(Question.Topic), q => q.Topic },
                { nameof(Question.Date), q => q.Date },
            };

            baseQuery = query.SortOrder == SortOrder.ASC ?
                baseQuery.OrderBy(columnSelector[query.SortBy])
                : baseQuery.OrderByDescending(columnSelector[query.SortBy]);
        }

        IQueryable<Question> queryResult = baseQuery
            .Skip(query.PageSize * (query.PageNumber - 1))
            .Take(query.PageSize);

        Dictionary<int, IEnumerable<Question>> openWith = new Dictionary<int, IEnumerable<Question>>();

        openWith.Add(1, baseQuery.AsEnumerable());
        openWith.Add(2, queryResult.AsEnumerable());

        return Task.FromResult(openWith);
    }

    public async Task<Question> GetQuestionByIdAsync(int id)
    {
        return await context.Questions
            .Include(q => q.Answers).ThenInclude(a => a.Ratings)
            .Include(q => q.Answers).ThenInclude(a => a.Author)
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

    public Task UpdateQuestionAsync(Question question)
    {
        return Task.FromResult(context.Entry(question).State = EntityState.Modified);
    }

    public async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }
}
