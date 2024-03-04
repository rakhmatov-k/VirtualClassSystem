using VirtualClassRoom.Models.Commons;

namespace VirtualClassRoom.Models.CourseStudents;

public class CourseStudentsModel : Auditable
{
    public long CourseId { get; set; }
    public long StudentId { get; set; }
}
