using AutoMapper;
using Forum.Entities;
using Forum.WebAPI.Dto_s;
using Forum.WebAPI.Repositories;

namespace Forum.WebAPI.Services
{
    public interface IRatingsService
    {
        Task InsertRatingAsync(CreateRatingDto createRatingDto);
    }

    public class RatingsService : IRatingsService
    {
        private readonly IRatingsRepository ratingsRepository;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly IUserService userService;

        public RatingsService(IRatingsRepository ratingsRepository, IUserRepository userRepository, IMapper mapper, IUserService userService)
        {
            this.ratingsRepository = ratingsRepository;
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.userService = userService;
        }

        public async Task InsertRatingAsync(CreateRatingDto createRatingDto)
        {
            User user = userRepository.GetUser(u => u.Id == userService.UserId);

            IQueryable<Rating> userRatings = ratingsRepository.GetRatings(r => r.AuthorId == user.Id);

            // Check if User has already any Rating for given Question if yes update it.
            if(createRatingDto.QuestionId is not null && userRatings.Any(r => r.QuestionId == createRatingDto.QuestionId))
            {
                Rating rating = userRatings.FirstOrDefault(r => r.QuestionId == createRatingDto.QuestionId); // Get the Rating.

                if (rating.Value.Equals(createRatingDto.Value)) await DeleteRatingAsync(rating.Id); // If u choose the same Value it gets deleted.

                else await UpdateRatingAsync(rating, createRatingDto.Value); // If u choose another Value it gets updated.
            } 
            else if (createRatingDto.AnswerId is not null && userRatings.Any(r => r.AnswerId == createRatingDto.AnswerId)) // Same for Answer.
            {
                Rating rating = userRatings.FirstOrDefault(r => r.AnswerId == createRatingDto.AnswerId);

                if (rating.Value.Equals(createRatingDto.Value)) await DeleteRatingAsync(rating.Id);

                else await UpdateRatingAsync(rating, createRatingDto.Value);
            }
            else // If User doesn't have any Ratings for given Question/Answer, create new Rating.
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

        private async Task UpdateRatingAsync(Rating rating, int value)
        {
            rating.Value = value;

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
