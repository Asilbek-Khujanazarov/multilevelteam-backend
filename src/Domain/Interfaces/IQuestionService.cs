using Multilevelteam.Platform.API.DTOs.Questions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IQuestionService
{
    Task<QuestionDto> CreateAsync(QuestionCreateDto dto);
    Task<QuestionDto> GetByIdAsync(Guid id);
    Task<List<QuestionDto>> GetAllAsync();
    Task<(List<QuestionDto> Questions, int Total)> GetAllAsyncPage(int page, int pageSize);
    Task<bool> UpdateAsync(Guid id, QuestionCreateDto dto);
    Task<bool> DeleteAsync(Guid id);
}