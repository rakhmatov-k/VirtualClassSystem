using Spectre.Console;
using System.Text.RegularExpressions;
using VirtualClassRoom.Models.Student;
using VirtualClassRoom.Services;

namespace VirtualClassRoom.Display;

public class StudentMenu
{
    private readonly StudentService studentService;
    public StudentMenu(StudentService studentService)
    {
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
                    .Title("--StudentMenu--")
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

        StudentCreationModel student = new()
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = password,
        };

        try
        {
            var addedStudent = await studentService.CreateAsync(student);
            AnsiConsole.Markup("[orange3]Successful created[/]\n");

            var table = new Table();
            table.AddColumn("[slateblue1]Id[/]");
            table.AddColumn("[slateblue1]FirstName[/]");
            table.AddColumn("[slateblue1]LastName[/]");
            table.AddColumn("[slateblue1]Email[/]");


            table.AddRow(addedStudent.Id.ToString(), addedStudent.FirstName, addedStudent.LastName, addedStudent.Email);
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
        long id = AnsiConsole.Ask<long>("Enter student Id to update: ");
        while (id <= 0)
        {
            AnsiConsole.MarkupLine("Was entered in the wrong format .Try again!");
            id = AnsiConsole.Ask<long>("Enter student Id to update: ");
        }
        string firstName = AnsiConsole.Ask<string>("FirstName : ");
        string lastName = AnsiConsole.Ask<string>("LastName : ");

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

        StudentUpdateModel student = new()
        {
            Email = email,
            Password = password,
            LastName = lastName,
            FirstName = firstName
        };
        try
        {
            var updatedStudent = await studentService.UpdateAsync(id, student);
            AnsiConsole.Markup("[orange3]Successful updated[/]\n");

            var table = new Table();
            table.AddColumn("[slateblue1]Id[/]");
            table.AddColumn("[slateblue1]FirstName[/]");
            table.AddColumn("[slateblue1]LastName[/]");
            table.AddColumn("[slateblue1]Email[/]");


            table.AddRow(updatedStudent.Id.ToString(), updatedStudent.FirstName, updatedStudent.LastName, updatedStudent.Email);
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
        table.AddColumn("[slateblue1]Email[/]");

        var students = await studentService.GetAllAsync();

        foreach (var student in students)
        {
            table.AddRow(student.Id.ToString(), student.FirstName, student.LastName, student.Email);
        }

        AnsiConsole.Write(table);
        Console.WriteLine("Enter any keyword to continue");
        Console.ReadKey();
        Console.Clear();
    }

    async ValueTask DeleteAsync()
    {
        Console.Clear();
        long id = AnsiConsole.Ask<long>("Enter student Id to delete: ");
        while (id <= 0)
        {
            AnsiConsole.MarkupLine("Was entered in the wrong format .Try again!");
            id = AnsiConsole.Ask<long>("Enter student Id to delete: ");
        }

        try
        {
            await studentService.DeleteAsync(id);
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
        long id = AnsiConsole.Ask<long>("Enter student Id: ");
        while (id <= 0)
        {
            AnsiConsole.MarkupLine("Was entered in the wrong format .Try again!");
            id = AnsiConsole.Ask<long>("Enter student Id: ");
        }

        try
        {
            var student = await studentService.GetByIdAsync(id);
            var table = new Table();

            table.AddColumn("[slateblue1]Id[/]");
            table.AddColumn("[slateblue1]FirstName[/]");
            table.AddColumn("[slateblue1]LastName[/]");
            table.AddColumn("[slateblue1]Email[/]");
            table.AddRow(student.Id.ToString(), student.FirstName, student.LastName, student.Email);
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
