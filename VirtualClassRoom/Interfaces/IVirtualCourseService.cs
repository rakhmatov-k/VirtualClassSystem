using VirtualClassRoom.Models.VirtualCourse;

namespace VirtualClassRoom.Interfaces;

public interface IVirtualCourseService
{
    ValueTask<VirtualCourseViewModel> CreateAsync(VirtualCourseCreationModel virtualCourse);
    ValueTask<VirtualCourseViewModel> GetByIdAsync(long id);
    ValueTask<IEnumerable<VirtualCourseViewModel>> GetAllAsync();
    ValueTask AttendedAsync(long courseId , long studentId);
}
