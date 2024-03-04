using VirtualClassRoom.Configurations;
using VirtualClassRoom.Extensions;
using VirtualClassRoom.Helpers;
using VirtualClassRoom.Interfaces;
using VirtualClassRoom.Models.CourseStudents;

namespace VirtualClassRoom.Services;

public class CourseStudentService : ICourseStudentsService
{
    private readonly CourseService courseService;
    private readonly StudentService studentService;
    private List<CourseStudentsModel> courseStudents;
    public CourseStudentService(CourseService courseService, StudentService studentService)
    {
        this.courseService = courseService;
        this.studentService = studentService;
    }

    public async ValueTask<CourseStudentsViewModel> CreateAsync(CourseStudentsCreationModel courseStudent)
    {
        await courseService.GetByIdAsync(courseStudent.CourseId);
        await studentService.GetByIdAsync(courseStudent.StudentId);

        courseStudents = await FileIO.ReadAsync<CourseStudentsModel>(Constantas.COURSE_STUDENT_PATH);
        var existCourseStudent = courseStudents.FirstOrDefault(t => t.CourseId == courseStudent.CourseId &&
                                                                    t.StudentId == courseStudent.StudentId);

        if (existCourseStudent != null && existCourseStudent.IsDeleted)
        {
            return await UpdateAsync(existCourseStudent.Id, courseStudent.MapTo<CourseStudentsUpdateModel>(), true);
        }

        if (existCourseStudent is not null)
            throw new Exception($"This Course is already exist with this Id = {existCourseStudent.Id}");

        var createdCourse = courseStudents.Create(courseStudent.MapTo<CourseStudentsModel>());
        await FileIO.WriteAsync(Constantas.COURSE_STUDENT_PATH, courseStudents);

        return createdCourse.MapTo<CourseStudentsViewModel>();
    }

    public async ValueTask<bool> DeleteAsync(long id)
    {
        courseStudents = await FileIO.ReadAsync<CourseStudentsModel>(Constantas.COURSE_STUDENT_PATH);
        var existCourseStudent = courseStudents.FirstOrDefault(c => c.Id == id && !c.IsDeleted)
            ?? throw new Exception($"This courseStudent is not found with Id = {id}");

        existCourseStudent.IsDeleted = true;
        existCourseStudent.DeletedAt = DateTime.UtcNow;
        await FileIO.WriteAsync(Constantas.COURSE_STUDENT_PATH, courseStudents);

        return true;
    }

    public async ValueTask<IEnumerable<CourseStudentsViewModel>> GetAllAsync()
    {
        courseStudents = await FileIO.ReadAsync<CourseStudentsModel>(Constantas.COURSE_STUDENT_PATH);

        return courseStudents.Where(t => !t.IsDeleted).MapTo<CourseStudentsViewModel>();
    }

    public async ValueTask<IEnumerable<CourseStudentsViewModel>> GetByCourseIdAsync(long courseId)
    {
        courseStudents = await FileIO.ReadAsync<CourseStudentsModel>(Constantas.COURSE_STUDENT_PATH);

        return courseStudents.Where(t => !t.IsDeleted).Where(t => t.CourseId == courseId).MapTo<CourseStudentsViewModel>();
    }

    public async ValueTask<CourseStudentsViewModel> GetByIdAsync(long id)
    {
        courseStudents = await FileIO.ReadAsync<CourseStudentsModel>(Constantas.COURSE_STUDENT_PATH);
        var existCourse = courseStudents.FirstOrDefault(c => c.Id == id && !c.IsDeleted)
            ?? throw new Exception($"This CourseStudent is not found with Id = {id}");

        return existCourse.MapTo<CourseStudentsViewModel>();
    }

    public async ValueTask<CourseStudentsViewModel> UpdateAsync(long id, CourseStudentsUpdateModel courseStudent, bool isCourseStudentDeleted = false)
    {
        await courseService.GetByIdAsync(courseStudent.CourseId);
        await studentService.GetByIdAsync(courseStudent.StudentId);

        courseStudents = await FileIO.ReadAsync<CourseStudentsModel>(Constantas.COURSE_STUDENT_PATH);
        var existCourse = new CourseStudentsModel();

        if (isCourseStudentDeleted)
        {
            existCourse = courseStudents.FirstOrDefault(c => c.Id == id);
            existCourse.IsDeleted = false;
        }
        else
        {
            existCourse = courseStudents.FirstOrDefault(c => c.Id == id && !c.IsDeleted)
                ?? throw new Exception($"This CourseStudent is not found with Id = {id}");
        }

        existCourse.UpdatedAt = DateTime.UtcNow;
        existCourse.CourseId = courseStudent.CourseId;
        existCourse.StudentId = courseStudent.StudentId;

        await FileIO.WriteAsync(Constantas.COURSE_STUDENT_PATH, courseStudents);

        return existCourse.MapTo<CourseStudentsViewModel>();
    }
}
