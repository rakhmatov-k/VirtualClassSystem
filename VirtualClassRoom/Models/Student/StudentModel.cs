using VirtualClassRoom.Models.Commons;

namespace VirtualClassRoom.Models.Student;

public class StudentModel : Auditable
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
