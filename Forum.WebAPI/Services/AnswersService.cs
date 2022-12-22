using AutoMapper;
using Forum.Entities;
using Forum.WebAPI.Authorization;
using Forum.WebAPI.Dto_s;
using Forum.WebAPI.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace Forum.WebAPI.Services;

public interface IAnswersService
{
    Task<IEnumerable<AnswerDto>> GetAnswersAsync();
    Task<AnswerDto> GetAnswerByIdAsync(int id);
    Task<int> InsertAnswerAsync(int questionId, CreateAnswerDto createAnswerDto);
    Task UpdateAnswerAsync(UpdateAnswerDto updateAnswerDto);
    Task DeleteAnswerAsync(int id);
}

public class AnswersService : IAnswersService
{
    private readonly IAnswersRepository answersRepository;
    private readonly IUserRepository userRepository;
    private readonly IMapper mapper;
    private readonly IUserService userService;
    private readonly IAuthorizationService authorizationService;

    public AnswersService(IAnswersRepository answersRepository, IUserRepository userRepository, IMapper mapper, IUserService userService,
            IAuthorizationService authorizationService)
    {
        this.answersRepository = answersRepository;
        this.userRepository = userRepository;
        this.mapper = mapper;
        this.userService = userService;
        this.authorizationService = authorizationService;
    }

    public async Task<IEnumerable<AnswerDto>> GetAnswersAsync() => mapper.Map<IEnumerable<AnswerDto>>(await answersRepository.GetAnswersAsync());
    
    public async Task<AnswerDto> GetAnswerByIdAsync(int id) => mapper.Map<AnswerDto>(await answersRepository.GetAnswerByIdAsync(id));

    public async Task<int> InsertAnswerAsync(int questionId, CreateAnswerDto createAnswerDto)
    {
        Answer answer = mapper.Map<Answer>(createAnswerDto);

        if (answer.Content is null) throw new Exception(StatusCodes.Status400BadRequest.ToString());
        
        User user = userRepository.GetUser(u => u.Id == userService.UserId);

        if (user is null) throw new Exception(StatusCodes.Status401Unauthorized.ToString());

        answer.Author = user;
        answer.QuestionId = questionId;

        await answersRepository.InsertAnswerAsync(answer);
        await answersRepository.SaveAsync();

        return answer.Id;
    }

    public async Task UpdateAnswerAsync(UpdateAnswerDto updateAnswerDto)
    {
        Answer answer = await answersRepository.GetAnswerByIdAsync(updateAnswerDto.Id);

        if (answer is null) throw new Exception(StatusCodes.Status404NotFound.ToString());

        var authorizationResult = authorizationService.AuthorizeAsync(userService.User, new Resource(answer),
                new ResourceOperationRequirement(ResourceOperation.Update)).Result;

        if (!authorizationResult.Succeeded) throw new Exception(StatusCodes.Status403Forbidden.ToString()); 

            answer.Content = updateAnswerDto.Content;
            answer.Date = DateTime.Now;

            await answersRepository.UpdateAnswerAsync(answer);
            await answersRepository.SaveAsync();
    }

    public async Task DeleteAnswerAsync(int id)
    {
        Answer answer = await answersRepository.GetAnswerByIdAsync(id);

        if (answer is null) throw new Exception(StatusCodes.Status404NotFound.ToString());

        var authorizationResult = authorizationService.AuthorizeAsync(userService.User, new Resource(answer),
           new ResourceOperationRequirement(ResourceOperation.Update)).Result;

        if (!authorizationResult.Succeeded) throw new Exception(StatusCodes.Status403Forbidden.ToString());

            await answersRepository.DeleteAnswerAsync(id);
            await answersRepository.SaveAsync();
    }
}
