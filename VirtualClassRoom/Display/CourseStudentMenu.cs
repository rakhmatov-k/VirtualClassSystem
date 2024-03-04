using Spectre.Console;
using VirtualClassRoom.Models.CourseStudents;
using VirtualClassRoom.Services;

namespace VirtualClassRoom.Display;

public class CourseStudentMenu
{
    private readonly CourseService courseService;
    private readonly StudentService studentService;
    private readonly CourseStudentService courseStudentService;
    public CourseStudentMenu(CourseService courseService, StudentService studentService, CourseStudentService courseStudentService)
    {
        this.courseService = courseService;
        this.studentService = studentService;
        this.courseStudentService = courseStudentService;
    }

    public async ValueTask DisplayAsync()
    {
        bool circle = true;
        while (circle)
        {
            AnsiConsole.Clear();
            var selectedOption = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("--CourseStudentMenu--")
                    .PageSize(10)
                    .AddChoices("Create", "GetById", "Update", "GetAll", "Delete", "Back")
            );
            switch (selectedOption)
            {
                case "Create":
                    await CreateAsync();
                    break;
                case "GetById":
                    await GetByIdAsync();
                    break;
                case "Update":
                    await UpdateAsync();
                    break;
                case "Delete":
                    await DeleteAsync();
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
        table.AddColumn("[slateblue1]Id[/]");
        table.AddColumn("[slateblue1]FirstName[/]");
        table.AddColumn("[slateblue1]LastName[/]");
        table.AddColumn("[slateblue1]Email[/]");

        var students = await studentService.GetAllAsync();

        foreach (var student in students)
        {
            table.AddRow(student.Id.ToString(), student.FirstName, student.LastName, student.Email);
        }

        AnsiConsole.Write(table);

        long studentId = AnsiConsole.Ask<long>("Enter student id : ");
        while (studentId <= 0)
        {
            AnsiConsole.MarkupLine("Was entered in the wrong format .Try again!");
            studentId = AnsiConsole.Ask<long>("Enter student Id : ");
        }

        var table1 = new Table();
        table1.AddColumn("[slateblue1]Id[/]");
        table1.AddColumn("[slateblue1]CourseName[/]");
        table1.AddColumn("[slateblue1]Description[/]");
        table1.AddColumn("[slateblue1]TeacherId[/]");

        var courses = await courseService.GetAllAsync();

        foreach (var item in courses)
        {
            table1.AddRow(item.Id.ToString(), item.CourseName, item.Description, item.TeacherId.ToString());
        }

        AnsiConsole.Write(table1);

        long courseId = AnsiConsole.Ask<long>("Enter course id : ");
        while (courseId <= 0)
        {
            AnsiConsole.MarkupLine("Was entered in the wrong format .Try again!");
            courseId = AnsiConsole.Ask<long>("Enter course Id : ");
        }

        CourseStudentsCreationModel courseStudent = new()
        {
            StudentId = studentId,
            CourseId = courseId
        };

        try
        {
            var addedCourse = await courseStudentService.CreateAsync(courseStudent);
            AnsiConsole.Markup("[orange3]Successful created[/]\n");

            var table2 = new Table();
            table2.AddColumn("[slateblue1]Id[/]");
            table2.AddColumn("[slateblue1]StudentId[/]");
            table2.AddColumn("[slateblue1]CourseId[/]");


            table2.AddRow(addedCourse.Id.ToString(), addedCourse.StudentId.ToString(), addedCourse.CourseId.ToString());
            AnsiConsole.Write(table2);
        }

        catch (Exception ex)
        {
            AnsiConsole.Markup($"[red]{ex.Message}[/]\n");
        }
        Console.WriteLine("Enter any keyword to continue");
        Console.ReadKey();
        Console.Clear();
    }

    async ValueTask UpdateAsync()
    {
        Console.Clear();
        long id = AnsiConsole.Ask<long>("Enter courseStudent Id to update: ");
        while (id <= 0)
        {
            AnsiConsole.MarkupLine("Was entered in the wrong format .Try again!");
            id = AnsiConsole.Ask<long>("Enter courseStudent Id to update: ");
        }

        var table = new Table();
        table.AddColumn("[slateblue1]Id[/]");
        table.AddColumn("[slateblue1]FirstName[/]");
        table.AddColumn("[slateblue1]LastName[/]");
        table.AddColumn("[slateblue1]Email[/]");

        var students = await studentService.GetAllAsync();

        foreach (var student in students)
        {
            table.AddRow(student.Id.ToString(), student.FirstName, student.LastName, student.Email);
        }

        AnsiConsole.Write(table);

        long studentId = AnsiConsole.Ask<long>("Enter student id : ");
        while (studentId <= 0)
        {
            AnsiConsole.MarkupLine("Was entered in the wrong format .Try again!");
            studentId = AnsiConsole.Ask<long>("Enter student Id : ");
        }

        var table1 = new Table();
        table1.AddColumn("[slateblue1]Id[/]");
        table1.AddColumn("[slateblue1]CourseName[/]");
        table1.AddColumn("[slateblue1]Description[/]");
        table1.AddColumn("[slateblue1]TeacherId[/]");

        var courses = await courseService.GetAllAsync();

        foreach (var item in courses)
        {
            table1.AddRow(item.Id.ToString(), item.CourseName, item.Description, item.TeacherId.ToString());
        }

        AnsiConsole.Write(table1);

        long courseId = AnsiConsole.Ask<long>("Enter course id : ");
        while (courseId <= 0)
        {
            AnsiConsole.MarkupLine("Was entered in the wrong format .Try again!");
            courseId = AnsiConsole.Ask<long>("Enter course Id : ");
        }

        CourseStudentsUpdateModel course = new()
        {
            StudentId = studentId,
            CourseId = courseId
        };
        try
        {
            var updatedCourse = await courseStudentService.UpdateAsync(id, course);
            AnsiConsole.Markup("[orange3]Successful updated[/]\n");

            var table2 = new Table();
            table2.AddColumn("[slateblue1]Id[/]");
            table2.AddColumn("[slateblue1]StudentId[/]");
            table2.AddColumn("[slateblue1]CourseId[/]");


            table2.AddRow(updatedCourse.Id.ToString(), updatedCourse.StudentId.ToString(), updatedCourse.CourseId.ToString());
            AnsiConsole.Write(table2);
        }
        catch (Exception ex)
        {
            AnsiConsole.Markup($"[red]{ex.Message}[/]\n");
        }
        Console.WriteLine("Enter any keyword to continue");
        Console.ReadKey();
        Console.Clear();
    }

    async ValueTask GetByIdAsync()
    {
        Console.Clear();
        long id = AnsiConsole.Ask<long>("Enter courseStudent Id: ");
        while (id <= 0)
        {
            AnsiConsole.MarkupLine("Was entered in the wrong format .Try again!");
            id = AnsiConsole.Ask<long>("Enter courseStudent Id: ");
        }

        try
        {
            var course = await courseStudentService.GetByIdAsync(id);
            var table = new Table();

            table.AddColumn("[slateblue1]Id[/]");
            table.AddColumn("[slateblue1]StudentId[/]");
            table.AddColumn("[slateblue1]CourseId[/]");

            table.AddRow(course.Id.ToString(), course.StudentId.ToString(), course.CourseId.ToString());
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

    async ValueTask GetAllAsync()
    {
        Console.Clear();

        var table = new Table();
        table.AddColumn("[slateblue1]Id[/]");
        table.AddColumn("[slateblue1]StudentId[/]");
        table.AddColumn("[slateblue1]CourseId[/]");

        var courses = await courseStudentService.GetAllAsync();

        foreach (var item in courses)
        {
            table.AddRow(item.Id.ToString(), item.StudentId.ToString(), item.CourseId.ToString());
        }

        AnsiConsole.Write(table);
        Console.WriteLine("Enter any keyword to continue");
        Console.ReadKey();
        Console.Clear();
    }

    async ValueTask DeleteAsync()
    {
        Console.Clear();
        long id = AnsiConsole.Ask<long>("Enter courseStudent Id to delete: ");
        while (id <= 0)
        {
            AnsiConsole.MarkupLine("Was entered in the wrong format .Try again!");
            id = AnsiConsole.Ask<long>("Enter course Id to delete: ");
        }

        try
        {
            await courseStudentService.DeleteAsync(id);
            AnsiConsole.Markup("[orange3]Successful deleted[/]\n");
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
