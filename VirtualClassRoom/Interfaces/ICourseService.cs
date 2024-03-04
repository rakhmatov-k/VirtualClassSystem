using VirtualClassRoom.Models.Course;

namespace VirtualClassRoom.Interfaces;

public interface ICourseService
{
    ValueTask<CourseViewModel> CreateAsync(CourseCreationModel course);
    ValueTask<CourseViewModel> UpdateAsync(long id, CourseUpdateModel course, bool isCourseDeleted = false);
    ValueTask<bool> DeleteAsync(long id);
    ValueTask<CourseViewModel> GetByIdAsync(long id);
    ValueTask<IEnumerable<CourseViewModel>> GetAllAsync();
}
