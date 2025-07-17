using AutoMapper;
using Multilevelteam.Platform.Application.Dtos.lessonDtos;
using Multilevelteam.Platform.Application.Interfaces;
using Multilevelteam.Platform.Domain.Entities;


namespace Multilevelteam.Platform.Application.Services
{
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository _repo;
        private readonly IMapper _mapper;
        public LessonService(ILessonRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<LessonDto> CreateAsync(CreateLessonDto dto)
        {
            var lesson = _mapper.Map<Lesson>(dto);
            lesson.CreatedAt = DateTime.UtcNow;
            lesson.UpdatedAt = DateTime.UtcNow;
            var entity = await _repo.AddAsync(lesson);
            return _mapper.Map<LessonDto>(entity);
        }

        public async Task<List<LessonDto>> GetByCourseIdAsync(Guid courseId)
        {
            var list = await _repo.GetByCourseIdAsync(courseId);
            return _mapper.Map<List<LessonDto>>(list);
        }
    }
}