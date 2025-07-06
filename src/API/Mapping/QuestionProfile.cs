using AutoMapper;
using Multilevelteam.Platform.Domain.Entities;
using Multilevelteam.Platform.API.DTOs.Questions;
using Multilevelteam.Platform.API.DTOs.Quesrtions;
public class QuestionProfile : Profile
{
    public QuestionProfile()
    {
        CreateMap<Question, QuestionDto>();
        CreateMap<Answer, AnswerDto>();
        CreateMap<QuestionCreateDto, Question>();
        CreateMap<AnswerCreateDto, Answer>();
    }
}