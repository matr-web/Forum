using AutoMapper;
using Forum.Entities;
using Forum.WebAPI.Dto_s;
using Forum.WebAPI.Repositories;

namespace Forum.WebAPI.Services
{
    public interface IRatingsService
    {
        Task<IEnumerable<RatingDto>> GetRatingsAsync();
        Task<int> InsertRatingAsync(CreateRatingDto createRatingDto);
        Task DeleteRatingAsync(int id);
        Task UpdateRatingAsync(UpdateRatingDto updateRatingDto);
    }

    public class RatingsService : IRatingsService
    {
        private readonly IRatingsRepository ratingsRepository;
        private readonly IMapper mapper;

        public RatingsService(IRatingsRepository ratingRepository, IMapper mapper)
        {
            this.ratingsRepository = ratingRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<RatingDto>> GetRatingsAsync() => mapper.Map<IEnumerable<RatingDto>>(await ratingsRepository.GetRatingsAsync());

        public async Task<int> InsertRatingAsync(CreateRatingDto createRatingDto)
        {
            Rating rating = mapper.Map<Rating>(createRatingDto);

            await ratingsRepository.InsertRatingAsync(rating);
            await ratingsRepository.SaveAsync();

            return rating.Id;
        }

        public async Task UpdateRatingAsync(UpdateRatingDto updateRatingDto)
        {
            Rating rating = mapper.Map<Rating>(updateRatingDto);

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
