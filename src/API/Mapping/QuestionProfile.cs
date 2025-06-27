using AutoMapper;
using Autotest.Platform.Domain.Entities;
using Autotest.Platform.API.DTOs.Questions;
public class QuestionProfile : Profile
{
    public QuestionProfile()
    {
        CreateMap<Question, QuestionDto>();
        CreateMap<Answer, AnswerDto>();
    }
}