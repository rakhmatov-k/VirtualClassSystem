using VirtualClassRoom.Models.Commons;

namespace VirtualClassRoom.Models.Teacher;

public class TeacherModel : Auditable
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Description { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
