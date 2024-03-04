using Spectre.Console;
using VirtualClassRoom.Services;

namespace VirtualClassRoom.Display;

public class MainMenu
{
    private readonly TeacherMenu teacherMenu;
    private readonly CourseMenu courseMenu;
    private readonly StudentMenu studentMenu;
    private readonly CourseStudentMenu courseStudentMenu;
    private readonly VirtualCourseMenu virtualCourseMenu;

    private readonly TeacherService teacherService;
    private readonly CourseService courseService;
    private readonly StudentService studentService;
    private readonly CourseStudentService courseStudentService;
    private readonly VirtualCourseService virtualCourseService;

    public MainMenu()
    {
        teacherService = new TeacherService();
        courseService = new CourseService(teacherService);
        studentService = new StudentService();
        courseStudentService = new CourseStudentService(courseService, studentService);
        virtualCourseService = new VirtualCourseService(courseService, courseStudentService);

        teacherMenu = new TeacherMenu(teacherService);
        courseMenu = new CourseMenu(teacherService, courseService);
        studentMenu = new StudentMenu(studentService);
        courseStudentMenu = new CourseStudentMenu(courseService, studentService, courseStudentService);
        virtualCourseMenu = new VirtualCourseMenu(virtualCourseService, courseService, courseStudentService, studentService);
    }
    public async ValueTask MainAsync()
    {
        bool circle = true;
        while (circle)
        {
            AnsiConsole.Clear();
            var selectedOption = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("--MainMenu--")
                    .PageSize(10)
                    .AddChoices("Teacher", "Course", "Student", "CourseStudent", "VirtualCourse", "ChatMessage", "Back")
            );
            switch (selectedOption)
            {
                case "Teacher":
                    await teacherMenu.DisplayAsync();
                    break;
                case "Course":
                    await courseMenu.DisplayAsync();
                    break;
                case "Student":
                    await studentMenu.DisplayAsync();
                    break;
                case "CourseStudent":
                    await courseStudentMenu.DisplayAsync();
                    break;
                case "VirtualCourse":
                    await virtualCourseMenu.DisplayAsync();
                    break;
                case "Back":
                    circle = false;
                    break;
            }
        }
    }
}
