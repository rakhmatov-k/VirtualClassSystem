using VirtualClassRoom.Configurations;
using VirtualClassRoom.Extensions;
using VirtualClassRoom.Helpers;
using VirtualClassRoom.Interfaces;
using VirtualClassRoom.Models.Course;

namespace VirtualClassRoom.Services;

public class CourseService : ICourseService
{
    private List<CourseModel> courses;
    private readonly TeacherService teacherService;
    public CourseService(TeacherService teacherService)
    {
        this.teacherService = teacherService;
    }
    public async ValueTask<CourseViewModel> CreateAsync(CourseCreationModel course)
    {
        await teacherService.GetByIdAsync(course.TeacherId);

        courses = await FileIO.ReadAsync<CourseModel>(Constantas.COURSE_PATH);
        var existCourse = courses.FirstOrDefault(t => t.CourseName.ToLower() == course.CourseName.ToLower() &&
                                                      t.Description.ToLower() == course.CourseName.ToLower());

        if (existCourse != null && existCourse.IsDeleted)
        {
            return await UpdateAsync(existCourse.Id, course.MapTo<CourseUpdateModel>(), true);
        }

        if (existCourse is not null)
            throw new Exception($"This Course is already exist with this courseName = {course.CourseName}");

        var createdCourse = courses.Create(course.MapTo<CourseModel>());
        await FileIO.WriteAsync(Constantas.COURSE_PATH, courses);

        return createdCourse.MapTo<CourseViewModel>();
    }

    public async ValueTask<bool> DeleteAsync(long id)
    {
        courses = await FileIO.ReadAsync<CourseModel>(Constantas.COURSE_PATH);
        var existCourse = courses.FirstOrDefault(c => c.Id == id && !c.IsDeleted)
            ?? throw new Exception($"This Course is not found with Id = {id}");

        existCourse.IsDeleted = true;
        existCourse.DeletedAt = DateTime.UtcNow;
        await FileIO.WriteAsync(Constantas.COURSE_PATH, courses);

        return true;
    }

    public async ValueTask<IEnumerable<CourseViewModel>> GetAllAsync()
    {
        courses = await FileIO.ReadAsync<CourseModel>(Constantas.COURSE_PATH);

        return courses.Where(t => !t.IsDeleted).MapTo<CourseViewModel>();
    }

    public async ValueTask<CourseViewModel> GetByIdAsync(long id)
    {
        courses = await FileIO.ReadAsync<CourseModel>(Constantas.COURSE_PATH);
        var existCourse = courses.FirstOrDefault(c => c.Id == id && !c.IsDeleted)
            ?? throw new Exception($"This Course is not found with Id = {id}");

        return existCourse.MapTo<CourseViewModel>();
    }

    public async ValueTask<CourseViewModel> UpdateAsync(long id, CourseUpdateModel course, bool isCourseDeleted = false)
    {
        await teacherService.GetByIdAsync(course.TeacherId);

        courses = await FileIO.ReadAsync<CourseModel>(Constantas.COURSE_PATH);
        var existCourse = new CourseModel();

        if (isCourseDeleted)
        {
            existCourse = courses.FirstOrDefault(c => c.Id == id);
            existCourse.IsDeleted = false;
        }
        else
        {
            existCourse = courses.FirstOrDefault(c => c.Id == id && !c.IsDeleted)
                ?? throw new Exception($"This Course is not found with Id = {id}");
        }

        existCourse.UpdatedAt = DateTime.UtcNow;
        existCourse.TeacherId = course.TeacherId;
        existCourse.CourseName = course.CourseName;
        existCourse.Description = course.Description;

        await FileIO.WriteAsync(Constantas.COURSE_PATH, courses);

        return existCourse.MapTo<CourseViewModel>();
    }
}
