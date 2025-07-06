using AutoMapper;
using Multilevelteam.Platform.Domain.Entities;
using Multilevelteam.Platform.API.DTOs.Questions;
public class ExamProfile : Profile
{
    public ExamProfile()
    {
        CreateMap<Question, QuestionDto>();
        CreateMap<Answer, AnswerDto>();
    }
}