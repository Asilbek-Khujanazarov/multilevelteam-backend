using Autotest.Platform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IQuestionRepository
{
    Task CreateAsync(Question question);
    Task<Question> GetByIdAsync(Guid id);
    Task<List<Question>> GetAllAsync();
    Task<(List<Question> Questions, int Total)> GetAllAsyncPage(int page, int pageSize);
    Task UpdateAsync(Question question);
    Task DeleteAsync(Question question);
}