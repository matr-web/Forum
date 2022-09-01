using AutoMapper;
using Forum.Entities;
using Forum.WebAPI.Dto_s;

namespace Forum.WebAPI;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Answer, AnswerDto>()
            .ForMember(aDto => aDto.AuthorFullName, m => m.MapFrom(a => a.Author.FullName));

        CreateMap<Question, QuestionDto>()
           .ForMember(qDto => qDto.AuthorFullName, m => m.MapFrom(q => q.Author.FullName));

        CreateMap<Rating, RatingDto>();

        CreateMap<CreateAnswerDto, Answer>();
        CreateMap<UpdateAnswerDto, Answer>();

        CreateMap<CreateQuestionDto, Question>();
        CreateMap<UpdateQuestionDto, Question>();

        CreateMap<CreateRatingDto, Rating>();
        CreateMap<UpdateRatingDto, Rating>();
    }
}
