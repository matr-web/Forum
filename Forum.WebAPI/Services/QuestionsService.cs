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
        Task<int> InsertQuestionAsync(CreateQuestionDto createQuestionDto);
        Task DeleteQuestionAsync(int id);
        Task UpdateQuestionAsync(UpdateQuestionDto updateQuestionDto);
    }

    public class QuestionsService : IQuestionsService
    {
        private readonly IQuestionsRepository questionsRepository;
        private readonly IMapper mapper;

        public QuestionsService(IQuestionsRepository questionsRepository, IMapper mapper)
        {
            this.questionsRepository = questionsRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<QuestionDto>> GetQuestionsAsync() => mapper.Map<IEnumerable<QuestionDto>>(await questionsRepository.GetQuestionsAsync());  

        public async Task<QuestionDto> GetQuestionByIdAsync(int id) => mapper.Map<QuestionDto>(await questionsRepository.GetQuestionByIDAsync(id));

        public async Task<int> InsertQuestionAsync(CreateQuestionDto createQuestionDto)
        {
            Question question = mapper.Map<Question>(createQuestionDto);

            await questionsRepository.InsertQuestionAsync(question);
            await questionsRepository.SaveAsync();

            return question.Id;
        }

        public async Task UpdateQuestionAsync(UpdateQuestionDto updateQuestionDto)
        {
            Question question = mapper.Map<Question>(updateQuestionDto);

            await questionsRepository.UpdateQuestionAsync(question);
            await questionsRepository.SaveAsync();
        }

        public async Task DeleteQuestionAsync(int id)
        {
            await questionsRepository.DeleteQuestionAsync(id);
            await questionsRepository.SaveAsync();
        }
    }
}
