using AutoMapper;
using Multilevelteam.Platform.Application.Dtos;
using Multilevelteam.Platform.Application.Dtos.CourseDtos;
using Multilevelteam.Platform.Application.Dtos.CourseUpdateDtos;
using Multilevelteam.Platform.Application.Interfaces;
using Multilevelteam.Platform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Multilevelteam.Platform.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _repo;
        private readonly IMapper _mapper;
        public CourseService(ICourseRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<CourseDto> CreateAsync(CreateCourseDto dto, Guid teacherId)
        {
            var course = _mapper.Map<Course>(dto);
            course.TeacherId = teacherId;
            course.CreatedAt = DateTime.UtcNow;
            course.UpdatedAt = DateTime.UtcNow;
            var entity = await _repo.AddAsync(course);
            return _mapper.Map<CourseDto>(entity);
        }

        public async Task<List<CourseDto>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return _mapper.Map<List<CourseDto>>(list);
        }

        public async Task<CourseDto> GetByIdAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return _mapper.Map<CourseDto>(entity);
        }
        public async Task<CourseDto> UpdateAsync(CourseUpdateDto dto)
        {
            var course = await _repo.GetByIdAsync(dto.Id);
            if (course == null) return null;

            // qiymatlarni yangilash
            course.Title = dto.Title;
            course.Description = dto.Description;
            course.Type = dto.Type;
            course.Price = dto.Price;
            course.UpdatedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(course);
            return _mapper.Map<CourseDto>(course);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repo.DeleteAsync(id);
        }
        public async Task<bool> UpdateTeacherAsync(Guid courseId, CourseTeacherIdDto dto)
        {
            // Teacher mavjudligini tekshirish
            var teacher = await _repo.GetByIdAsync(dto.TeacherId);
            if (teacher == null)
                throw new Exception("Bunday Teacher topilmadi!");

            return await _repo.UpdateTeacherAsync(courseId, dto.TeacherId);
        }

    }
}