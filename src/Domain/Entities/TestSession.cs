namespace Autotest.Platform.Domain.Entities
{
    public class TestSession
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public int QuestionCount { get; set; }
        public virtual ICollection<TestSessionQuestion> Questions { get; set; }
        public int Score { get; set; }
    }
}