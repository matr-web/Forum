using Forum.Entities;
using Forum.WebAPI.Repositories;

namespace Forum.WebAPI.Services
{
    public interface IQuestionsService
    {
        Task<IEnumerable<Question>> GetQuestionsAsync();
        Task<Question> GetQuestionByIdAsync(int id);
        Task InsertQuestionAsync(Question question);
        Task DeleteQuestionAsync(int id);
        Task UpdateQuestionAsync(Question question);
    }

    public class QuestionsService : IQuestionsService
    {
        private readonly IQuestionsRepository questionsRepository;

        public QuestionsService(IQuestionsRepository questionsRepository)
        {
            this.questionsRepository = questionsRepository;
        }

        public async Task<IEnumerable<Question>> GetQuestionsAsync() => await questionsRepository.GetQuestionsAsync();

        public async Task<Question> GetQuestionByIdAsync(int id) => await questionsRepository.GetQuestionByIDAsync(id);

        public async Task InsertQuestionAsync(Question question)
        {
            question.Date = DateTime.Now;

            await questionsRepository.InsertQuestionAsync(question);
            await questionsRepository.SaveAsync();
        }

        public async Task UpdateQuestionAsync(Question question)
        {
            question.Date = DateTime.Now;

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
