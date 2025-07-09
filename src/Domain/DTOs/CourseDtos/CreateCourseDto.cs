using Multilevelteam.Platform.Domain.Enums;

public class CreateCourseDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public CourseType Type { get; set; }
    public decimal Price { get; set; }
    public Guid TeacherId { get; set; }
    // public string? CoverImageUrl { get; set; }
}