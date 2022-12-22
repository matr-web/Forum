using AutoMapper;
using Forum.Entities;
using Forum.WebAPI.Authorization;
using Forum.WebAPI.Dto_s;
using Forum.WebAPI.Pagination;
using Forum.WebAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace Forum.WebAPI.Services
{
    public interface IQuestionsService
    {
        Task<PagedResult<QuestionDto>> GetQuestionsAsync(Query query);
        Task<QuestionDto> GetQuestionByIdAsync(int id);
        Task<int> InsertQuestionAsync(CreateQuestionDto createQuestionDto);
        Task UpdateQuestionAsync(UpdateQuestionDto updateQuestionDto);
        Task DeleteQuestionAsync(int id);
    }

    public class QuestionsService : IQuestionsService
    {
        private readonly IQuestionsRepository questionsRepository;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly IUserService userService;
        private readonly IAuthorizationService authorizationService;

        public QuestionsService(IQuestionsRepository questionsRepository, IUserRepository userRepository, IMapper mapper, IUserService userService,
            IAuthorizationService authorizationService)
        {
            this.questionsRepository = questionsRepository;
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.userService = userService;
            this.authorizationService = authorizationService;
        }

        public async Task<PagedResult<QuestionDto>> GetQuestionsAsync(Query query)
        {
            var questionsDictionary = await questionsRepository.GetQuestionsAsync(query);

            var baseQuery = questionsDictionary[1];
            var queryResult = questionsDictionary[2];

            IEnumerable<QuestionDto> questionsDtos = mapper.Map<IEnumerable<QuestionDto>>(queryResult);

            PagedResult<QuestionDto> result = new PagedResult<QuestionDto>(questionsDtos.ToList(), baseQuery.Count(), query.PageSize, query.PageNumber);

            return result;
        }

        public async Task<QuestionDto> GetQuestionByIdAsync(int id) => 
            mapper.Map<QuestionDto>(await questionsRepository.GetQuestionByIdAsync(id));

        public async Task<int> InsertQuestionAsync(CreateQuestionDto createQuestionDto)
        {
            Question question = mapper.Map<Question>(createQuestionDto);

            if (question.Topic is null || question.Content is null)
                throw new Exception(StatusCodes.Status400BadRequest.ToString());

            User user = userRepository.GetUser(u => u.Id == userService.UserId);

            if (user is null) throw new Exception(StatusCodes.Status404NotFound.ToString());

            question.Author = user;

            await questionsRepository.InsertQuestionAsync(question);
            await questionsRepository.SaveAsync();

            return question.Id;
        }

        public async Task UpdateQuestionAsync(UpdateQuestionDto updateQuestionDto)
        {
            Question question = await questionsRepository.GetQuestionByIdAsync(updateQuestionDto.Id);

            if(question is null) throw new Exception(StatusCodes.Status404NotFound.ToString());

            var authorizationResult = authorizationService.AuthorizeAsync(userService.User, new Resource(question),
                new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded) throw new Exception(StatusCodes.Status403Forbidden.ToString());

            question.Topic = updateQuestionDto.Topic;
            question.Content = updateQuestionDto.Content;
            question.Date = DateTime.Now;

            await questionsRepository.UpdateQuestionAsync(question);
            await questionsRepository.SaveAsync();
        }

        public async Task DeleteQuestionAsync(int id)
        {
            Question question = await questionsRepository.GetQuestionByIdAsync(id);

            if (question is null) throw new Exception(StatusCodes.Status404NotFound.ToString());

            var authorizationResult = authorizationService.AuthorizeAsync(userService.User, new Resource(question),
               new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded) throw new Exception(StatusCodes.Status403Forbidden.ToString()); 

            await questionsRepository.DeleteQuestionAsync(id);
            await questionsRepository.SaveAsync();
        }
    }
}
