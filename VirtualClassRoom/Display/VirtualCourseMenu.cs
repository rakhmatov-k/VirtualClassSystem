using Spectre.Console;
using VirtualClassRoom.Models.VirtualCourse;
using VirtualClassRoom.Services;

namespace VirtualClassRoom.Display;

public class VirtualCourseMenu
{
    private readonly CourseService courseService;
    private readonly CourseStudentService courseStudentService;
    private readonly VirtualCourseService virtualCourseService;
    private readonly StudentService studentService;
    public VirtualCourseMenu(VirtualCourseService virtualCourseService, CourseService courseService, CourseStudentService courseStudentService, StudentService studentService)
    {
        this.courseService = courseService;
        this.virtualCourseService = virtualCourseService;
        this.courseStudentService = courseStudentService;
        this.studentService = studentService;
    }

    public async ValueTask DisplayAsync()
    {
        bool circle = true;
        while (circle)
        {
            AnsiConsole.Clear();
            var selectedOption = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("--VirtualCourseMenu--")
                    .PageSize(10)
                    .AddChoices("Create", "GetAll", "GetById", "Back")
            );
            switch (selectedOption)
            {
                case "Create":
                    await CreateAsync();
                    break;
                case "GetById":
                    await GetByIdAsync();
                    break;
                case "GetAll":
                    await GetAllAsync();
                    break;
                case "Back":
                    circle = false;
                    break;
            }
        }
    }

    async ValueTask CreateAsync()
    {
        Console.Clear();
        var table = new Table();
        table.AddColumn("[slateblue1]CourseId[/]");
        table.AddColumn("[slateblue1]TeacherId[/]");
        table.AddColumn("[slateblue1]CourseName[/]");

        var courses = await courseService.GetAllAsync();

        foreach (var item in courses)
        {
            table.AddRow(item.Id.ToString(), item.TeacherId.ToString(), item.CourseName);
        }
        Console.WriteLine("Courses");
        AnsiConsole.Write(table);

        long courseId = AnsiConsole.Ask<long>("Enter courseId : ");
        while (courseId <= 0)
        {
            AnsiConsole.MarkupLine("Was entered in the wrong format .Try again!");
            courseId = AnsiConsole.Ask<long>("Enter courseId : ");
        }


        VirtualCourseCreationModel model = new()
        {
            CourceId = courseId,
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddHours(2),
        };

        try
        {
            var virtualCourse = await virtualCourseService.CreateAsync(model);
            var students = await courseStudentService.GetByCourseIdAsync(model.CourceId);

            var studentTable = new Table();
            studentTable.AddColumn("[slateblue1]StudentId[/]");
            studentTable.AddColumn("[slateblue1]FirstName[/]");
            studentTable.AddColumn("[slateblue1]LastName[/]");
            studentTable.AddColumn("[slateblue1]Email[/]");

            foreach (var studentId in students)
            {
                var student = await studentService.GetByIdAsync(studentId.StudentId);
                studentTable.AddRow(student.Id.ToString(), student.FirstName, student.LastName, student.Email);
            }
            Console.WriteLine("\nAll CourseStudents ");
            AnsiConsole.Write(studentTable);
            bool myBool = true;
            while (myBool)
            {
                long studentId = AnsiConsole.Ask<long>("Enter studentId : ");
                while (courseId <= 0)
                {
                    AnsiConsole.MarkupLine("Was entered in the wrong format .Try again!");
                    studentId = AnsiConsole.Ask<long>("Enter studentId : ");
                }

                await virtualCourseService.AttendedAsync(virtualCourse.Id, studentId);
                Console.WriteLine("1.Continue\n2.Finish");
                string choice = AnsiConsole.Ask<string>("Enter choice : ");
                if (choice == "2")
                    myBool = false;
            }
        }

        catch (Exception ex)
        {
            AnsiConsole.Markup($"[red]{ex.Message}[/]\n");
        }
        Console.WriteLine("Enter any keyword to continue");
        Console.ReadKey();
        Console.Clear();
    }

    async ValueTask GetAllAsync()
    {
        Console.Clear();

        var table = new Table();
        table.AddColumn("[slateblue1]Id[/]");
        table.AddColumn("[slateblue1]CourseId[/]");
        table.AddColumn("[slateblue1]StartTime[/]");
        table.AddColumn("[slateblue1]EndTime[/]");
        table.AddColumn("[slateblue1]AttendedStudents[/]");

        var virtualCourses = await virtualCourseService.GetAllAsync();

        foreach (var item in virtualCourses)
        {
            table.AddRow(item.Id.ToString(), item.CourceId.ToString(), item.StartTime.ToString(), item.EndTime.ToString(), item.studentCount.ToString());
        }

        AnsiConsole.Write(table);
        Console.WriteLine("Enter any keyword to continue");
        Console.ReadKey();
        Console.Clear();
    }

    async ValueTask GetByIdAsync()
    {
        Console.Clear();
        long id = AnsiConsole.Ask<long>("Enter virtualCourse Id: ");
        while (id <= 0)
        {
            AnsiConsole.MarkupLine("Was entered in the wrong format .Try again!");
            id = AnsiConsole.Ask<long>("Enter virtualCourse Id: ");
        }

        try
        {
            var virtualCourse = await virtualCourseService.GetByIdAsync(id);
            var table = new Table();

            table.AddColumn("[slateblue1]Id[/]");
            table.AddColumn("[slateblue1]CourseId[/]");
            table.AddColumn("[slateblue1]StartTime[/]");
            table.AddColumn("[slateblue1]EndTime[/]");
            table.AddColumn("[slateblue1]AttendedStudents[/]");
            table.AddRow(virtualCourse.Id.ToString(), virtualCourse.CourceId.ToString(), virtualCourse.StartTime.ToString(), virtualCourse.EndTime.ToString(), virtualCourse.studentCount.ToString());
            AnsiConsole.Write(table);
        }

        catch (Exception ex)
        {
            AnsiConsole.Markup($"[red]{ex.Message}[/]\n");
        }

        Console.WriteLine("Enter any keyword to continue");
        Console.ReadKey();
        Console.Clear();
    }
}
