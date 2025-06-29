namespace Autotest.Platform.API.DTOs.Quesrtions
{
    public class AnswerCreateDto
    {
        public string Text { get; set; }
        public IFormFile? Image { get; set; }
        public bool IsCorrect { get; set; }
        public string? CorrectDescription { get; set; }
        public string? ImageUrl { get; internal set; }
    }
}