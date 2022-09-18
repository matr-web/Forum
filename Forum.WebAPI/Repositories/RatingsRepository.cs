using Forum.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Forum.WebAPI.Repositories;

public interface IRatingsRepository 
{
    Task<IEnumerable<Rating>> GetRatingsAsync();
    IQueryable<Rating> GetRatings(Expression<Func<Rating, bool>> predicate);
    Task InsertRatingAsync(Rating rating);
    Task DeleteRatingAsync(int ratingId);
    Task UpdateRatingAsync(Rating rating);
    Task SaveAsync();
}

public class RatingsRepository : IRatingsRepository
{
    private DatabaseContext context;

    public RatingsRepository(DatabaseContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<Rating>> GetRatingsAsync()
    {
        return await context.Ratings.ToListAsync();
    }

    public IQueryable<Rating> GetRatings(Expression<Func<Rating, bool>> predicate)
    {
        return context.Ratings.Where(predicate);
    }

    public async Task InsertRatingAsync(Rating rating)
    {
        await context.Ratings.AddAsync(rating);
    }

    public async Task DeleteRatingAsync(int ratingId)
    {
        Rating rating = await context.Ratings.FindAsync(ratingId);
        context.Ratings.Remove(rating);
    }

    public Task UpdateRatingAsync(Rating rating)
    {
        return Task.FromResult(context.Entry(rating).State = EntityState.Modified);
    }

    public async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }
}
