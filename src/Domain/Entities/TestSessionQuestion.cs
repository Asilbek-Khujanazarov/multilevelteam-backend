namespace Autotest.Platform.Domain.Entities;

public class TestSessionQuestion
{
    public Guid Id { get; set; }
    public Guid TestSessionId { get; set; }
    public Guid QuestionId { get; set; }
    public Guid? SelectedAnswerId { get; set; }
    public bool? IsCorrect { get; set; }
    public TestSession TestSession { get; set; }
    public Question Question { get; set; }
}