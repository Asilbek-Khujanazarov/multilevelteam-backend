namespace Autotest.Platform.Domain.Entities;

public class TestSession
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public int DurationMinutes { get; set; }
    public int QuestionCount { get; set; }
    public List<TestSessionQuestion> Questions { get; set; } = new();
    public int Score { get; set; }
    public bool IsFinished { get; set; }
}