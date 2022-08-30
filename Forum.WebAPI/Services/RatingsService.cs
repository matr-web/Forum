using Forum.Entities;
using Forum.WebAPI.Repositories;

namespace Forum.WebAPI.Services
{
    public interface IRatingsService
    {
        Task<IEnumerable<Rating>> GetRatingsAsync();
        Task InsertRatingAsync(Rating rating);
        Task DeleteRatingAsync(int id);
        Task UpdateRatingAsync(Rating rating);
    }

    public class RatingsService : IRatingsService
    {
        private readonly IRatingsRepository ratingsRepository;

        public RatingsService(IRatingsRepository ratingRepository)
        {
            this.ratingsRepository = ratingRepository;
        }

        public async Task<IEnumerable<Rating>> GetRatingsAsync() => await ratingsRepository.GetRatingsAsync();

        public async Task InsertRatingAsync(Rating rating)
        {
            await ratingsRepository.InsertRatingAsync(rating);
            await ratingsRepository.SaveAsync();
        }

        public async Task UpdateRatingAsync(Rating rating)
        {
            await ratingsRepository.UpdateRatingAsync(rating);
            await ratingsRepository.SaveAsync();
        }

        public async Task DeleteRatingAsync(int id)
        {
            await ratingsRepository.DeleteRatingAsync(id);
            await ratingsRepository.SaveAsync();
        }
    }
}
