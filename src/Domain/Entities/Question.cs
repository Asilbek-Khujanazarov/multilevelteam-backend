namespace Autotest.Platform.Domain.Entities
{
    public class Question
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImagePublicId { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
    }
}