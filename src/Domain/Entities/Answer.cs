namespace Multilevelteam.Platform.Domain.Entities
{
    public class Answer
    {
        public Guid Id { get; set; }
        public string ?Text { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImagePublicId { get; set; }
        public bool IsCorrect { get; set; }
        public string? CorrectDescription { get; set; }
        public Guid QuestionId { get; set; }
        public virtual Question Question { get; set; }
    }
}