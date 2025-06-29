using Autotest.Platform.API.DTOs.Quesrtions;

namespace Autotest.Platform.API.DTOs.Questions
{
    public class QuestionCreateDto
{
    public string Text { get; set; }
    public IFormFile? Image { get; set; }
    public List<AnswerCreateDto> Answers { get; set; }
        public string? ImageUrl { get; internal set; }
    }
}