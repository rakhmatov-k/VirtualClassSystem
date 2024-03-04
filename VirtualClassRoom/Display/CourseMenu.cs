using Spectre.Console;
using System.Text.RegularExpressions;
using VirtualClassRoom.Interfaces;
using VirtualClassRoom.Models.Course;
using VirtualClassRoom.Models.Student;
using VirtualClassRoom.Services;
using static System.Collections.Specialized.BitVector32;

namespace VirtualClassRoom.Display;

public class CourseMenu
{
    private readonly TeacherService teacherService;
    private readonly CourseService courseService;
    public CourseMenu(TeacherService teacherService, CourseService courseService)
    {
        this.teacherService = teacherService;
        this.courseService = courseService;
    }

    public async ValueTask DisplayAsync()
    {
        bool circle = true;
        while (circle)
        {
            AnsiConsole.Clear();
            var selectedOption = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("--CourseMenu--")
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
        table.AddColumn("[slateblue1]Description[/]");
        table.AddColumn("[slateblue1]Email[/]");

        var teachers = await teacherService.GetAllAsync();

        foreach (var teacher in teachers)
        {
            table.AddRow(teacher.Id.ToString(), teacher.FirstName, teacher.LastName, teacher.Description, teacher.Email);
        }

        AnsiConsole.Write(table);

        long teacherId = AnsiConsole.Ask<long>("Enter teacherId : ");
        while (teacherId <= 0)
        {
            AnsiConsole.MarkupLine("Was entered in the wrong format .Try again!");
            teacherId = AnsiConsole.Ask<long>("Enter teacherId : ");
        }
        string courseName = AnsiConsole.Ask<string>("CourseName:");
        string description = AnsiConsole.Ask<string>("Description:");


        CourseCreationModel course = new()
        {
            TeacherId = teacherId,
            CourseName = courseName,
            Description = description
        };

        try
        {
            var addedCourse = await courseService.CreateAsync(course);
            AnsiConsole.Markup("[orange3]Successful created[/]\n");

            var table2 = new Table();
            table2.AddColumn("[slateblue1]Id[/]");
            table2.AddColumn("[slateblue1]CourseName[/]");
            table2.AddColumn("[slateblue1]Description[/]");
            table2.AddColumn("[slateblue1]TeacherId[/]");


            table2.AddRow(addedCourse.Id.ToString(), addedCourse.CourseName, addedCourse.Description, addedCourse.TeacherId.ToString());
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
        long id = AnsiConsole.Ask<long>("Enter course Id to update: ");
        while (id <= 0)
        {
            AnsiConsole.MarkupLine("Was entered in the wrong format .Try again!");
            id = AnsiConsole.Ask<long>("Enter course Id to update: ");
        }

        var table2 = new Table();
        table2.AddColumn("[slateblue1]Id[/]");
        table2.AddColumn("[slateblue1]FirstName[/]");
        table2.AddColumn("[slateblue1]LastName[/]");
        table2.AddColumn("[slateblue1]Description[/]");
        table2.AddColumn("[slateblue1]Email[/]");

        var teachers = await teacherService.GetAllAsync();

        foreach (var teacher in teachers)
        {
            table2.AddRow(teacher.Id.ToString(), teacher.FirstName, teacher.LastName, teacher.Description, teacher.Email);
        }

        AnsiConsole.Write(table2);

        long teacherId = AnsiConsole.Ask<long>("Enter teacherId : ");
        while (teacherId <= 0)
        {
            AnsiConsole.MarkupLine("Was entered in the wrong format .Try again!");
            teacherId = AnsiConsole.Ask<long>("Enter teacherId : ");
        }
        string courseName = AnsiConsole.Ask<string>("CourseName:");
        string description = AnsiConsole.Ask<string>("Description:");

        CourseUpdateModel course = new()
        {
            TeacherId = teacherId,
            CourseName = courseName,
            Description = description
        };
        try
        {
            var updatedCourse = await courseService.UpdateAsync(id, course);
            AnsiConsole.Markup("[orange3]Successful updated[/]\n");

            var table = new Table();
            table.AddColumn("[slateblue1]Id[/]");
            table.AddColumn("[slateblue1]CourseName[/]");
            table.AddColumn("[slateblue1]Description[/]");
            table.AddColumn("[slateblue1]TeacherId[/]");


            table.AddRow(updatedCourse.Id.ToString(), updatedCourse.CourseName, updatedCourse.Description, updatedCourse.TeacherId.ToString());
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

    async ValueTask GetByIdAsync()
    {
        Console.Clear();
        long id = AnsiConsole.Ask<long>("Enter course Id: ");
        while (id <= 0)
        {
            AnsiConsole.MarkupLine("Was entered in the wrong format .Try again!");
            id = AnsiConsole.Ask<long>("Enter course Id: ");
        }

        try
        {
            var course = await courseService.GetByIdAsync(id);
            var table = new Table();

            table.AddColumn("[slateblue1]Id[/]");
            table.AddColumn("[slateblue1]CourseName[/]");
            table.AddColumn("[slateblue1]Description[/]");
            table.AddColumn("[slateblue1]TeacherId[/]");

            table.AddRow(course.Id.ToString(), course.CourseName, course.Description, course.TeacherId.ToString());
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
        table.AddColumn("[slateblue1]CourseName[/]");
        table.AddColumn("[slateblue1]Description[/]");
        table.AddColumn("[slateblue1]TeacherId[/]");

        var courses = await courseService.GetAllAsync();

        foreach (var item in courses)
        {
            table.AddRow(item.Id.ToString(), item.CourseName, item.Description, item.TeacherId.ToString());
        }

        AnsiConsole.Write(table);
        Console.WriteLine("Enter any keyword to continue");
        Console.ReadKey();
        Console.Clear();
    }

    async ValueTask DeleteAsync()
    {
        Console.Clear();
        long id = AnsiConsole.Ask<long>("Enter course Id to delete: ");
        while (id <= 0)
        {
            AnsiConsole.MarkupLine("Was entered in the wrong format .Try again!");
            id = AnsiConsole.Ask<long>("Enter course Id to delete: ");
        }

        try
        {
            await courseService.DeleteAsync(id);
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

