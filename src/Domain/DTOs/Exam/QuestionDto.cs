namespace Autotest.Platform.API.DTOs.Questions
{
    public class QuestionDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public string? ImageUrl { get; set; }
        public List<AnswerDto> Answers { get; set; }
    }
}