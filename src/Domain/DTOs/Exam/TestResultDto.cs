using Autotest.Platform.API.DTOs.Questions;

namespace Autotest.Platform.Domain.DTOs.Exam;
public class TestResultDto
{
    public Guid SessionId { get; set; }
    public int TotalQuestions { get; set; }
    public int CorrectAnswers { get; set; }
    public string Grade { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime FinishedAt { get; set; }
    public List<QuestionDto> CorrectQuestions { get; set; }
    public List<QuestionDto> WrongQuestions { get; set; }
    public List<QuestionDto> UnansweredQuestions { get; set; }
}