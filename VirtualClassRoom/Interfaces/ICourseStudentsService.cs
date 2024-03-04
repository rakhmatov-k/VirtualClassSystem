using VirtualClassRoom.Models.CourseStudents;

namespace VirtualClassRoom.Interfaces;

public interface ICourseStudentsService
{
    ValueTask<CourseStudentsViewModel> CreateAsync(CourseStudentsCreationModel courseStudent);
    ValueTask<CourseStudentsViewModel> UpdateAsync(long id, CourseStudentsUpdateModel courseStudent, bool isCourseStudentDeleted = false);
    ValueTask<bool> DeleteAsync(long id);
    ValueTask<CourseStudentsViewModel> GetByIdAsync(long id);
    ValueTask<IEnumerable<CourseStudentsViewModel>> GetAllAsync();
    ValueTask<IEnumerable<CourseStudentsViewModel>> GetByCourseIdAsync(long courseId);
}
