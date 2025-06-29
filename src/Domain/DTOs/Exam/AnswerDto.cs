namespace Autotest.Platform.API.DTOs.Questions
{
    public class AnswerDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsCorrect { get; set; }
        public string? CorrectDescription { get; set; }
    }
}