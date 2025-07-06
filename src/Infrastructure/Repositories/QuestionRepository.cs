using Multilevelteam.Platform.Domain.Entities;
using Multilevelteam.Platform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class QuestionRepository : IQuestionRepository
{
    private readonly AppDbContext _context;

    public QuestionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Question question)
    {
        await _context.Questions.AddAsync(question);
        await _context.SaveChangesAsync();
    }

    public async Task<Question> GetByIdAsync(Guid id)
    {
        return await _context.Questions
            .Include(q => q.Answers)
            .FirstOrDefaultAsync(q => q.Id == id);
    }

    public async Task<List<Question>> GetAllAsync()
    {
        return await _context.Questions
            .Include(q => q.Answers)
            .ToListAsync();
    }

    public async Task<(List<Question> Questions, int Total)> GetAllAsyncPage(int page, int pageSize)
    {
        var query = _context.Questions
            .Include(q => q.Answers)
            .AsQueryable();

        var total = await query.CountAsync();
        var questions = await query
            .OrderBy(q => q.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (questions, total);
    }

    public async Task UpdateAsync(Question question)
    {
        var existingQuestion = await _context.Questions
            .Include(q => q.Answers)
            .FirstOrDefaultAsync(q => q.Id == question.Id);

        if (existingQuestion == null)
        {
            throw new Exception("Savol topilmadi");
        }

        // Savol ma'lumotlarini yangilash
        _context.Entry(existingQuestion).CurrentValues.SetValues(question);

        // Eski javoblarni o‘chirish
        _context.Answers.RemoveRange(existingQuestion.Answers);

        // Yangi javoblarni qo‘shish
        foreach (var answer in question.Answers)
        {
            answer.QuestionId = existingQuestion.Id;
            _context.Answers.Add(answer);
        }

        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(Question question)
    {
        _context.Questions.Remove(question);
        await _context.SaveChangesAsync();
    }
}