using AutoMapper;
using Forum.Entities;
using Forum.WebAPI.Dto_s;
using Forum.WebAPI.Repositories;

namespace Forum.WebAPI.Services
{
    public interface IRatingsService
    {
        Task<IEnumerable<RatingDto>> GetRatingsAsync();
        Task InsertRatingAsync(CreateRatingDto createRatingDto, string authorId);
    }

    public class RatingsService : IRatingsService
    {
        private readonly IRatingsRepository ratingsRepository;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public RatingsService(IRatingsRepository ratingsRepository, IUserRepository userRepository, IMapper mapper)
        {
            this.ratingsRepository = ratingsRepository;
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<RatingDto>> GetRatingsAsync() => mapper.Map<IEnumerable<RatingDto>>(await ratingsRepository.GetRatingsAsync());

        public async Task InsertRatingAsync(CreateRatingDto createRatingDto, string authorId)
        {
            User user = userRepository.GetUser(u => u.Id == new Guid(authorId));

            IQueryable<Rating> userRatings = ratingsRepository.GetRatings(r => r.AuthorId == user.Id);

            // Check if User has already any Rating for given Question/Answer if yes update it.
            if(createRatingDto.QuestionId is not null && userRatings.Any(r => r.QuestionId == createRatingDto.QuestionId))
            {
                Rating rating = userRatings.FirstOrDefault(r => r.QuestionId == createRatingDto.QuestionId);

                if (rating.Value.Equals(createRatingDto.Value)) await DeleteRatingAsync(rating.Id); // If u choose the same Value it gets deleted.

                else await UpdateRatingAsync(rating, createRatingDto); // If u choose another Value updated it.
            }
            else if (createRatingDto.AnswerId is not null && userRatings.Any(r => r.AnswerId == createRatingDto.AnswerId))
            {
                Rating rating = userRatings.FirstOrDefault(r => r.AnswerId == createRatingDto.AnswerId);

                if (rating.Value.Equals(createRatingDto.Value)) await DeleteRatingAsync(rating.Id);

                else await UpdateRatingAsync(rating, createRatingDto);
            }
            else // If not create new Rating.
            {
                Rating rating = mapper.Map<Rating>(createRatingDto);

                await InsertRatingAsync(rating, user);
            }
        }

        private async Task InsertRatingAsync(Rating rating, User user)
        {
            rating.Author = user;

            await ratingsRepository.InsertRatingAsync(rating);
            await ratingsRepository.SaveAsync();
        }

        private async Task UpdateRatingAsync(Rating rating, CreateRatingDto createRatingDto)
        {
                rating.Value = createRatingDto.Value;

                await ratingsRepository.UpdateRatingAsync(rating);
                await ratingsRepository.SaveAsync();
        }

        private async Task DeleteRatingAsync(int id)
        {
            await ratingsRepository.DeleteRatingAsync(id);
            await ratingsRepository.SaveAsync();
        }
    }
}
