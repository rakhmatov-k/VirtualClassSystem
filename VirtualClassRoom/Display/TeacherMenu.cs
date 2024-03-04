using Spectre.Console;
using System.Text.RegularExpressions;
using VirtualClassRoom.Models.Teacher;
using VirtualClassRoom.Services;

namespace VirtualClassRoom.Display;

public class TeacherMenu
{
    private readonly TeacherService teacherService;
    public TeacherMenu(TeacherService teacherService)
    {
        this.teacherService = teacherService;
    }

    public async ValueTask DisplayAsync()
    {
        bool circle = true;
        while (circle)
        {
            AnsiConsole.Clear();
            var selectedOption = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("--TeacherMenu--")
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
        string firstName = AnsiConsole.Ask<string>("FirstName:");
        string lastName = AnsiConsole.Ask<string>("LastName:");
        string description = AnsiConsole.Ask<string>("Description");

        Console.Write("Enter  Email (email@gmail.com):");
        string email = Console.ReadLine();
        while (!Regex.IsMatch(email, @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]{3,}$"))
        {
            Console.WriteLine("Was entered in the wrong format. Try again!");
            Console.Write("Enter new Email (email@gmail.com):");
            email = Console.ReadLine();
        }

        string password = AnsiConsole.Prompt(
        new TextPrompt<string>("Enter password :")
         .PromptStyle("red").Secret());

        TeacherCreationModel teacher = new()
        {
            Email = email,
            Password = password,
            LastName = lastName,
            FirstName = firstName,
            Description = description
        };

        try
        {
            var addedTeacher = await teacherService.CreateAsync(teacher);
            AnsiConsole.Markup("[orange3]Successful created[/]\n");

            var table = new Table();
            table.AddColumn("[slateblue1]Id[/]");
            table.AddColumn("[slateblue1]FirstName[/]");
            table.AddColumn("[slateblue1]LastName[/]");
            table.AddColumn("[slateblue1]Description[/]");
            table.AddColumn("[slateblue1]Email[/]");


            table.AddRow(addedTeacher.Id.ToString(), addedTeacher.FirstName, addedTeacher.LastName, addedTeacher.Description, addedTeacher.Email);
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

    async ValueTask UpdateAsync()
    {
        Console.Clear();
        long id = AnsiConsole.Ask<long>("Enter teacher Id to update: ");
        while (id <= 0)
        {
            AnsiConsole.MarkupLine("Was entered in the wrong format .Try again!");
            id = AnsiConsole.Ask<long>("Enter teacher Id to update: ");
        }
        string firstName = AnsiConsole.Ask<string>("FirstName : ");
        string lastName = AnsiConsole.Ask<string>("LastName : ");
        string description = AnsiConsole.Ask<string>("Description");

        Console.Write("Enter new Email (email@gmail.com):");
        string email = Console.ReadLine();
        while (!Regex.IsMatch(email, @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]{3,}$"))
        {
            Console.WriteLine("Was entered in the wrong format .Try again !");
            Console.Write("Enter new Email (email@gmail.com):");
            email = Console.ReadLine();
        }

        string password = AnsiConsole.Prompt(
        new TextPrompt<string>("Enter password :")
         .PromptStyle("red").Secret());

        TeacherUpdateModel teacher = new()
        {
            Email = email,
            Password = password,
            LastName = lastName,
            FirstName = firstName,
            Description = description
        };

        try
        {
            var updatedTeacher = await teacherService.UpdateAsync(id, teacher);
            AnsiConsole.Markup("[orange3]Successful updated[/]\n");

            var table = new Table();
            table.AddColumn("[slateblue1]Id[/]");
            table.AddColumn("[slateblue1]FirstName[/]");
            table.AddColumn("[slateblue1]LastName[/]");
            table.AddColumn("[slateblue1]Description[/]");
            table.AddColumn("[slateblue1]Email[/]");


            table.AddRow(updatedTeacher.Id.ToString(), updatedTeacher.FirstName, updatedTeacher.LastName, updatedTeacher.Description, updatedTeacher.Email);
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
        Console.WriteLine("Enter any keyword to continue");
        Console.ReadKey();
        Console.Clear();
    }

    async ValueTask DeleteAsync()
    {
        Console.Clear();
        long id = AnsiConsole.Ask<long>("Enter teacher Id to delete: ");
        while (id <= 0)
        {
            AnsiConsole.MarkupLine("Was entered in the wrong format .Try again!");
            id = AnsiConsole.Ask<long>("Enter teacher Id to delete: ");
        }

        try
        {
            await teacherService.DeleteAsync(id);
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

    async ValueTask GetByIdAsync()
    {
        Console.Clear();
        long id = AnsiConsole.Ask<long>("Enter teacher Id: ");
        while (id <= 0)
        {
            AnsiConsole.MarkupLine("Was entered in the wrong format .Try again!");
            id = AnsiConsole.Ask<long>("Enter teacher Id: ");
        }

        try
        {
            var teacher = await teacherService.GetByIdAsync(id);
            var table = new Table();

            table.AddColumn("[slateblue1]Id[/]");
            table.AddColumn("[slateblue1]FirstName[/]");
            table.AddColumn("[slateblue1]LastName[/]");
            table.AddColumn("[slateblue1]Description[/]");
            table.AddColumn("[slateblue1]Email[/]");
            table.AddRow(teacher.Id.ToString(), teacher.FirstName, teacher.LastName, teacher.Description, teacher.Email);
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
