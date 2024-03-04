using VirtualClassRoom.Configurations;
using VirtualClassRoom.Extensions;
using VirtualClassRoom.Helpers;
using VirtualClassRoom.Interfaces;
using VirtualClassRoom.Models.VirtualCourse;

namespace VirtualClassRoom.Services;

public class VirtualCourseService : IVirtualCourseService
{
    private List<VirtualCourseModel> lessons;
    private readonly CourseService courseService;
    private readonly CourseStudentService courseStudentService;
    public VirtualCourseService(CourseService courseService, CourseStudentService courseStudentService)
    {
        this.courseService = courseService;
        this.courseStudentService = courseStudentService;
    }
    public async ValueTask<VirtualCourseViewModel> CreateAsync(VirtualCourseCreationModel virtualCourse)
    {
        var existCourse = await courseService.GetByIdAsync(virtualCourse.CourceId)
            ?? throw new Exception($"This course is not found with this courseId {virtualCourse.CourceId}");

        lessons = await FileIO.ReadAsync<VirtualCourseModel>(Constantas.VIRTUAL_COURSES_PATH);
        var createdLesson = lessons.Create(virtualCourse.MapTo<VirtualCourseModel>());
        await FileIO.WriteAsync(Constantas.VIRTUAL_COURSES_PATH, lessons);

        return createdLesson.MapTo<VirtualCourseViewModel>();
    }

    public async ValueTask<IEnumerable<VirtualCourseViewModel>> GetAllAsync()
    {
        lessons = await FileIO.ReadAsync<VirtualCourseModel>(Constantas.VIRTUAL_COURSES_PATH);

        return lessons.Where(t => !t.IsDeleted).MapTo<VirtualCourseViewModel>();
    }

    public async ValueTask<VirtualCourseViewModel> GetByIdAsync(long id)
    {
        lessons = await FileIO.ReadAsync<VirtualCourseModel>(Constantas.VIRTUAL_COURSES_PATH);
        var existLesson = lessons.FirstOrDefault(c => c.Id == id && !c.IsDeleted)
            ?? throw new Exception($"This classroom is not found with Id = {id}");

        return existLesson.MapTo<VirtualCourseViewModel>();
    }

    public async ValueTask AttendedAsync(long virtualLessonId, long studentId)
    {
        lessons = await FileIO.ReadAsync<VirtualCourseModel>(Constantas.VIRTUAL_COURSES_PATH);
        var existLesson = lessons.FirstOrDefault(c => c.Id == virtualLessonId);
        var existStudentId = existLesson.StudentsId.FirstOrDefault(s => s == studentId);
        if (existStudentId is not 0)
            throw new Exception("This student is already attended");

        var existStudents = await courseStudentService.GetByCourseIdAsync(existLesson.CourceId);
        var existStudent = existStudents.FirstOrDefault(c => c.StudentId == studentId)
            ?? throw new Exception("This student is not found");
        existLesson.StudentsId.Add(studentId);
        existLesson.studentCount = existLesson.StudentsId.Count();
        await FileIO.WriteAsync(Constantas.VIRTUAL_COURSES_PATH, lessons);
    }
}
