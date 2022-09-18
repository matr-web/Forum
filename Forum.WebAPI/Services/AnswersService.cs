using AutoMapper;
using Forum.Entities;
using Forum.WebAPI.Dto_s;
using Forum.WebAPI.Repositories;

namespace Forum.WebAPI.Services;

public interface IAnswersService
{
    Task<IEnumerable<AnswerDto>> GetAnswersAsync();
    Task<AnswerDto> GetAnswerByIdAsync(int id);
    Task<int> InsertAnswerAsync(int questionId, CreateAnswerDto createAnswerDto, string authorId);
    Task<bool> DeleteAnswerAsync(int id, string authorId);
    Task<bool> UpdateAnswerAsync(UpdateAnswerDto updateAnswerDto, string authorId);
}

public class AnswersService : IAnswersService
{
    private readonly IAnswersRepository answersRepository;
    private readonly IUserRepository userRepository;
    private readonly IMapper mapper;

    public AnswersService(IAnswersRepository answerRepository, IUserRepository userRepository, IMapper mapper)
    {
        this.answersRepository = answerRepository;
        this.userRepository = userRepository;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<AnswerDto>> GetAnswersAsync() => mapper.Map<IEnumerable<AnswerDto>>(await answersRepository.GetAnswersAsync());
    
    public async Task<AnswerDto> GetAnswerByIdAsync(int id) => mapper.Map<AnswerDto>(await answersRepository.GetAnswerByIdAsync(id));

    public async Task<int> InsertAnswerAsync(int questionId, CreateAnswerDto createAnswerDto, string authorId)
    {
        Answer answer = mapper.Map<Answer>(createAnswerDto);

        User user = userRepository.GetUser(u => u.Id == new Guid(authorId));

        answer.Author = user;
        answer.QuestionId = questionId;

        await answersRepository.InsertAnswerAsync(answer);
        await answersRepository.SaveAsync();

        return answer.Id;
    }

    public async Task<bool> UpdateAnswerAsync(UpdateAnswerDto updateAnswerDto, string authorId)
    {
        Answer answer = await answersRepository.GetAnswerByIdAsync(updateAnswerDto.Id);

        User user = userRepository.GetUser(u => u.Id == new Guid(authorId));

        if(answer.AuthorId.ToString().Equals(authorId))
        {
            answer.Content = updateAnswerDto.Content;
            answer.Date = DateTime.Now;

            await answersRepository.UpdateAnswerAsync(answer);
            await answersRepository.SaveAsync();

            return true;
        }

        return false;
    }

    public async Task<bool> DeleteAnswerAsync(int id, string authorId)
    {
        Answer answer = await answersRepository.GetAnswerByIdAsync(id);

        User user = userRepository.GetUser(u => u.Id == new Guid(authorId));

        if (answer.AuthorId.ToString().Equals(authorId) || user.RoleId == 1)
        {
            await answersRepository.DeleteAnswerAsync(id);
            await answersRepository.SaveAsync();

            return true;
        }

        return false;
    }
}
