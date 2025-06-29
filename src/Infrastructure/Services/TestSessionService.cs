using Autotest.Platform.Domain.DTOs.Exam;
using Autotest.Platform.Domain.Entities;
using Autotest.Platform.Domain.Interfaces;
using AutoMapper;
using Autotest.Platform.API.DTOs.Questions;

public class TestSessionService : ITestSessionService
{
    private readonly ITestSessionRepository _sessionRepo;
    private readonly IQuestionRepository _questionRepo;
    private readonly IMapper _mapper;

    public TestSessionService(
        ITestSessionRepository sessionRepo,
        IQuestionRepository questionRepo,
        IMapper mapper)
    {
        _sessionRepo = sessionRepo;
        _questionRepo = questionRepo;
        _mapper = mapper;
    }

    public async Task<StartTestResponseDto> StartTestAsync(Guid userId)
    {
        const int questionCount = 20;
        const int durationMinutes = 30;

        var prevSessions = await _sessionRepo.GetLastSessionsAsync(userId, 5);
        var prevQuestionIds = prevSessions
            .SelectMany(s => s.Questions.Select(q => q.QuestionId))
            .Distinct()
            .ToHashSet();

        var allQuestions = await _questionRepo.GetAllAsync();

        var random = new Random();
        var weightedQuestions = allQuestions
            .OrderBy(q => prevQuestionIds.Contains(q.Id) ? random.Next(100, 200) : random.Next(0, 99))
            .Take(questionCount)
            .ToList();

        var session = new TestSession
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            StartedAt = DateTime.UtcNow,
            FinishedAt = null,
            DurationMinutes = durationMinutes,
            QuestionCount = questionCount,
            IsFinished = false,
            Questions = weightedQuestions.Select(q => new TestSessionQuestion
            {
                Id = Guid.NewGuid(),
                QuestionId = q.Id
            }).ToList()
        };

        await _sessionRepo.CreateAsync(session);

        var questionDtos = _mapper.Map<List<QuestionDto>>(weightedQuestions);
        return new StartTestResponseDto
        {
            SessionId = session.Id,
            Questions = questionDtos,
            DurationMinutes = durationMinutes,
            Deadline = session.StartedAt.AddMinutes(durationMinutes)
        };
    }

    public async Task SubmitAnswerAsync(Guid userId, Guid sessionId, Guid questionId, Guid answerId)
    {
        var session = await _sessionRepo.GetByIdAsync(sessionId);
        if (session == null || session.UserId != userId || session.IsFinished)
            throw new Exception("Sessiya topilmadi yoki yakunlangan");

        var endTime = session.StartedAt.AddMinutes(session.DurationMinutes);
        if (DateTime.UtcNow > endTime)
        {
            await FinishTestAsync(userId, sessionId, force: true);
            throw new Exception("Vaqt tugagan, test avtomatik tugatildi");
        }

        var sessionQuestion = session.Questions.FirstOrDefault(x => x.QuestionId == questionId);
        if (sessionQuestion == null)
            throw new Exception("Savol topilmadi");

        sessionQuestion.SelectedAnswerId = answerId;
        var correctAnswer = sessionQuestion.Question.Answers.FirstOrDefault(a => a.IsCorrect);
        sessionQuestion.IsCorrect = correctAnswer != null && correctAnswer.Id == answerId;

        await _sessionRepo.UpdateAsync(session);
    }

    public async Task<TestResultDto> FinishTestAsync(Guid userId, Guid sessionId, bool force = false)
    {
        var session = await _sessionRepo.GetByIdAsync(sessionId);
        if (session == null || session.UserId != userId || session.IsFinished)
            throw new Exception("Sessiya topilmadi yoki yakunlangan");

        var endTime = session.StartedAt.AddMinutes(session.DurationMinutes);
        if (!force && DateTime.UtcNow > endTime)
            force = true;

        session.FinishedAt = force ? endTime : DateTime.UtcNow;
        session.Score = session.Questions.Count(q => q.IsCorrect == true);
        session.IsFinished = true;

        // Daraja
        string grade;
        double percentage = (double)session.Score / session.QuestionCount * 100;
        if (percentage >= 90) grade = "Excellent";
        else if (percentage >= 75) grade = "Good";
        else if (percentage >= 50) grade = "Average";
        else grade = "Bad";

        await _sessionRepo.UpdateAsync(session);

        var correct = session.Questions.Where(q => q.IsCorrect == true).ToList();
        var wrong = session.Questions.Where(q => q.IsCorrect == false).ToList();
        var unanswered = session.Questions.Where(q => q.SelectedAnswerId == null).ToList();

        return new TestResultDto
        {
            SessionId = session.Id,
            TotalQuestions = session.QuestionCount,
            CorrectAnswers = session.Score,
            StartedAt = session.StartedAt,
            FinishedAt = session.FinishedAt.Value,
            Grade = grade,
            CorrectQuestions = _mapper.Map<List<QuestionDto>>(correct.Select(q => q.Question)),
            WrongQuestions = _mapper.Map<List<QuestionDto>>(wrong.Select(q => q.Question)),
            UnansweredQuestions = _mapper.Map<List<QuestionDto>>(unanswered.Select(q => q.Question))
        };
    }

    // Cron yoki background servisda ishlatish uchun: vaqt tugagan testlarni avtomatik yakunlash
    public async Task AutoFinishExpiredTestsAsync()
    {
        // Bu metodni background service yoki cron uchun yozasiz,
        // IsFinished == false va (StartedAt + DurationMinutes) < DateTime.UtcNow bo'lgan testlarni tugatadi
        // (implementatsiyani loyihangizda to'ldiring)
    }
}