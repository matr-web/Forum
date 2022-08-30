using Forum.Entities;
using Microsoft.EntityFrameworkCore;

namespace Forum.WebAPI.Repositories;

public interface IRatingsRepository 
{
    Task<IEnumerable<Rating>> GetRatingsAsync();
    Task<Rating> GetRatingByIDAsync(int ratingId);
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

    public async Task<Rating> GetRatingByIDAsync(int id)
    {
        return await context.Ratings.FindAsync(id);
    }

    public async Task InsertRatingAsync(Rating rating)
    {
        await context.Ratings.AddAsync(rating);
    }

    public async Task DeleteRatingAsync(int ratingID)
    {
        Rating rating = await context.Ratings.FindAsync(ratingID);
        context.Ratings.Remove(rating);
    }

    public async Task UpdateRatingAsync(Rating rating)
    {
        await Task.FromResult(context.Entry(rating).State = EntityState.Modified);
    }

    public async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }
}
