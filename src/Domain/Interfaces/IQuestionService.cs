
using Autotest.Platform.API.DTOs.Questions;

public interface IQuestionService
{
    Task<QuestionDto> CreateAsync(QuestionCreateDto dto);
    Task<QuestionDto> GetByIdAsync(Guid id);
    Task<List<QuestionDto>> GetAllAsync();
    Task<bool> UpdateAsync(Guid id, QuestionCreateDto dto);
    Task<bool> DeleteAsync(Guid id);

}