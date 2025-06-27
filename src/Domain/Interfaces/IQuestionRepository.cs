using Autotest.Platform.Domain.Entities;

public interface IQuestionRepository
{
     Task CreateAsync(Question question);
    Task<Question> GetByIdAsync(Guid id);
    Task<List<Question>> GetAllAsync();
    Task UpdateAsync(Question question);
    Task DeleteAsync(Question question);
}