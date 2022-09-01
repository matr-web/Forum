using AutoMapper;
using Forum.Entities;
using Forum.WebAPI.Dto_s;
using Forum.WebAPI.Repositories;

namespace Forum.WebAPI.Services;

public interface IAnswersService
{
    Task<IEnumerable<AnswerDto>> GetAnswersAsync();
    Task<AnswerDto> GetAnswerByIdAsync(int id);
    Task<int> InsertAnswerAsync(CreateAnswerDto createAnswerDto);
    Task DeleteAnswerAsync(int id);
    Task UpdateAnswerAsync(UpdateAnswerDto updateAnswerDto);
}

public class AnswersService : IAnswersService
{
    private readonly IAnswersRepository answersRepository;
    private readonly IMapper mapper;

    public AnswersService(IAnswersRepository answerRepository, IMapper mapper)
    {
        this.answersRepository = answerRepository;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<AnswerDto>> GetAnswersAsync() => mapper.Map<IEnumerable<AnswerDto>>(await answersRepository.GetAnswersAsync());
    
    public async Task<AnswerDto> GetAnswerByIdAsync(int id) => mapper.Map<AnswerDto>(await answersRepository.GetAnswerByIDAsync(id));

    public async Task<int> InsertAnswerAsync(CreateAnswerDto createAnswerDto)
    {
        Answer answer = mapper.Map<Answer>(createAnswerDto);

        await answersRepository.InsertAnswerAsync(answer);
        await answersRepository.SaveAsync();

        return answer.Id;
    }

    public async Task UpdateAnswerAsync(UpdateAnswerDto updateAnswerDto)
    {
        Answer answer = mapper.Map<Answer>(updateAnswerDto);

        await answersRepository.UpdateAnswerAsync(answer);
        await answersRepository.SaveAsync();
    }

    public async Task DeleteAnswerAsync(int id)
    {
        await answersRepository.DeleteAnswerAsync(id);
        await answersRepository.SaveAsync();
    }
}
