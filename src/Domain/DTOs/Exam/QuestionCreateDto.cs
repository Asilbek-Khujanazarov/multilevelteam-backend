using Multilevelteam.Platform.API.DTOs.Quesrtions;

namespace Multilevelteam.Platform.API.DTOs.Questions
{
    public class QuestionCreateDto
{
    public string Text { get; set; }
    public IFormFile? Image { get; set; }
    public List<AnswerCreateDto> Answers { get; set; }
        public string? ImageUrl { get; internal set; }
    }
}