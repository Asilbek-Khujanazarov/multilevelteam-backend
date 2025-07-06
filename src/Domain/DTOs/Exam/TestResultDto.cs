using Multilevelteam.Platform.API.DTOs.Questions;

namespace Multilevelteam.Platform.Domain.DTOs.Exam;
public class TestResultDto
{
    public Guid SessionId { get; set; }
    public int TotalQuestions { get; set; }
    public int CorrectAnswers { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime FinishedAt { get; set; }
    public string Grade { get; set; }
    public string? FailureReason { get; set; } // Added
    public List<QuestionDto> CorrectQuestions { get; set; }
    public List<QuestionDto> WrongQuestions { get; set; }
    public List<QuestionDto> UnansweredQuestions { get; set; }
    public int WrongAnswerCount { get; internal set; }
}