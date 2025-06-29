using AutoMapper;
using Autotest.Platform.Domain.Entities;
using Autotest.Platform.API.DTOs.Questions;
using Autotest.Platform.API.DTOs.Quesrtions;
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