using AutoMapper;
using Autotest.Platform.Domain.Entities;
using Autotest.Platform.API.DTOs.Questions;
public class ExamProfile : Profile
{
    public ExamProfile()
    {
        CreateMap<Question, QuestionDto>();
        CreateMap<Answer, AnswerDto>();
    }
}