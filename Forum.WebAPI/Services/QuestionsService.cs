using AutoMapper;
using Forum.Entities;
using Forum.WebAPI.Dto_s;
using Forum.WebAPI.Repositories;

namespace Forum.WebAPI.Services
{
    public interface IQuestionsService
    {
        Task<IEnumerable<QuestionDto>> GetQuestionsAsync();
        Task<QuestionDto> GetQuestionByIdAsync(int id);
        Task<int> InsertQuestionAsync(CreateQuestionDto createQuestionDto, string authorId);
        Task<bool> UpdateQuestionAsync(UpdateQuestionDto updateQuestionDto, string authorId);
        Task<bool> DeleteQuestionAsync(int id, string authorId);
    }

    public class QuestionsService : IQuestionsService
    {
        private readonly IQuestionsRepository questionsRepository;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public QuestionsService(IQuestionsRepository questionsRepository, IUserRepository userRepository, IMapper mapper)
        {
            this.questionsRepository = questionsRepository;
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<QuestionDto>> GetQuestionsAsync() => mapper.Map<IEnumerable<QuestionDto>>(await questionsRepository.GetQuestionsAsync());  

        public async Task<QuestionDto> GetQuestionByIdAsync(int id) => mapper.Map<QuestionDto>(await questionsRepository.GetQuestionByIdAsync(id));

        public async Task<int> InsertQuestionAsync(CreateQuestionDto createQuestionDto, string authorId)
        {
            Question question = mapper.Map<Question>(createQuestionDto);

            User user = userRepository.GetUser(u => u.Id == new Guid(authorId));

            question.Author = user;

            await questionsRepository.InsertQuestionAsync(question, authorId);
            await questionsRepository.SaveAsync();

            return question.Id;
        }

        public async Task<bool> UpdateQuestionAsync(UpdateQuestionDto updateQuestionDto, string authorId)
        {
            Question question = await questionsRepository.GetQuestionByIdAsync(updateQuestionDto.Id);
          
            User user = userRepository.GetUser(u => u.Id == new Guid(authorId));

            if (question.AuthorId.ToString().Equals(authorId))
            {
                question.Topic = updateQuestionDto.Topic;
                question.Content = updateQuestionDto.Content;
                question.Date = DateTime.Now;

                await questionsRepository.UpdateQuestionAsync(question);
                await questionsRepository.SaveAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> DeleteQuestionAsync(int id, string userId)
        {
            Question question = await questionsRepository.GetQuestionByIdAsync(id);

            User user = userRepository.GetUser(u => u.Id == new Guid(userId));

            if (question.AuthorId.ToString().Equals(userId) || user.RoleId == 1)
            {
                await questionsRepository.DeleteQuestionAsync(id);
                await questionsRepository.SaveAsync();

                return true;
            }

            return false;
        }
    }
}
