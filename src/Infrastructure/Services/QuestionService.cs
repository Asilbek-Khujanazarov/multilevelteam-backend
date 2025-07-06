using AutoMapper;
using Multilevelteam.Platform.API.DTOs.Questions;
using Multilevelteam.Platform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class QuestionService : IQuestionService
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IMapper _mapper;
    private readonly CloudinaryService _cloudinaryService;

    public QuestionService(IQuestionRepository questionRepository, IMapper mapper, CloudinaryService cloudinaryService)
    {
        _questionRepository = questionRepository;
        _mapper = mapper;
        _cloudinaryService = cloudinaryService;
    }

    public async Task<QuestionDto> CreateAsync(QuestionCreateDto dto)
    {
        string? imageUrl = null;
        string? imagePublicId = null;
        if (dto.Image != null)
        {
            var imgResult = await _cloudinaryService.UploadImageAsync(dto.Image, "questions");
            imageUrl = imgResult.Url;
            imagePublicId = imgResult.PublicId;
        }

        var question = new Question
        {
            Id = Guid.NewGuid(),
            Text = dto.Text,
            ImageUrl = imageUrl,
            ImagePublicId = imagePublicId,
            Answers = new List<Answer>()
        };

        if (dto.Answers != null && dto.Answers.Count > 0)
        {
            foreach (var a in dto.Answers)
            {
                string? answerImageUrl = null;
                string? answerImagePublicId = null;
                if (a.Image != null)
                {
                    var ansResult = await _cloudinaryService.UploadImageAsync(a.Image, "answers");
                    answerImageUrl = ansResult.Url;
                    answerImagePublicId = ansResult.PublicId;
                }

                question.Answers.Add(new Answer
                {
                    Id = Guid.NewGuid(),
                    Text = a.Text,
                    ImageUrl = answerImageUrl,
                    ImagePublicId = answerImagePublicId,
                    IsCorrect = a.IsCorrect,
                    CorrectDescription = a.CorrectDescription, // Tuzatildi
                    QuestionId = question.Id
                });
            }
        }

        await _questionRepository.CreateAsync(question);
        return _mapper.Map<QuestionDto>(question);
    }

    public async Task<QuestionDto> GetByIdAsync(Guid id)
    {
        var question = await _questionRepository.GetByIdAsync(id);
        return _mapper.Map<QuestionDto>(question);
    }

    public async Task<List<QuestionDto>> GetAllAsync()
    {
        var questions = await _questionRepository.GetAllAsync();
        return _mapper.Map<List<QuestionDto>>(questions);
    }

    public async Task<(List<QuestionDto> Questions, int Total)> GetAllAsyncPage(int page, int pageSize)
    {
        var (questions, total) = await _questionRepository.GetAllAsyncPage(page, pageSize);
        var questionDtos = _mapper.Map<List<QuestionDto>>(questions);
        return (questionDtos, total);
    }

    public async Task<bool> UpdateAsync(Guid id, QuestionCreateDto dto)
    {
        var question = await _questionRepository.GetByIdAsync(id);
        if (question == null) return false;

        question.Text = dto.Text;

        // Savol rasmini yangilash
        if (dto.Image != null)
        {
            if (!string.IsNullOrEmpty(question.ImagePublicId))
            {
                await _cloudinaryService.DeleteImageAsync(question.ImagePublicId);
            }
            var imgResult = await _cloudinaryService.UploadImageAsync(dto.Image, "questions");
            question.ImageUrl = imgResult.Url;
            question.ImagePublicId = imgResult.PublicId;
        }
        else if (dto.Image == null && !string.IsNullOrEmpty(dto.ImageUrl))
        {
            question.ImageUrl = dto.ImageUrl;
            question.ImagePublicId = null; // Agar ImageUrl saqlansa, PublicId o‘chiriladi
        }
        else
        {
            // Agar rasm yo‘q bo‘lsa, eski rasmni o‘chirish
            if (!string.IsNullOrEmpty(question.ImagePublicId))
            {
                await _cloudinaryService.DeleteImageAsync(question.ImagePublicId);
            }
            question.ImageUrl = null;
            question.ImagePublicId = null;
        }

        // Eski javoblarni o‘chirish
        var existingAnswers = question.Answers.ToList();
        foreach (var oldAnswer in existingAnswers)
        {
            if (!string.IsNullOrEmpty(oldAnswer.ImagePublicId))
            {
                await _cloudinaryService.DeleteImageAsync(oldAnswer.ImagePublicId);
            }
        }
        question.Answers.Clear();

        // Yangi javoblarni qo‘shish
        if (dto.Answers != null && dto.Answers.Count > 0)
        {
            foreach (var a in dto.Answers)
            {
                string? answerImageUrl = a.ImageUrl;
                string? answerImagePublicId = null;
                if (a.Image != null)
                {
                    var ansResult = await _cloudinaryService.UploadImageAsync(a.Image, "answers");
                    answerImageUrl = ansResult.Url;
                    answerImagePublicId = ansResult.PublicId;
                }

                question.Answers.Add(new Answer
                {
                    Id = Guid.NewGuid(),
                    Text = a.Text,
                    ImageUrl = answerImageUrl,
                    ImagePublicId = answerImagePublicId,
                    IsCorrect = a.IsCorrect,
                    CorrectDescription = a.CorrectDescription,
                    QuestionId = id
                });
            }
        }

        await _questionRepository.UpdateAsync(question);
        return true;
    }
    public async Task<bool> DeleteAsync(Guid id)
    {
        var question = await _questionRepository.GetByIdAsync(id);
        if (question == null) return false;

        if (!string.IsNullOrEmpty(question.ImagePublicId))
            await _cloudinaryService.DeleteImageAsync(question.ImagePublicId);

        foreach (var answer in question.Answers)
        {
            if (!string.IsNullOrEmpty(answer.ImagePublicId))
                await _cloudinaryService.DeleteImageAsync(answer.ImagePublicId);
        }

        await _questionRepository.DeleteAsync(question);
        return true;
    }
}